#if SAMPLES
using MicroByt.IA.Application;
using MicroByt.IA.Application.Interfaces;
using MicroByt.IA.Domain.AgentSkills;
using MicroByt.IA.Domain.AI;
using Microsoft.Extensions.DependencyInjection;

namespace MicroByt.IA.API.Samples;

public static class SelectSkillsSample
{
    public static async Task RunTest1()
    {
        var services = new ServiceCollection();
        services.AddMicrobytIA();

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
            // Model = "deepseek-r1:32b",
            // Model = "deepseek-r1:1.5b"
            // Model = "qwen3",
            Model = "nemotron-3-nano:4b",
        };

        /*
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
        */

        string[] tasks =
        [
            "Busca en la web comparativas entre Unity IAP y RevenueCat y mándame un resumen claro con pros y contras.",
            "A partir de ahora recuerda que prefiero respuestas técnicas, breves y en español.",
            "Guarda que trabajo sobre todo con C#, Unity y ASP.NET Core.",
            "Recuerda que estoy preparando mi proyecto final del máster sobre agentes con IA en .NET.",
            "¿Qué recuerdas de mis preferencias de trabajo?",
            "Adapta tus respuestas a mi perfil de desarrollador backend y de videojuegos.",
            "Guarda una nota: idea de proyecto -> MiniClaw.NET con memoria, tools y skills.",
            "Añade una nota con los módulos: gateway, orchestrator, memory, tools, skills y audit.",
            "Muéstrame mis notas sobre el proyecto final.",
            "Busca en mis notas cualquier referencia a Telegram o SignalR.",
            "Resume mis notas del proyecto en 5 puntos.",
            "Convierte mis notas en una lista de tareas inicial.",
            "Crea una tarea: diseñar el modelo de datos de MiniClaw.NET para mañana a las 18:00.",
            "Apunta una tarea para este fin de semana: implementar memoria persistente con EF Core.",
            "¿Qué tareas tengo pendientes del proyecto?",
            "Ordéname las tareas por prioridad técnica.",
            "Divide el desarrollo del MVP en tareas de 2 horas.",
            "Crea un plan de 3 semanas para terminar el proyecto final.",
            "Busca arquitecturas modernas de agentes con tool calling en .NET y dame un resumen.",
            "Compara Semantic Kernel y un orquestador propio para MiniClaw.NET.",
            "Busca ejemplos de sistemas con memoria persistente para asistentes IA.",
            "Encuentra documentación oficial sobre function calling o tool calling.",
            "Busca proyectos open source similares a OpenClaw pero centrados en .NET.",
            "Resume las mejores prácticas de seguridad para asistentes con ejecución de tools.",
            "Busca en la web ejemplos de arquitectura para agentes en .NET, guarda un resumen en mis notas y crea una tarea con los puntos que deba investigar.",
            "Revisa mis notas del proyecto, detecta huecos importantes y conviértelos en tareas.",
            "Busca información reciente sobre tool calling, compárala con mis notas y dime qué me falta.",
            "Resume todo lo que hemos hablado del proyecto y guárdalo como nota \"estado actual\".",
            "Localiza en mis notas los módulos pendientes y genera una propuesta de roadmap.",
            "Actúa como arquitecto .NET y propón la estructura por capas para MiniClaw.NET.",
            "Diseña las interfaces públicas del sistema sin abusar de abstracciones innecesarias.",
            "Propón un modelo de dominio para conversaciones, tools, skills y memoria.",
            "Actúa como project manager y convierte este proyecto en hitos, entregables y riesgos.",
            "Haz un backlog priorizado del MVP.",
            "Identifica qué partes son imprescindibles para llegar a una demo funcional.",
            "Redacta la justificación del proyecto para la memoria del máster.",
            "Escribe los objetivos generales y específicos del proyecto.",
            "Convierte esta arquitectura en una explicación académica formal.",
            "Genera el esqueleto de un servicio MemoryService en C# con EF Core.",
            "Proponme una API REST mínima para conversaciones, notas y tareas.",
            "Dame pruebas unitarias para el ToolRegistry.",
            "Recuerda que el proyecto se llama MiniClaw.NET.",
            "Guarda esta idea: el sistema debe pedir confirmación antes de ejecutar acciones sensibles.",
            "Busca mejores prácticas de seguridad para agentes con tools.",
            "Resume lo encontrado en 4 puntos.",
            "Guárdalo en notas bajo \"seguridad\".",
            "Créame una tarea para implementar confirmación humana en acciones críticas.",
            ";Muy buenos si quieres que el proyecto parezca más serio.",
            "Antes de crear tareas o sobrescribir notas, pide confirmación.",
            "Quiero que las búsquedas web sean automáticas pero que borrar notas requiera aprobación.",
            "Muéstrame qué acciones consideras sensibles.",
            "Enséñame el historial de tools ejecutadas en esta conversación.",
            "Explica por qué has decidido usar esta tool y no otra.",
            "Guarda que mi proyecto final debe combinar IA y.NET de forma realista.",
            "Busca ideas para integrar MiniClaw.NET con Unity como asistente externo.",
            "Dame una arquitectura para usar MiniClaw.NET como backend de un NPC inteligente en Unity.",
            "Convierte estas ideas en un backlog técnico para un desarrollador C#.",
            "Proponme endpoints ASP.NET Core para conectar Unity con el asistente.",
            "Analiza esta petición y decide qué tools necesitas usar antes de responder.",
            "Si te falta contexto, primero busca en mis notas y luego en la web.",
            "Cuando termines, guarda automáticamente un resumen útil de la conversación.",
            "Si detectas una preferencia estable, proponme recordarla en memoria.",
            "Si una tarea requiere varios pasos, ejecútalos en orden y explícame el resultado.",
            "Recuerda que estoy desarrollando MiniClaw.NET como proyecto final del máster.",
            "Guarda una nota con los módulos principales del sistema.",
            "Busca arquitecturas de agentes en.NET y resume lo esencial.",
            "Añade ese resumen a mis notas.",
            "Genera un backlog inicial en 8 tareas.",
            "Crea una tarea para implementar la memoria persistente mañana a las 18:00.",
            "¿Qué sabes ya sobre mi proyecto y qué me recomiendas hacer después?",
            "Eso queda muy bien porque enseña continuidad, herramientas y memoria real.",
            "Conviene que el sistema no parezca rígido.Algunos prompts podrían ser así:",
            "Apunta esto para luego: quiero que el sistema tenga skills en Markdown.",
            "Recuérdame mañana seguir con el ToolRegistry.",
            "Busca si hay buenas prácticas para auditoría de tools en asistentes IA.",
            "Resume lo importante y guárdalo.",
            "Dime qué parte del proyecto tiene más riesgo técnico."
        ];

        using var scope = provider.CreateScope();

        var skillsService = scope.ServiceProvider.GetRequiredService<ISkillsToolsCollectionService>();
        //skillsService.SetSkills(skills);

        var skillsAgent = scope.ServiceProvider.GetRequiredService<ISkillsAgentService>();

        foreach (var task in tasks)
        {
            try
            {
                await skillsAgent.Select(model, task);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error seleccionando skills: " + ex.Message);
            }
        }
    }
}
#endif