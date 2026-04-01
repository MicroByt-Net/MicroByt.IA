using MicroByt.IA.Core.Application.Interfaces;
using MicroByt.IA.Core.Entities.AgentSkills;

namespace MicroByt.IA.Core.Application.Services;

/// <summary>Implementation of <see cref="ISkillsService"/> that loads skills from SKILL.md files on disk.</summary>
public class SkillsService(IFileSkillCacheService fileCache) : ISkillsService
{
    private const string SkillsDirectory = "Data/Skills";
    private const string SkillFileName = "SKILL.md";
    private const string PurposePrefix = "**Purpose:**";

    private readonly List<Skill> _skills = [];

    /// <summary>Returns a snapshot of the currently loaded skills.</summary>
    public Skill[] GetEligibleSkills() => _skills.ToArray();

    /// <summary>Clears all loaded skills.</summary>
    public void Clean() => _skills.Clear();

    /// <summary>Loads all skills from the default system directory (<c>Data/Skills</c> relative to the application base).</summary>
    public void LoadSkillsSystem() =>
        LoadSkills(Path.Combine(AppContext.BaseDirectory, SkillsDirectory));
    
    // TODO:Hacer un LoadYamlSkill que le paso un fichero y crea el skill desde el contenido --- --- dentro es un contenido yaml
    // y en skill guardar el fichero para poder leer el contenido de este.
    
    // TODO: LoadContentSkill que devuelve el contenido del fichero .md sin los datos del yaml

    /// <summary>Loads all skills from SKILL.md files under the specified base directory and stores them internally.</summary>
    public void LoadSkills(string baseDir)
    {
        if (!Directory.Exists(baseDir))
            return;

        var loaded = Directory
            .GetDirectories(baseDir)
            .Select(dir =>
            {
                var name = Path.GetFileName(dir);
                var filePath = Path.Combine(dir, SkillFileName);

                var content = fileCache.GetContent(filePath);
                if (content is null)
                    return null;

                var purposeLine = content
                    .Split('\n')
                    .FirstOrDefault(l => l.StartsWith(PurposePrefix));

                var purpose = purposeLine is not null
                    ? purposeLine[PurposePrefix.Length..].Trim()
                    : string.Empty;

                return new Skill { Name = name, Purpose = purpose };
            })
            .Where(s => s is not null)
            .ToList();

        _skills.AddRange(loaded!);
    }
}