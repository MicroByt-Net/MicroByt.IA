using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using MicroByt.IA.Application.Interfaces;
using MicroByt.IA.Domain.AgentSkills;
using Microsoft.Extensions.Configuration;

namespace MicroByt.IA.Infrastructure.ToolChains;

/// <summary>Implementación del tool <c>web_search</c> usando la API de Tavily.</summary>
public class WebSearchToolChain(IConfiguration configuration) : IToolChain
{
    private static readonly HttpClient _http = new();
    private readonly string _apiKey = configuration["ApiKeys:Tavily"] ?? string.Empty;
    private const string TavilyEndpoint = "https://api.tavily.com/search";

    public Tool Tool { get; } = new()
    {
        Name = "web_search",
        Description = "Busca información actual en la web.",
        JsonSchema = """
            {
              "type": "object",
              "properties": {
                "query": { "type": "string", "description": "Consulta de búsqueda" },
                "top_k": { "type": "integer", "minimum": 1, "maximum": 20 }
              },
              "required": ["query"]
            }
            """,
    };

    public async Task<string> ExecuteAsync(JsonDocument arguments, CancellationToken ct = default)
    {
        var query = arguments.RootElement.GetProperty("query").GetString()!;
        var topK = arguments.RootElement.TryGetProperty("top_k", out var tk) ? tk.GetInt32() : 5;

        var payload = new
        {
            api_key = _apiKey,
            query,
            max_results = topK,
        };

        using var response = await _http.PostAsJsonAsync(TavilyEndpoint, payload, ct);
        response.EnsureSuccessStatusCode();

        using var doc = await response.Content.ReadFromJsonAsync<JsonDocument>(cancellationToken: ct);
        return FormatResponse(doc!);
    }

    private static string FormatResponse(JsonDocument doc)
    {
        var sb = new StringBuilder();
        var root = doc.RootElement;

        if (root.TryGetProperty("answer", out var answer) && answer.ValueKind == JsonValueKind.String)
        {
            sb.AppendLine($"Answer: {answer.GetString()}");
            sb.AppendLine();
        }

        if (!root.TryGetProperty("results", out var results))
            return sb.ToString().TrimEnd();

        foreach (var result in results.EnumerateArray())
        {
            var title   = result.TryGetProperty("title",   out var t) ? t.GetString() : string.Empty;
            var url     = result.TryGetProperty("url",     out var u) ? u.GetString() : string.Empty;
            var content = result.TryGetProperty("content", out var c) ? c.GetString() : string.Empty;

            sb.AppendLine($"## {title}");
            sb.AppendLine($"URL: {url}");
            sb.AppendLine(content);
            sb.AppendLine();
        }

        return sb.ToString().TrimEnd();
    }
}
