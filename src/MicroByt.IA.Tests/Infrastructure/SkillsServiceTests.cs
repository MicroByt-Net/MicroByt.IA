using MicroByt.IA.Application.Interfaces;
using MicroByt.IA.Application.Models;
using MicroByt.IA.Infrastructure.Services;

namespace MicroByt.IA.Tests.Infrastructure;

public class SkillsServiceTests
{
    // -------------------------------------------------------------------------
    // Stub
    // -------------------------------------------------------------------------

    private sealed class FakeFileCache : IFileSkillCacheService
    {
        private readonly Dictionary<string, string?> _files = new();
        private readonly bool _fallbackToDisk;

        public FakeFileCache(bool fallbackToDisk = false) => _fallbackToDisk = fallbackToDisk;

        public void Setup(string filePath, string? content) => _files[filePath] = content;

        public string? GetContent(string filePath)
        {
            if (_files.TryGetValue(filePath, out var content))
                return content;

            if (_fallbackToDisk && File.Exists(filePath))
                return File.ReadAllText(filePath);

            return null;
        }

        public SkillFileCacheEntry? GetEntry(string filePath) => null;
        public void Invalidate(string filePath) { }
        public void InvalidateAll() { }
    }

    private const string ValidYaml =
        """
        ---
        name: test-skill
        description: A test skill
        ---
        Some markdown content here.
        """;

    // -------------------------------------------------------------------------
    // GetEligibleSkills
    // -------------------------------------------------------------------------

    [Fact]
    public void GetEligibleSkills_WhenNoSkillsLoaded_ReturnsEmptyArray()
    {
        var sut = new SkillsService(new FakeFileCache());

        var result = sut.GetEligibleSkills();

        Assert.Empty(result);
    }

    [Fact]
    public void GetEligibleSkills_ReturnsSnapshot_NotLiveReference()
    {
        var cache = new FakeFileCache();
        cache.Setup("skill.md", ValidYaml);
        var sut = new SkillsService(cache);
        sut.LoadYamlSkill("skill.md");

        var first = sut.GetEligibleSkills();
        sut.Clean();
        var second = sut.GetEligibleSkills();

        Assert.Single(first);
        Assert.Empty(second);
    }

    // -------------------------------------------------------------------------
    // Clean
    // -------------------------------------------------------------------------

    [Fact]
    public void Clean_AfterLoadingSkills_RemovesAllSkills()
    {
        var cache = new FakeFileCache();
        cache.Setup("skill.md", ValidYaml);
        var sut = new SkillsService(cache);
        sut.LoadYamlSkill("skill.md");

        sut.Clean();

        Assert.Empty(sut.GetEligibleSkills());
    }

    [Fact]
    public void Clean_WhenEmpty_DoesNotThrow()
    {
        var sut = new SkillsService(new FakeFileCache());
        var ex = Record.Exception(() => sut.Clean());
        Assert.Null(ex);
    }

    // -------------------------------------------------------------------------
    // LoadYamlSkill
    // -------------------------------------------------------------------------

    [Fact]
    public void LoadYamlSkill_WhenFileNotFound_DoesNotAddSkill()
    {
        var sut = new SkillsService(new FakeFileCache()); // cache vacía → null

        sut.LoadYamlSkill("missing.md");

        Assert.Empty(sut.GetEligibleSkills());
    }

    [Fact]
    public void LoadYamlSkill_WhenFileHasNoFrontMatter_DoesNotAddSkill()
    {
        const string content = "Just markdown without front matter.";
        var cache = new FakeFileCache();
        cache.Setup("skill.md", content);
        var sut = new SkillsService(cache);

        sut.LoadYamlSkill("skill.md");

        Assert.Empty(sut.GetEligibleSkills());
    }

    [Fact]
    public void LoadYamlSkill_WhenFrontMatterIsUnclosed_DoesNotAddSkill()
    {
        const string content = "---\nname: no-close\n";
        var cache = new FakeFileCache();
        cache.Setup("skill.md", content);
        var sut = new SkillsService(cache);

        sut.LoadYamlSkill("skill.md");

        Assert.Empty(sut.GetEligibleSkills());
    }

    [Fact]
    public void LoadYamlSkill_WithValidFrontMatter_AddsSkillWithCorrectName()
    {
        var cache = new FakeFileCache();
        cache.Setup("skill.md", ValidYaml);
        var sut = new SkillsService(cache);

        sut.LoadYamlSkill("skill.md");

        var skills = sut.GetEligibleSkills();
        Assert.Single(skills);
        Assert.Equal("test-skill", skills[0].Name);
    }

    [Fact]
    public void LoadYamlSkill_WithValidFrontMatter_AddsSkillWithCorrectDescription()
    {
        var cache = new FakeFileCache();
        cache.Setup("skill.md", ValidYaml);
        var sut = new SkillsService(cache);

        sut.LoadYamlSkill("skill.md");

        Assert.Equal("A test skill", sut.GetEligibleSkills()[0].Description);
    }

