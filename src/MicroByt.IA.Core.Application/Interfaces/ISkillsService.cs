namespace MicroByt.IA.Core.Application.Interfaces;

/// <summary>Service that loads and exposes the skills stored as SKILL.md files on disk.</summary>
public interface ISkillsService
{
    /// <summary>Loads all skills from SKILL.md files under the Data/Skills directory and stores them internally.</summary>
    void LoadSkills();
}