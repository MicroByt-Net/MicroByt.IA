using MicroByt.IA.Core.Application.Models;
using MicroByt.IA.Core.Entities.AgentSkills;

namespace MicroByt.IA.Core.Application.Interfaces;

/// <summary>Agente que selecciona las <see cref="Skill"/> necesarias para completar una tarea.</summary>
public interface ISkillsAgentService
{
    /// <summary>Analiza la tarea y devuelve las skills necesarias para completarla.</summary>
    /// <param name="input">Datos de entrada con el modelo y la descripción de la tarea.</param>
    Task Select(SkillsAgentInput input);
}