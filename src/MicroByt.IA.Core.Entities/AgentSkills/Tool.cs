namespace MicroByt.IA.Core.Entities.AgentSkills;

public class Tool
{
    public string Name { get; init; } = string.Empty;
    public string Description { get; init; } = string.Empty;
    public string JsonSchema { get; init; } = string.Empty;

    // TODO: clase temporal
    public static readonly Tool WebSearch = new()
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
}
