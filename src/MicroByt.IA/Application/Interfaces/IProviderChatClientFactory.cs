using MicroByt.IA.Domain.AI;
using OpenAI.Chat;

namespace MicroByt.IA.Application.Interfaces;

/// <summary>Fábrica para crear instancias de <see cref="ChatClient"/> configuradas según un <see cref="AIModel"/>.</summary>
public interface IProviderChatClientFactory
{
    /// <summary>Obtiene un <see cref="ChatClient"/> configurado para el modelo indicado, creándolo si aún no existe.</summary>
    /// <param name="aiModel">Modelo de IA que incluye el proveedor y el identificador de modelo.</param>
    /// <returns>Un <see cref="ChatClient"/> listo para realizar peticiones al proveedor.</returns>
    ChatClient GetClient(AIModel aiModel);
}