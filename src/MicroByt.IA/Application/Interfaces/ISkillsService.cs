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

    /// <summary>
    /// Carga una skill desde el bloque YAML front matter de un fichero SKILL.md y la añade a la colección interna.
    /// El formato esperado es un bloque <c>---</c> al inicio del fichero con los campos de la skill en YAML.
    /// </summary>
    /// <param name="filePath">Ruta absoluta al fichero SKILL.md.</param>
    void LoadYamlSkill(string filePath);

    /// <summary>Returns the body content of a SKILL.md file, stripping the YAML front matter block.</summary>
    /// <param name="filePath">Absolute path to the SKILL.md file.</param>
    string? LoadContentSkill(string filePath);

    /// <summary>Clears all loaded skills.</summary>
    void Clean();
}