using MicroByt.IA.Domain.AI;

namespace MicroByt.IA.Application.Models;

/// <summary>Datos de entrada para el agente de selección de skills.</summary>
public class SkillsAgentInput
{
    /// <summary>Modelo de IA que se usará para la selección.</summary>
    public required AIModel Model { get; init; }

    /// <summary>Descripción de la tarea del usuario.</summary>
    public required string Task { get; init; }
}