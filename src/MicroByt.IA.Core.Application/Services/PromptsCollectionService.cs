using MicroByt.IA.Core.Application.Interfaces;

namespace MicroByt.IA.Core.Application.Services;

/// <summary>Implementación de <see cref="IPromptsCollectionService"/> que carga cada prompt desde su fichero Markdown bajo demanda.</summary>
public class PromptsCollectionService : IPromptsCollectionService
{
    private const string PromptsDirectory = "Data/Prompts";

    private readonly Dictionary<string, string> _cache = [];

    /// <summary>Obtiene el contenido del prompt cuyo nombre de fichero (sin extensión) coincide con <paramref name="promptName"/>.</summary>
    /// <param name="promptName">Nombre del fichero de prompt sin extensión (p.ej. "SelectSkills").</param>
    /// <returns>Contenido del fichero Markdown, o <see langword="null"/> si no existe ningún prompt con ese nombre.</returns>
    public string? GetPrompt(string promptName)
    {
        if (_cache.TryGetValue(promptName, out var cached))
            return cached;

        var path = Path.Combine(AppContext.BaseDirectory, PromptsDirectory, $"{promptName}.md");

        if (!File.Exists(path))
            return null;

        var content = File.ReadAllText(path);

        if (content.StartsWith("# Prompt:"))
        {
            var newlineIndex = content.IndexOf('\n');
            content = newlineIndex >= 0 ? content[(newlineIndex + 1)..] : string.Empty;
        }

        _cache[promptName] = content;
        return content;
    }
}