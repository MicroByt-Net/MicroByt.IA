using MicroByt.IA.Domain.AgentSkills;

namespace MicroByt.IA.Application.Interfaces;

/// <summary>Service that loads and exposes the skills stored as SKILL.md files on disk.</summary>
public interface ISkillsService
{
    /// <summary>Loads all skills from SKILL.md files under the specified base directory and stores them internally.</summary>
    void LoadSkills(string baseDir);

    /// <summary>Loads all skills from the default system directory (<c>Data/Skills</c> relative to the application base).</summary>
    void LoadSkillsSystem();

    /// <summary>Returns a snapshot of the currently loaded skills.</summary>
    Skill[] GetEligibleSkills();

    /// <summary>Clears all loaded skills.</summary>
    void Clean();
}