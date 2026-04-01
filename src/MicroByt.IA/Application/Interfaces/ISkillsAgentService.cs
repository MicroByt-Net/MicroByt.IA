using MicroByt.IA.Application.Models;
using MicroByt.IA.Domain.AgentSkills;

namespace MicroByt.IA.Application.Interfaces;

/// <summary>Agente que selecciona las <see cref="Skill"/> necesarias para completar una tarea.</summary>
public interface ISkillsAgentService
{
    /// <summary>Analiza la tarea y devuelve las skills necesarias para completarla.</summary>
    /// <param name="input">Datos de entrada con el modelo y la descripción de la tarea.</param>
    Task Select(SkillsAgentInput input);
}