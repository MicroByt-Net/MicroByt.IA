namespace MicroByt.IA.Domain.AI;

/// <summary>Combinación de proveedor y modelo que identifica un LLM concreto a usar.</summary>
public class AIModel
{
    /// <summary>Proveedor que aloja el modelo (contiene URL base y API key).</summary>
    public Provider Provider { get; init; } = new();

    /// <summary>Identificador del modelo dentro del proveedor, por ejemplo "gpt-4o" o "o3-mini".</summary>
    public string Model { get; init; } = string.Empty;
}