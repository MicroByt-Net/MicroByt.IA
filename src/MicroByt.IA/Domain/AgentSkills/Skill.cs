namespace MicroByt.IA.Domain.AgentSkills;

/// <summary>Habilidad que puede ejecutar un agente, compuesta por herramientas e instrucciones de comportamiento.</summary>
public class Skill
{
    /// <summary>Identificador único de la skill, usado para referenciarla desde el agente.</summary>
    public string Name { get; init; } = string.Empty;

    /// <summary>Descripción del propósito de la skill, usada por el agente para decidir cuándo aplicarla.</summary>
    public string Description { get; init; } = string.Empty;

    /// <summary>Nombres de las herramientas que esta skill necesita para operar.</summary>
    public IReadOnlyList<string> RequiredTools { get; init; } = [];

    /// <summary>Propósito de la skill, usado para comunicar al agente el objetivo que debe cumplir al ejecutarla.</summary>
    public string Purpose { get; init; } = string.Empty;

    /// <summary>Instrucciones de comportamiento que guían al agente durante la ejecución de esta skill.</summary>
    public IReadOnlyList<string> Instructions { get; init; } = [];
}