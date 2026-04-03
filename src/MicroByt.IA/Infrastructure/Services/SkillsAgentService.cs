using System.Text;
using System.Text.Json;
using MicroByt.IA.Application.Interfaces;
using MicroByt.IA.Infrastructure.Interfaces;
using MicroByt.IA.Domain.AgentSkills;
using MicroByt.IA.Domain.AI;
using OpenAI.Chat;

namespace MicroByt.IA.Infrastructure.Services;

/// <summary>Implementación de <see cref="ISkillsAgentService"/> que usa un LLM para seleccionar las skills necesarias para una tarea.</summary>
public class SkillsAgentService(
    IProviderChatClientFactory chatClientFactory,
    IPromptsCollectionService promptsService,
    ISkillsService skillsService,
    IToolChainRegistry toolChainRegistry)
    : ISkillsAgentService
{
    private readonly IProviderChatClientFactory _chatClientFactory = chatClientFactory;
    private readonly IPromptsCollectionService _promptsService = promptsService;
    private readonly ISkillsService _skillsService = skillsService;
    private readonly IToolChainRegistry _toolChainRegistry = toolChainRegistry;

    /// <inheritdoc/>
    public async Task<string> RunAgent(AIModel model, string task)
    {
        var promptTemplate = _promptsService.GetPrompt("Agent");
        if (promptTemplate is null)
            return string.Empty;

        var skills = _skillsService.GetEligibleSkills();
        if (skills.Length == 0)
            return string.Empty;

        var skillsText = string.Join("\n", skills.Select(FormatSkill));
        var systemPrompt = promptTemplate.Replace("{ActiveSkills}", skillsText);

        var messages = new List<ChatMessage>
        {
            new SystemChatMessage(systemPrompt),
            new UserChatMessage(task),
        };

        var options = BuildChatOptions();
        var chatClient = _chatClientFactory.GetClient(model);
        var responseBuilder = new StringBuilder();

        while (true)
        {
            var toolCallAccumulators = new Dictionary<int, (string Id, string Name, StringBuilder Args)>();
            var finishReason = ChatFinishReason.Stop;

            await foreach (var update in chatClient.CompleteChatStreamingAsync(messages, options))
            {
                foreach (var part in update.ContentUpdate)
                    responseBuilder.Append(part.Text);

                foreach (var toolUpdate in update.ToolCallUpdates)
                {
                    if (!toolCallAccumulators.ContainsKey(toolUpdate.Index))
                        toolCallAccumulators[toolUpdate.Index] = (toolUpdate.ToolCallId ?? string.Empty, toolUpdate.FunctionName ?? string.Empty, new StringBuilder());

                    toolCallAccumulators[toolUpdate.Index].Args.Append(toolUpdate.FunctionArgumentsUpdate);
                }

                if (update.FinishReason.HasValue)
                    finishReason = update.FinishReason.Value;
            }

            if (finishReason != ChatFinishReason.ToolCalls || toolCallAccumulators.Count == 0)
                break;

            var toolCalls = toolCallAccumulators.Values
                .Select(acc => ChatToolCall.CreateFunctionToolCall(acc.Id, acc.Name, BinaryData.FromBytes(Encoding.UTF8.GetBytes(acc.Args.ToString()))))
                .ToList();

            messages.Add(new AssistantChatMessage(toolCalls));

            foreach (var acc in toolCallAccumulators.Values)
            {
                using var arguments = JsonDocument.Parse(acc.Args.ToString());
                var result = await _toolChainRegistry.ExecuteAsync(acc.Name, arguments);
                messages.Add(new ToolChatMessage(acc.Id, result));
            }
        }

        return responseBuilder.ToString();
    }

    private ChatCompletionOptions? BuildChatOptions()
    {
        var tools = _toolChainRegistry.GetTools();
        if (tools.Count == 0)
            return null;

        var options = new ChatCompletionOptions();
        foreach (var tool in tools)
            options.Tools.Add(ChatTool.CreateFunctionTool(tool.Name, tool.Description, BinaryData.FromBytes(Encoding.UTF8.GetBytes(tool.JsonSchema))));

        return options;
    }

    private string FormatSkill(Skill skill)
    {
        var instructions = skill.FilePath is not null
            ? _skillsService.LoadContentSkill(skill.FilePath)
            : null;

        var sb = new StringBuilder();
        sb.AppendLine($"- Skill: {skill.Name}");
        sb.AppendLine($"  Description: {skill.Description}");
        sb.AppendLine("  Instructions:");
        if (instructions is not null)
            foreach (var line in instructions.Split('\n'))
                sb.AppendLine($"  {line}");
        return sb.ToString();
    }
}