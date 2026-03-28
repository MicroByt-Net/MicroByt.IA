using System.Text.Json;
using MicroByt.IA.Core.Application.Extensions.DependencyInjection;
using MicroByt.IA.Core.Application.Interfaces;
using MicroByt.IA.Core.Entities.AgentSkills;
using MicroByt.IA.Core.Entities.Data.AI;
using Microsoft.Extensions.DependencyInjection;

namespace MicroByt.IA.Core.Samples.Samples;

public static class SelectSkillsSample
{
    public static async Task RunTest1()
    {
        var services = new ServiceCollection();
        services.AddMicrobytIAApplication();

        var provider = services.BuildServiceProvider();

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
            //Model = "deepseek-r1:32b",
            Model = "deepseek-r1:1.5b"
        };

        Skill[] skills =
        [
            new Skill
            {
                Name = "web-research",
                Description = "Investiga información en internet y prioriza fuentes útiles.",
                Instructions =
                [
                    "Empieza con una búsqueda amplia y luego refina.",
                    "Prioriza documentación oficial y fuentes fiables.",
                    "Extrae solo la información relevante para la tarea.",
                    "Evita repetir información redundante.",
                ],
            },
            new Skill
            {
                Name = "delivery-summary",
                Description = "Empaqueta el resultado en un mensaje breve, claro y accionable.",
                Instructions =
                [
                    "Redacta en tono directo.",
                    "Pon primero la conclusión principal.",
                    "Después detalla puntos clave.",
                    "Evita párrafos excesivamente largos.",
                ],
            },
            new Skill
            {
                Name = "comparison-summary",
                Description = "Compara varias opciones y produce una síntesis con pros y contras.",
                Instructions =
                [
                    "Identifica criterios de comparación claros.",
                    "Resume similitudes y diferencias clave.",
                    "Entrega pros y contras en lenguaje claro.",
                    "Concluye con una recomendación si hay suficiente evidencia.",
                ],
            },
            new Skill
            {
                Name = "local-file-work",
                Description = "Trabaja con archivos locales para leer o generar contenido.",
                Instructions =
                [
                    "Valida la ruta y el tipo de archivo.",
                    "Lee antes de sobrescribir si procede.",
                    "Minimiza cambios innecesarios.",
                ],
            },
        ];

        using var scope = provider.CreateScope();

        var skillsService = scope.ServiceProvider.GetRequiredService<ISkillsToolsCollectionService>();
        skillsService.SetSkills(skills);

        var skillsAgent = scope.ServiceProvider.GetRequiredService<ISkillsAgentService>();

        var selectedSkills = await skillsAgent.Select(model,
            "Busca en la web comparativas entre Unity IAP y RevenueCat y mándame un resumen claro con pros y contras.");

        var json = JsonSerializer.Serialize(selectedSkills, new JsonSerializerOptions { WriteIndented = true });
        Console.WriteLine(json);
    }
}