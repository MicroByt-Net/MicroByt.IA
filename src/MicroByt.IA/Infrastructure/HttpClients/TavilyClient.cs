using System.Net.Http.Json;
using System.Text.Json;
using MicroByt.IA.Infrastructure.Interfaces;
using MicroByt.IA.Infrastructure.Options;
using Microsoft.Extensions.Options;

namespace MicroByt.IA.Infrastructure.HttpClients;

/// <summary>Cliente HTTP para la API de búsqueda web de Tavily.</summary>
public class TavilyClient(HttpClient http, IOptions<ApiKeysOptions> apiKeysOptions) : ITavilyClient
{
    private readonly HttpClient _http = http;
    private readonly string? _apiKey = apiKeysOptions.Value.Tavily;

    public async Task<JsonDocument> SearchAsync(string query, int maxResults = 5, CancellationToken ct = default)
    {
        var payload = new { api_key = _apiKey, query, max_results = maxResults };

        using var response = await _http.PostAsJsonAsync("/search", payload, ct);
        response.EnsureSuccessStatusCode();

        return await response.Content.ReadFromJsonAsync<JsonDocument>(cancellationToken: ct)
            ?? JsonDocument.Parse("{}");
    }
}
