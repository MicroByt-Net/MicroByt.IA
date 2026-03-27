using MicroByt.IA.Application.Interfaces;
using MicroByt.IA.Core.Entities.Data.AI;

namespace MicroByt.IA.Application.Services;

/// <summary>Implementación de <see cref="IProviderHttpClientFactory"/> que crea <see cref="HttpClient"/> configurados para un <see cref="Provider"/>.</summary>
public class ProviderHttpClientFactory : IProviderHttpClientFactory
{
    private readonly Dictionary<string, HttpClient> _clients = [];

    /// <summary>Obtiene un <see cref="HttpClient"/> configurado para el proveedor indicado, creándolo si aún no existe.</summary>
    /// <param name="provider">Proveedor de IA cuya configuración se aplica al cliente HTTP.</param>
    /// <returns>Un <see cref="HttpClient"/> listo para realizar peticiones al proveedor.</returns>
    public HttpClient GetClient(Provider provider)
    {
        if (_clients.TryGetValue(provider.Name, out var existing))
            return existing;

        var client = new HttpClient
        {
            BaseAddress = new Uri(provider.UrlBase),
        };

        if (!string.IsNullOrEmpty(provider.ApiKey))
            client.DefaultRequestHeaders.Add("Authorization", $"Bearer {provider.ApiKey}");

        _clients[provider.Name] = client;

        return client;
    }
}