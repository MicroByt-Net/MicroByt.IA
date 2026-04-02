using System.Text;
using MicroByt.IA.Application.Interfaces;
using MicroByt.IA.Domain.AgentSkills;
using MicroByt.IA.Domain.AI;
using OpenAI.Chat;

namespace MicroByt.IA.Application.Services;

/// <summary>Implementación de <see cref="ISkillsAgentService"/> que usa un LLM para seleccionar las skills necesarias para una tarea.</summary>
public class SkillsAgentService(
    IProviderChatClientFactory chatClientFactory,
    IPromptsCollectionService promptsService,
    ISkillsService skillsService)
    : ISkillsAgentService
{
    /// <inheritdoc/>
    public async Task RunAgent(AIModel model, string task)
    {
        var promptTemplate = promptsService.GetPrompt("Agent");
        if (promptTemplate is null)
            return;

        var skills = skillsService.GetEligibleSkills();
        if (skills.Length == 0)
            return;

        var skillsText = string.Join("\n", skills.Select(FormatSkill));
        var systemPrompt = promptTemplate.Replace("{ActiveSkills}", skillsText);

        var messages = new List<ChatMessage>
        {
            new SystemChatMessage(systemPrompt),
            new UserChatMessage(task),
        };

        var chatClient = chatClientFactory.GetClient(model);
        var responseBuilder = new StringBuilder();

        await foreach (var update in chatClient.CompleteChatStreamingAsync(messages))
        {
            foreach (var part in update.ContentUpdate)
                responseBuilder.Append(part.Text);
        }
    }

    private string FormatSkill(Skill skill)
    {
        var instructions = skill.FilePath is not null
            ? skillsService.LoadContentSkill(skill.FilePath)
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