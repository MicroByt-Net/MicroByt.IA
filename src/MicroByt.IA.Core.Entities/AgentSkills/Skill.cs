namespace MicroByt.IA.Core.Entities.AgentSkills;

public class Skill
{
    public string Name { get; init; } = string.Empty;
    public string Description { get; init; } = string.Empty;
    public IReadOnlyList<string> RequiredTools { get; init; } = [];
    public IReadOnlyList<string> Instructions { get; init; } = [];

    // TODO: clase temporal
    public static readonly Skill WebResearch = new()
    {
        Name = "web-research",
        Description = "Investiga información en internet y prioriza fuentes útiles.",
        RequiredTools = ["web_search", "web_fetch"],
        Instructions =
        [
            "Empieza con una búsqueda amplia y luego refina.",
            "Prioriza documentación oficial y fuentes fiables.",
            "Extrae solo la información relevante para la tarea.",
            "Evita repetir información redundante.",
        ],
    };
}
