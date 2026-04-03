using System.Text.Json;
using MicroByt.IA.Domain.AgentSkills;

namespace MicroByt.IA.Application.Interfaces;

/// <summary>Implementación ejecutable de una herramienta que un agente puede invocar.</summary>
public interface IToolChain
{
    /// <summary>Metadatos de la herramienta: nombre, descripción y JSON Schema de parámetros.</summary>
    Tool Tool { get; }

    /// <summary>Ejecuta la herramienta con los argumentos proporcionados por el LLM.</summary>
    /// <param name="arguments">Argumentos parseados según el schema de <see cref="Tool.JsonSchema"/>.</param>
    /// <param name="ct">Token de cancelación.</param>
    /// <returns>Resultado de la ejecución como cadena (normalmente JSON o texto plano).</returns>
    Task<string> ExecuteAsync(JsonDocument arguments, CancellationToken ct = default);
}