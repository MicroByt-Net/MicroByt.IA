using MicroByt.IA.Application.Interfaces;
using MicroByt.IA.Domain.AgentSkills;
using MicroByt.IA.Infrastructure.Exceptions;

namespace MicroByt.IA.Application.Services;

/// <summary>
/// Agrega todos los <see cref="IToolChain"/> registrados en el contenedor de DI
/// y expone el punto de entrada único para consultarlos y ejecutarlos.
/// </summary>
public class ToolChainRegistry : IToolChainRegistry
{
    private readonly IReadOnlyDictionary<string, IToolChain> _chains;

    public ToolChainRegistry(IEnumerable<IToolChain> toolChains)
    {
        _chains = toolChains.ToDictionary(tc => tc.Tool.Name);
    }

    public IReadOnlyList<Tool> GetTools() =>
        _chains.Values.Select(tc => tc.Tool).ToList();

    public Task<string> ExecuteAsync(string toolName, string argumentsJson, CancellationToken ct = default)
    {
        if (!_chains.TryGetValue(toolName, out var chain))
            throw new ToolChainNotFoundException(toolName);

        return chain.ExecuteAsync(argumentsJson, ct);
    }
}