using MicroByt.IA.Application.Interfaces;
using MicroByt.IA.Domain.AgentSkills;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace MicroByt.IA.Application.Services;

/// <summary>Implementation of <see cref="ISkillsService"/> that loads skills from SKILL.md files on disk.</summary>
public class SkillsService(IFileSkillCacheService fileCache) : ISkillsService
{
    private const string SkillsDirectory = "Data/Skills";
    private const string SkillFileName = "SKILL.md";

    private static readonly IDeserializer Deserializer = new DeserializerBuilder()
        .WithNamingConvention(UnderscoredNamingConvention.Instance)
        .IgnoreUnmatchedProperties()
        .Build();

    private readonly List<Skill> _skills = [];

    /// <summary>Returns a snapshot of the currently loaded skills.</summary>
    public Skill[] GetEligibleSkills() => _skills.ToArray();

    /// <summary>Clears all loaded skills.</summary>
    public void Clean() => _skills.Clear();

    /// <summary>Loads all skills from the default system directory (<c>Data/Skills</c> relative to the application base).</summary>
    public void LoadSkillsSystem() =>
        LoadSkills(Path.Combine(AppContext.BaseDirectory, SkillsDirectory));

    /// <inheritdoc/>
    public void LoadYamlSkill(string filePath)
    {
        var content = fileCache.GetContent(filePath);
        if (content is null)
            return;

        var yaml = ExtractFrontMatter(content);
        if (yaml is null)
            return;

        var dto = Deserializer.Deserialize<SkillYaml>(yaml);

        _skills.Add(new Skill
        {
            Name          = dto.Name,
            Description   = dto.Description,
            FilePath      = filePath,
        });
    }

    /// <inheritdoc/>
    public string? LoadContentSkill(string filePath)
    {
        var content = fileCache.GetContent(filePath);
        if (content is null)
            return null;

        var lines = content.Split('\n');
        if (lines.Length < 2 || lines[0].Trim() != "---")
            return content;

        for (var i = 1; i < lines.Length; i++)
        {
            if (lines[i].Trim() == "---")
                return string.Join("\n", lines[(i + 1)..]).TrimStart();
        }

        return content;
    }

    /// <summary>Loads all skills from SKILL.md files under the specified base directory and stores them internally.</summary>
    public void LoadSkills(string baseDir)
    {
        if (!Directory.Exists(baseDir))
            return;

        foreach (var dir in Directory.GetDirectories(baseDir))
            LoadYamlSkill(Path.Combine(dir, SkillFileName));
    }

    // -------------------------------------------------------------------------

    private static string? ExtractFrontMatter(string content)
    {
        var lines = content.Split('\n');
        if (lines.Length < 2 || lines[0].Trim() != "---")
            return null;

        var end = -1;
        for (var i = 1; i < lines.Length; i++)
        {
            if (lines[i].Trim() == "---")
            {
                end = i;
                break;
            }
        }

        return end < 0 ? null : string.Join("\n", lines[1..end]);
    }

    private sealed class SkillYaml
    {
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Purpose { get; set; } = string.Empty;
        public List<string> RequiredTools { get; set; } = [];
        public List<string> Instructions { get; set; } = [];
    }
}