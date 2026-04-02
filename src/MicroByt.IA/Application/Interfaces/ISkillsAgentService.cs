using MicroByt.IA.Domain.AgentSkills;
using MicroByt.IA.Domain.AI;

namespace MicroByt.IA.Application.Interfaces;

/// <summary>Agente que selecciona las <see cref="Skill"/> necesarias para completar una tarea.</summary>
public interface ISkillsAgentService
{
    /// <summary>Analiza la tarea y devuelve las skills necesarias para completarla.</summary>
    /// <param name="model">Modelo de IA que se usará para la selección.</param>
    /// <param name="task">Descripción de la tarea del usuario.</param>
    /// <returns>Respuesta generada por el agente.</returns>
    Task<string> RunAgent(AIModel model, string task);
}