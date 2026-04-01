using MicroByt.IA.Application.Interfaces;
using MicroByt.IA.Domain.AgentSkills;

namespace MicroByt.IA.Application.Services;

/// <summary>Implementación de <see cref="ISkillsToolsCollectionService"/> que gestiona la colección de <see cref="Skill"/> disponibles para los agentes.</summary>
public class SkillsToolsCollectionService : ISkillsToolsCollectionService
{
    private Skill[]? _skills;

    /// <summary>Colección de skills disponibles. <see langword="null"/> si aún no se ha configurado ninguna.</summary>
    public Skill[]? Skills => _skills;

    /// <inheritdoc/>
    public void SetSkills(Skill[] skills) => _skills = skills;
}