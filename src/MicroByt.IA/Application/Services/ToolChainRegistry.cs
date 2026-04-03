using System.Text.Json;
using MicroByt.IA.Application.Interfaces;
using MicroByt.IA.Domain.AgentSkills;
using MicroByt.IA.Application.Exceptions;

namespace MicroByt.IA.Application.Services;

/// <summary>
/// Agrega todos los <see cref="IToolChain"/> registrados en el contenedor de DI
/// y expone el punto de entrada único para consultarlos y ejecutarlos.
/// </summary>
public class ToolChainRegistry(IEnumerable<IToolChain> toolChains) : IToolChainRegistry
{
    private readonly IReadOnlyDictionary<string, IToolChain> _chains
        = toolChains.ToDictionary(tc => tc.Tool.Name);

    public IReadOnlyList<Tool> GetTools() =>
        _chains.Values.Select(tc => tc.Tool).ToList();

    public Task<string> ExecuteAsync(string toolName, JsonDocument arguments, CancellationToken ct = default)
    {
        if (!_chains.TryGetValue(toolName, out var chain))
            throw new ToolChainNotFoundException(toolName);

        return chain.ExecuteAsync(arguments, ct);
    }
}