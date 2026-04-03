using MicroByt.IA.Application.Interfaces;
using MicroByt.IA.Application.Services;
using MicroByt.IA.Infrastructure.Interfaces;
using MicroByt.IA.Infrastructure.Services;
using MicroByt.IA.Infrastructure.ToolChains;
using Microsoft.Extensions.DependencyInjection;

namespace MicroByt.IA.Infrastructure;

/// <summary>Métodos de extensión para registrar los servicios en el contenedor de DI.</summary>
public static class DependencyInjection
{
    /// <summary>Registra todos los servicios de MicroByt.IA en el <see cref="IServiceCollection"/> proporcionado.</summary>
    public static IServiceCollection AddMicrobytIA(this IServiceCollection services)
    {
        services.AddMemoryCache();

        services.AddSingleton<IProviderChatClientFactory, ProviderChatClientFactory>();
        services.AddSingleton<IPromptsCollectionService, PromptsCollectionService>();
        services.AddSingleton<IFileSkillCacheService, FileSkillCacheService>();
        services.AddScoped<ISkillsAgentService, SkillsAgentService>();
        services.AddScoped<ISkillsService, SkillsService>();

        services.AddSingleton<IToolChain, WebSearchToolChain>();
        services.AddSingleton<IToolChainRegistry, ToolChainRegistry>();

        return services;
    }
}