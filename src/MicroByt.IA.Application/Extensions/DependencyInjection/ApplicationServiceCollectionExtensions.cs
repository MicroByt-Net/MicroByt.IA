using MicroByt.IA.Application.Interfaces;
using MicroByt.IA.Application.Services;
using Microsoft.Extensions.DependencyInjection;

namespace MicroByt.IA.Application.Extensions.DependencyInjection;

/// <summary>Métodos de extensión para registrar los servicios de la capa Application en el contenedor de DI.</summary>
public static class ApplicationServiceCollectionExtensions
{
    /// <summary>Registra todos los servicios de MicroByt.IA.Application en el <see cref="IServiceCollection"/> proporcionado.</summary>
    /// <param name="services">Colección de servicios donde se registran las dependencias.</param>
    /// <returns>La misma instancia de <see cref="IServiceCollection"/> para encadenar llamadas.</returns>
    public static IServiceCollection AddMicrobytIAApplication(this IServiceCollection services)
    {
        services.AddSingleton<IProviderChatClientFactory, ProviderChatClientFactory>();
        services.AddScoped<ISkillsToolsCollectionService, SkillsToolsCollectionService>();

        return services;
    }
}