using MicroByt.IA.Domain.AgentSkills;

namespace MicroByt.IA.Application.Interfaces;

/// <summary>Servicio que gestiona la colección de <see cref="Skill"/> disponibles para los agentes.</summary>
public interface ISkillsToolsCollectionService
{
    /// <summary>Colección de skills disponibles. <see langword="null"/> si aún no se ha configurado ninguna.</summary>
    Skill[]? Skills { get; }

    /// <summary>Establece la colección de skills disponibles.</summary>
    void SetSkills(Skill[] skills);
}