    [Fact]
    public void LoadYamlSkill_WithValidFrontMatter_StoresFilePath()
    {
        const string path = "/some/path/SKILL.md";
        var cache = new FakeFileCache();
        cache.Setup(path, ValidYaml);
        var sut = new SkillsService(cache);

        sut.LoadYamlSkill(path);

        Assert.Equal(path, sut.GetEligibleSkills()[0].FilePath);
    }

    [Fact]
    public void LoadYamlSkill_CalledMultipleTimes_AccumulatesSkills()
    {
        const string yaml2 =
            """
            ---
            name: second-skill
            description: Another skill
            ---
            """;

        var cache = new FakeFileCache();
        cache.Setup("skill1.md", ValidYaml);
        cache.Setup("skill2.md", yaml2);
        var sut = new SkillsService(cache);

        sut.LoadYamlSkill("skill1.md");
        sut.LoadYamlSkill("skill2.md");

        Assert.Equal(2, sut.GetEligibleSkills().Length);
    }

    // -------------------------------------------------------------------------
    // LoadContentSkill
    // -------------------------------------------------------------------------

    [Fact]
    public void LoadContentSkill_WhenFileNotFound_ReturnsNull()
    {
        var sut = new SkillsService(new FakeFileCache());

        var result = sut.LoadContentSkill("missing.md");

        Assert.Null(result);
    }

    [Fact]
    public void LoadContentSkill_WhenFileHasNoFrontMatter_ReturnsFullContent()
    {
        const string content = "Just markdown without front matter.";
        var cache = new FakeFileCache();
        cache.Setup("skill.md", content);
        var sut = new SkillsService(cache);

        var result = sut.LoadContentSkill("skill.md");

        Assert.Equal(content, result);
    }

    [Fact]
    public void LoadContentSkill_WhenFrontMatterIsUnclosed_ReturnsFullContent()
    {
        const string content = "---\nname: no-close\n";
        var cache = new FakeFileCache();
        cache.Setup("skill.md", content);
        var sut = new SkillsService(cache);

        var result = sut.LoadContentSkill("skill.md");

        Assert.Equal(content, result);
    }

    [Fact]
    public void LoadContentSkill_WithValidFrontMatter_ReturnsBodyWithoutYamlBlock()
    {
        var cache = new FakeFileCache();
        cache.Setup("skill.md", ValidYaml);
        var sut = new SkillsService(cache);

        var result = sut.LoadContentSkill("skill.md");

        Assert.Equal("Some markdown content here.", result?.Trim());
    }

    [Fact]
    public void LoadContentSkill_WithOnlyFrontMatter_ReturnsEmptyBody()
    {
        const string content = "---\nname: empty-body\ndescription: No body\n---\n";
        var cache = new FakeFileCache();
        cache.Setup("skill.md", content);
        var sut = new SkillsService(cache);

        var result = sut.LoadContentSkill("skill.md");

        Assert.Equal(string.Empty, result?.Trim());
    }

    // -------------------------------------------------------------------------
    // LoadSkills
    // -------------------------------------------------------------------------

    [Fact]
    public void LoadSkills_WhenDirectoryDoesNotExist_DoesNotAddSkills()
    {
        var sut = new SkillsService(new FakeFileCache());

        sut.LoadSkills("/nonexistent/directory");

        Assert.Empty(sut.GetEligibleSkills());
    }

    [Fact]
    public void LoadSkills_WithSubdirectoriesContainingSkillFiles_LoadsAllSkills()
    {
        var baseDir = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());
        var skill1Dir = Path.Combine(baseDir, "skill-one");
        var skill2Dir = Path.Combine(baseDir, "skill-two");

        Directory.CreateDirectory(skill1Dir);
        Directory.CreateDirectory(skill2Dir);

        const string yaml1 = "---\nname: skill-one\ndescription: First\n---\n";
        const string yaml2 = "---\nname: skill-two\ndescription: Second\n---\n";

        File.WriteAllText(Path.Combine(skill1Dir, "SKILL.md"), yaml1);
        File.WriteAllText(Path.Combine(skill2Dir, "SKILL.md"), yaml2);

        try
        {
            var sut = new SkillsService(new FakeFileCache(fallbackToDisk: true));

            sut.LoadSkills(baseDir);

            Assert.Equal(2, sut.GetEligibleSkills().Length);
        }
        finally
        {
            Directory.Delete(baseDir, recursive: true);
        }
    }

    [Fact]
    public void LoadSkills_WhenSubdirHasNoSkillMdFile_DoesNotAddSkill()
    {
        var baseDir = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());
        var emptySubdir = Path.Combine(baseDir, "empty-skill");
        Directory.CreateDirectory(emptySubdir);

        try
        {
            var sut = new SkillsService(new FakeFileCache());

            sut.LoadSkills(baseDir);

            Assert.Empty(sut.GetEligibleSkills());
        }
        finally
        {
            Directory.Delete(baseDir, recursive: true);
        }
    }
}
