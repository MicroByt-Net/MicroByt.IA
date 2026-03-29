namespace MicroByt.IA.Core.Entities.AgentSkills;

/// <summary>Habilidad que puede ejecutar un agente, compuesta por herramientas e instrucciones de comportamiento.</summary>
public class Skill
{
    /// <summary>Identificador único de la skill, usado para referenciarla desde el agente.</summary>
    public string Name { get; init; } = string.Empty;

    /// <summary>Descripción del propósito de la skill, usada por el agente para decidir cuándo aplicarla.</summary>
    public string Description { get; init; } = string.Empty;

    /// <summary>Nombres de las herramientas que esta skill necesita para operar.</summary>
    public IReadOnlyList<string> RequiredTools { get; init; } = [];

    /// <summary>Instrucciones de comportamiento que guían al agente durante la ejecución de esta skill.</summary>
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
