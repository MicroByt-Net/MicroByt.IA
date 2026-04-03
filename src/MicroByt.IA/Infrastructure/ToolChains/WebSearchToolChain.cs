using System.Text.Json;
using MicroByt.IA.Application.Interfaces;
using MicroByt.IA.Domain.AgentSkills;

namespace MicroByt.IA.Infrastructure.ToolChains;

/// <summary>Implementación del tool <c>web_search</c>. Busca información actual en la web.</summary>
public class WebSearchToolChain : IToolChain
{
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

    public Task<string> ExecuteAsync(JsonDocument arguments, CancellationToken ct = default)
    {
        // TODO: implementar búsqueda real
        throw new NotImplementedException();
    }
}