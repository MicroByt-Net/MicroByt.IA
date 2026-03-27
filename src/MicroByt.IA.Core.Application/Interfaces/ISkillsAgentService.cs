using MicroByt.IA.Core.Entities.AgentSkills;
using MicroByt.IA.Core.Entities.Data.AI;

namespace MicroByt.IA.Core.Application.Interfaces;

/// <summary>Agente que selecciona las <see cref="Skill"/> necesarias para completar una tarea.</summary>
public interface ISkillsAgentService
{
    /// <summary>Analiza la tarea y devuelve las skills necesarias para completarla.</summary>
    /// <param name="model">Modelo de IA que se usará para la selección.</param>
    /// <param name="task">Descripción de la tarea del usuario.</param>
    /// <returns>Array de skills seleccionadas, o <see langword="null"/> si no se pudo determinar.</returns>
    Task<Skill[]?> Select(AIModel model, string task);
}