using MicroByt.IA.Core.Entities.Data.AI;

namespace MicroByt.IA.Application.Interfaces;

/// <summary>Fábrica para crear instancias de <see cref="HttpClient"/> configuradas según un <see cref="Provider"/>.</summary>
public interface IProviderHttpClientFactory
{
    /// <summary>Obtiene un <see cref="HttpClient"/> configurado para el proveedor indicado, creándolo si aún no existe.</summary>
    /// <param name="provider">Proveedor de IA cuya configuración se aplica al cliente HTTP.</param>
    /// <returns>Un <see cref="HttpClient"/> listo para realizar peticiones al proveedor.</returns>
    HttpClient GetClient(Provider provider);
}
