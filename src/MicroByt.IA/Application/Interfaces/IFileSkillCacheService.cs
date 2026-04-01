using MicroByt.IA.Application.Models;

namespace MicroByt.IA.Application.Interfaces;

/// <summary>Caché de ficheros SKILL.md basada en metadatos del sistema de ficheros.</summary>
/// <remarks>
/// Antes de devolver el contenido cacheado, comprueba que el tamaño y la fecha de modificación
/// del fichero no hayan cambiado. Si han cambiado, releerá el fichero y actualizará la caché.
/// </remarks>
public interface IFileSkillCacheService
{
    /// <summary>
    /// Devuelve el contenido del fichero indicado desde la caché si sigue vigente,
    /// o lo lee del disco y lo almacena si ha cambiado o no estaba cacheado.
    /// Devuelve <see langword="null"/> si el fichero no existe.
    /// </summary>
    string? GetContent(string filePath);

    /// <summary>Devuelve la entrada de caché del fichero, o <see langword="null"/> si no está cacheado.</summary>
    SkillFileCacheEntry? GetEntry(string filePath);

    /// <summary>Elimina la entrada de caché del fichero indicado, forzando una relectura en el siguiente acceso.</summary>
    void Invalidate(string filePath);

    /// <summary>Elimina todas las entradas de caché.</summary>
    void InvalidateAll();
}