namespace MicroByt.IA.Core.Application.Interfaces;

/// <summary>Servicio que carga y expone el contenido de los prompts almacenados como ficheros Markdown.</summary>
public interface IPromptsCollectionService
{
    /// <summary>Obtiene el contenido del prompt cuyo nombre de fichero (sin extensión) coincide con <paramref name="promptName"/>.</summary>
    /// <param name="promptName">Nombre del fichero de prompt sin extensión (p.ej. "SelectSkills").</param>
    /// <returns>Contenido del fichero Markdown, o <see langword="null"/> si no existe ningún prompt con ese nombre.</returns>
    string? GetPrompt(string promptName);
}