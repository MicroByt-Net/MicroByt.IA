using System.ClientModel;
using MicroByt.IA.Infrastructure.Interfaces;
using MicroByt.IA.Domain.AI;
using OpenAI;
using OpenAI.Chat;

namespace MicroByt.IA.Infrastructure.Services;

/// <summary>Implementación de <see cref="IProviderChatClientFactory"/> que crea <see cref="ChatClient"/> configurados para un <see cref="AIModel"/>.</summary>
public class ProviderChatClientFactory : IProviderChatClientFactory
{
    private readonly Dictionary<string, ChatClient> _clients = [];

    /// <summary>Obtiene un <see cref="ChatClient"/> configurado para el modelo indicado, creándolo si aún no existe.</summary>
    /// <param name="aiModel">Modelo de IA que incluye el proveedor y el identificador de modelo.</param>
    /// <returns>Un <see cref="ChatClient"/> listo para realizar peticiones al proveedor.</returns>
    public ChatClient GetClient(AIModel aiModel)
    {
        var key = $"{aiModel.Provider.Name}:{aiModel.Model}";

        if (_clients.TryGetValue(key, out var existing))
            return existing;

        var options = new OpenAIClientOptions
        {
            Endpoint = new Uri(aiModel.Provider.UrlBase),
        };

        var credential = new ApiKeyCredential(aiModel.Provider.ApiKey ?? string.Empty);
        var client = new ChatClient(aiModel.Model, credential, options);

        _clients[key] = client;

        return client;
    }
}