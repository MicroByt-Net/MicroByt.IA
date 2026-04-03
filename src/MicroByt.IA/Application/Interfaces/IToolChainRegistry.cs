using MicroByt.IA.Domain.AgentSkills;

namespace MicroByt.IA.Application.Interfaces;

/// <summary>Agrega todos los <see cref="IToolChain"/> registrados y expone el punto de entrada único para ejecutarlos.</summary>
public interface IToolChainRegistry
{
    /// <summary>Devuelve los metadatos de todas las herramientas disponibles para enviar al LLM.</summary>
    IReadOnlyList<Tool> GetTools();

    /// <summary>Ejecuta la herramienta identificada por <paramref name="toolName"/>.</summary>
    /// <param name="toolName">Nombre del tool tal como lo devuelve el LLM en su tool call.</param>
    /// <param name="argumentsJson">JSON con los argumentos proporcionados por el LLM.</param>
    /// <param name="ct">Token de cancelación.</param>
    /// <returns>Resultado de la ejecución como cadena.</returns>
    Task<string> ExecuteAsync(string toolName, string argumentsJson, CancellationToken ct = default);
}