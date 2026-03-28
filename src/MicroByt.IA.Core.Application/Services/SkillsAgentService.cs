using System.Text;
using System.Text.Json;
using MicroByt.AI.Core.Common.Helpers;
using MicroByt.IA.Core.Application.Interfaces;
using MicroByt.IA.Core.Entities.AgentSkills;
using MicroByt.IA.Core.Entities.Data.AI;
using OpenAI.Chat;

namespace MicroByt.IA.Core.Application.Services;

/// <summary>Implementación de <see cref="ISkillsAgentService"/> que usa un LLM para seleccionar las skills necesarias para una tarea.</summary>
public class SkillsAgentService : ISkillsAgentService
{
    private readonly IProviderChatClientFactory _chatClientFactory;
    private readonly IPromptsCollectionService _promptsService;
    private readonly ISkillsToolsCollectionService _skillsService;

    public SkillsAgentService(
        IProviderChatClientFactory chatClientFactory,
        IPromptsCollectionService promptsService,
        ISkillsToolsCollectionService skillsService)
    {
        _chatClientFactory = chatClientFactory;
        _promptsService = promptsService;
        _skillsService = skillsService;
    }

    /// <inheritdoc/>
    public async Task<Skill[]?> Select(AIModel model, string task)
    {
        var promptTemplate = _promptsService.GetPrompt("SelectSkills");
        if (promptTemplate is null)
            return null;

        var skills = _skillsService.Skills;
        if (skills is null || skills.Length == 0)
            return null;

        var skillsText = string.Join("\n", skills.Select(s => $"{s.Name}: {s.Description}"));
        var systemPrompt = promptTemplate.Replace("{Skills}", skillsText);

        var messages = new List<ChatMessage>
        {
            new SystemChatMessage(systemPrompt),
            new UserChatMessage(task),
        };

        var chatClient = _chatClientFactory.GetClient(model);
        var responseBuilder = new StringBuilder();

        await foreach (var update in chatClient.CompleteChatStreamingAsync(messages))
        {
            foreach (var part in update.ContentUpdate)
                responseBuilder.Append(part.Text);
        }

        var response = JsonHelper.Deserialize<SelectSkillsResponse>(
            responseBuilder.ToString(),
            new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower });

        if (response is null)
            return null;

        var skillsByName = skills.ToDictionary(s => s.Name);

        return response.SelectedSkills
            .Select(s => skillsByName.GetValueOrDefault(s.Skill))
            .OfType<Skill>()
            .ToArray();
    }

    private record SelectedSkill(string Skill);
    private record SelectSkillsResponse(SelectedSkill[] SelectedSkills);
}