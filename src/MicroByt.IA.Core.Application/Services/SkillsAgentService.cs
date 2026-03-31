using System.Text;
using MicroByt.IA.Core.Application.Interfaces;
using MicroByt.IA.Core.Application.Models;
using MicroByt.IA.Core.Entities.AgentSkills;
using OpenAI.Chat;

namespace MicroByt.IA.Core.Application.Services;

/// <summary>Implementación de <see cref="ISkillsAgentService"/> que usa un LLM para seleccionar las skills necesarias para una tarea.</summary>
public class SkillsAgentService(
    IProviderChatClientFactory chatClientFactory,
    IPromptsCollectionService promptsService,
    ISkillsToolsCollectionService skillsService)
    : ISkillsAgentService
{
    /// <inheritdoc/>
    public async Task<Skill[]?> Select(SkillsAgentInput input)
    {
        var promptTemplate = promptsService.GetPrompt("SelectSkills");
        if (promptTemplate is null)
            return null;

        var skills = skillsService.Skills;
        if (skills is null || skills.Length == 0)
            return null;

        var skillsText = string.Join("\n", skills.Select(s => $"{s.Name}: {s.Description}"));
        var systemPrompt = promptTemplate.Replace("{Skills}", skillsText);

        var messages = new List<ChatMessage>
        {
            new SystemChatMessage(systemPrompt),
            new UserChatMessage(input.Task),
        };

        var chatClient = chatClientFactory.GetClient(input.Model);
        var responseBuilder = new StringBuilder();

        await foreach (var update in chatClient.CompleteChatStreamingAsync(messages))
        {
            foreach (var part in update.ContentUpdate)
                responseBuilder.Append(part.Text);
        }

        var skillsByName = skills.ToDictionary(s => s.Name);

        return responseBuilder.ToString()
            .Split('\n', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
            .Select(line => skillsByName.GetValueOrDefault(line))
            .OfType<Skill>()
            .ToArray();
    }
}