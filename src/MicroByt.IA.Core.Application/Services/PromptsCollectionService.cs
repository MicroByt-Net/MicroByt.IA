using MicroByt.IA.Core.Application.Interfaces;

namespace MicroByt.IA.Core.Application.Services;

/// <summary>Implementación de <see cref="IPromptsCollectionService"/> que carga los prompts desde ficheros Markdown una única vez.</summary>
public class PromptsCollectionService : IPromptsCollectionService
{
    private const string PromptsDirectory = "Data/Prompts";

    private Dictionary<string, string>? _prompts;

    /// <summary>Obtiene el contenido del prompt cuyo nombre de fichero (sin extensión) coincide con <paramref name="promptName"/>.</summary>
    /// <param name="promptName">Nombre del fichero de prompt sin extensión (p.ej. "SelectSkills").</param>
    /// <returns>Contenido del fichero Markdown, o <see langword="null"/> si no existe ningún prompt con ese nombre.</returns>
    public string? GetPrompt(string promptName)
    {
        _prompts ??= LoadPrompts();
        return _prompts.TryGetValue(promptName, out var content) ? content : null;
    }

    private static Dictionary<string, string> LoadPrompts()
    {
        var directory = Path.Combine(AppContext.BaseDirectory, PromptsDirectory);

        if (!Directory.Exists(directory))
            return [];

        return Directory
            .EnumerateFiles(directory, "*.md")
            .ToDictionary(
                path => Path.GetFileNameWithoutExtension(path),
                File.ReadAllText);
    }
}