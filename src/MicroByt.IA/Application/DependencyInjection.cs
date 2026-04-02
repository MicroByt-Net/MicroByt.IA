using MicroByt.IA.Application.Interfaces;
using MicroByt.IA.Application.Services;
using MicroByt.IA.Infrastructure.Services;
using Microsoft.Extensions.DependencyInjection;

namespace MicroByt.IA.Application;

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

        return services;
    }
}