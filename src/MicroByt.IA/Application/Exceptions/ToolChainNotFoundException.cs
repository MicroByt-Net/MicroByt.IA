namespace MicroByt.IA.Application.Exceptions;

public class ToolChainNotFoundException(string toolName)
    : Exception($"No hay ningún IToolChain registrado con el nombre '{toolName}'.")
{
    public string ToolName { get; } = toolName;
}