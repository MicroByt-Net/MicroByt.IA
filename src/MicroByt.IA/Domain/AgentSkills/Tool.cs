namespace MicroByt.IA.Domain.AgentSkills;

/// <summary>Herramienta que un agente puede invocar para realizar una acción concreta.</summary>
public class Tool
{
    /// <summary>Identificador único de la herramienta. Debe coincidir con el nombre esperado por el agente.</summary>
    public string Name { get; init; } = string.Empty;

    /// <summary>Descripción de qué hace la herramienta, usada por el agente para decidir cuándo invocarla.</summary>
    public string Description { get; init; } = string.Empty;

    /// <summary>JSON Schema que describe los parámetros de entrada que acepta la herramienta.</summary>
    public string JsonSchema { get; init; } = string.Empty;

}