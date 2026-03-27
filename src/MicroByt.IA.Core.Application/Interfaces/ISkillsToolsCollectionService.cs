using MicroByt.IA.Core.Entities.AgentSkills;

namespace MicroByt.IA.Core.Application.Interfaces;

/// <summary>Servicio que gestiona la colección de <see cref="Skill"/> disponibles para los agentes.</summary>
public interface ISkillsToolsCollectionService
{
    /// <summary>Colección de skills disponibles. <see langword="null"/> si aún no se ha configurado ninguna.</summary>
    Skill[]? Skills { get; }
}