#if SAMPLES
using MicroByt.IA.Application.Interfaces;
using MicroByt.IA.Domain.AI;
using MicroByt.IA.Infrastructure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace MicroByt.IA.API.Samples;

public static class SelectSkillsSample
{
    public static async Task RunTest1()
    {
        var builder = Host.CreateApplicationBuilder();
#if ADDUSERSECRETS
        builder.Configuration.AddUserSecrets(typeof(SelectSkillsSample).Assembly);
#endif
        builder.Services.AddMicrobytIA();

        using var host = builder.Build();

        var ollamaProvider = new Provider
        {
            Name = "Ollama",
            UrlBase = "http://localhost:11434/v1",
            ApiKey = "ollama",
            Local = true,
        };

        var model = new AIModel
        {
            Provider = ollamaProvider,
            // Model = "deepseek-r1:32b",
            // Model = "deepseek-r1:1.5b"
            // Model = "qwen3",
            Model = "nemotron-3-nano:4b",
        };

        string[] tasks =
        [
            "Busca en la web comparativas entre Unity IAP y RevenueCat y mándame un resumen claro con pros y contras.",
        ];

        using var scope = host.Services.CreateScope();

        var skillsService = scope.ServiceProvider.GetRequiredService<ISkillsService>();
        skillsService.LoadSkillsSystem();

        var skillsAgent = scope.ServiceProvider.GetRequiredService<ISkillsAgentService>();

        foreach (var task in tasks)
        {
            try
            {
                Console.WriteLine($"Task: {task}");
                var result = await skillsAgent.RunAgent(model, task);
                Console.WriteLine($"Response: {result}");
                Console.WriteLine();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error seleccionando skills: " + ex.Message);
            }
        }
    }
}
#endif