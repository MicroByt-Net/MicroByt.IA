using System.Text.Json;

namespace MicroByt.IA.Infrastructure.Interfaces;

/// <summary>Cliente HTTP para la API de búsqueda web de Tavily.</summary>
public interface ITavilyClient
{
    /// <summary>Ejecuta una búsqueda web y devuelve la respuesta cruda de Tavily.</summary>
    Task<JsonDocument> SearchAsync(string query, int maxResults = 5, CancellationToken ct = default);
}