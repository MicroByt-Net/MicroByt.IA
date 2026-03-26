namespace MicroByt.IA.Core.Entities.Data.AI;

public class AIModel
{
    public Provider Provider { get; init; } = new();
    public string Model { get; init; } = string.Empty;
}
