using System.Text;
using MicroByt.IA.Application.Interfaces;
using MicroByt.IA.Domain.AI;
using OpenAI.Chat;

namespace MicroByt.IA.Application.Services;

/// <summary>Implementación de <see cref="ISkillsAgentService"/> que usa un LLM para seleccionar las skills necesarias para una tarea.</summary>
public class SkillsAgentService(
    IProviderChatClientFactory chatClientFactory,
    IPromptsCollectionService promptsService,
    ISkillsToolsCollectionService skillsService)
    : ISkillsAgentService
{
    /// <inheritdoc/>
    public async Task Select(AIModel model, string task)
    {
        var promptTemplate = promptsService.GetPrompt("SelectSkills");
        if (promptTemplate is null)
            return;

        var skills = skillsService.Skills;
        if (skills is null || skills.Length == 0)
            return;

        var skillsText = string.Join("\n", skills.Select(s => $"{s.Name}: {s.Description}"));
        var systemPrompt = promptTemplate.Replace("{Skills}", skillsText);

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
}