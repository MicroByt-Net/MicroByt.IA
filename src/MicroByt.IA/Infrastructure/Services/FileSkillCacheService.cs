using MicroByt.IA.Application.Interfaces;
using MicroByt.IA.Application.Models;
using Microsoft.Extensions.Caching.Memory;

namespace MicroByt.IA.Infrastructure.Services;

/// <summary>
/// Implementación de <see cref="IFileSkillCacheService"/> que usa <see cref="IMemoryCache"/>
/// para cachear el contenido de ficheros SKILL.md.
/// </summary>
/// <remarks>
/// Una entrada cacheada se considera vigente si el tamaño del fichero y su fecha de última
/// modificación (UTC) coinciden con los registrados en el momento de la lectura.
/// Cualquier discrepancia provoca la relectura del fichero y la actualización de la caché.
/// </remarks>
public sealed class FileSkillCacheService(IMemoryCache cache) : IFileSkillCacheService
{
    private readonly IMemoryCache _cache = cache;

    /// <inheritdoc/>
    public string? GetContent(string filePath)
    {
        if (TryGetValidEntry(filePath, out var entry))
            return entry!.Content;

        return ReadAndCache(filePath)?.Content;
    }

    /// <inheritdoc/>
    public SkillFileCacheEntry? GetEntry(string filePath)
    {
        if (TryGetValidEntry(filePath, out var entry))
            return entry;

        return ReadAndCache(filePath);
    }

    /// <inheritdoc/>
    public void Invalidate(string filePath) => _cache.Remove(CacheKey(filePath));

    /// <inheritdoc/>
    public void InvalidateAll()
    {
        if (_cache is MemoryCache mc)
            mc.Clear();
    }

    // -------------------------------------------------------------------------

    private bool TryGetValidEntry(string filePath, out SkillFileCacheEntry? entry)
    {
        if (!_cache.TryGetValue(CacheKey(filePath), out entry) || entry is null)
            return false;

        var info = new FileInfo(filePath);

        if (!info.Exists)
        {
            _cache.Remove(CacheKey(filePath));
            entry = null;
            return false;
        }

        // La entrada es válida sólo si tamaño y fecha de modificación coinciden.
        if (info.Length == entry.FileSize && info.LastWriteTimeUtc == entry.LastModifiedUtc)
            return true;

        // El fichero cambió: invalida la entrada obsoleta.
        _cache.Remove(CacheKey(filePath));
        entry = null;
        return false;
    }

    private SkillFileCacheEntry? ReadAndCache(string filePath)
    {
        var info = new FileInfo(filePath);

        if (!info.Exists)
            return null;

        var content = File.ReadAllText(filePath);

        // Releer FileInfo tras la lectura para capturar el estado real del fichero.
        info.Refresh();

        var newEntry = new SkillFileCacheEntry
        {
            FilePath        = filePath,
            Content         = content,
            FileSize        = info.Length,
            LastModifiedUtc = info.LastWriteTimeUtc,
            CachedAtUtc     = DateTime.UtcNow,
        };

        _cache.Set(CacheKey(filePath), newEntry);
        return newEntry;
    }

    private static string CacheKey(string filePath) => $"skill:{filePath}";
}