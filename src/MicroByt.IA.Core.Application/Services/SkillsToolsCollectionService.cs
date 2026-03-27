using MicroByt.IA.Core.Application.Interfaces;
using MicroByt.IA.Core.Entities.AgentSkills;

namespace MicroByt.IA.Core.Application.Services;

/// <summary>Implementación de <see cref="ISkillsToolsCollectionService"/> que gestiona la colección de <see cref="Skill"/> disponibles para los agentes.</summary>
public class SkillsToolsCollectionService : ISkillsToolsCollectionService
{
    /// <summary>Colección de skills disponibles. <see langword="null"/> si aún no se ha configurado ninguna.</summary>
    public Skill[]? Skills { get; }
}