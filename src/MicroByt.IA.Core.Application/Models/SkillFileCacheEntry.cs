namespace MicroByt.IA.Core.Application.Models;

/// <summary>Metadatos de un fichero SKILL.md almacenado en caché.</summary>
public sealed class SkillFileCacheEntry
{
    /// <summary>Ruta absoluta del fichero.</summary>
    public required string FilePath { get; init; }

    /// <summary>Contenido completo del fichero en el momento de la lectura.</summary>
    public required string Content { get; init; }

    /// <summary>Tamaño del fichero en bytes en el momento de la lectura.</summary>
    public required long FileSize { get; init; }

    /// <summary>Fecha y hora de la última modificación del fichero (UTC) en el momento de la lectura.</summary>
    public required DateTime LastModifiedUtc { get; init; }

    /// <summary>Fecha y hora en que la entrada fue almacenada en caché (UTC).</summary>
    public required DateTime CachedAtUtc { get; init; }
}