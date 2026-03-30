using MicroByt.IA.Core.Application.Interfaces;
using MicroByt.IA.Core.Entities.AgentSkills;

namespace MicroByt.IA.Core.Application.Services;

/// <summary>Implementation of <see cref="ISkillsService"/> that loads skills from SKILL.md files on disk.</summary>
public class SkillsService : ISkillsService
{
    private const string SkillsDirectory = "Data/Skills";
    private const string SkillFileName = "SKILL.md";
    private const string PurposePrefix = "**Purpose:**";

    private Skill[]? _skills;

    /// <summary>Collection of loaded skills. <see langword="null"/> if <see cref="LoadSkills"/> has not been called yet.</summary>
    public Skill[]? Skills => _skills;

    /// <summary>Loads all skills from SKILL.md files under the Data/Skills directory and stores them internally.</summary>
    public void LoadSkills()
    {
        var baseDir = Path.Combine(AppContext.BaseDirectory, SkillsDirectory);

        if (!Directory.Exists(baseDir))
        {
            _skills = [];
            return;
        }

        _skills = Directory
            .GetDirectories(baseDir)
            .Select(dir =>
            {
                var name = Path.GetFileName(dir);
                var filePath = Path.Combine(dir, SkillFileName);

                if (!File.Exists(filePath))
                    return null;

                var purposeLine = File.ReadLines(filePath)
                    .FirstOrDefault(l => l.StartsWith(PurposePrefix));

                var purpose = purposeLine is not null
                    ? purposeLine[PurposePrefix.Length..].Trim()
                    : string.Empty;

                return new Skill { Name = name, Purpose = purpose };
            })
            .Where(s => s is not null)
            .ToArray()!;
    }
}