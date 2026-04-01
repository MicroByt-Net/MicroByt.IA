namespace MicroByt.IA.Domain.AgentSkills;

/// <summary>Habilidad que puede ejecutar un agente, compuesta por herramientas e instrucciones de comportamiento.</summary>
public class Skill
{
    /// <summary>Identificador único de la skill, usado para referenciarla desde el agente.</summary>
    public string Name { get; init; } = string.Empty;

    /// <summary>Descripción del propósito de la skill, usada por el agente para decidir cuándo aplicarla.</summary>
    public string Description { get; init; } = string.Empty;

    /// <summary>Ruta absoluta del fichero SKILL.md del que se cargó esta skill. <see langword="null"/> si no se cargó desde fichero.</summary>
    public string? FilePath { get; init; }
}