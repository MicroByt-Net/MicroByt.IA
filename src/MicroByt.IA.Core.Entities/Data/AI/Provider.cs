namespace MicroByt.IA.Core.Entities.Data.AI;

/// <summary>Proveedor de modelos de IA con su configuración de conexión.</summary>
public class Provider
{
    /// <summary>URL base del endpoint de la API del proveedor.</summary>
    public string UrlBase { get; init; } = string.Empty;

    /// <summary>Clave de autenticación para acceder a la API del proveedor.</summary>
    public string ApiKey { get; init; } = string.Empty;

    /// <summary>Indica si el proveedor es un modelo local (sin llamadas a servicios externos).</summary>
    public bool Local { get; init; }

    // TODO: clase temporal
    public static readonly Provider OpenAI = new()
    {
        UrlBase = "https://api.openai.com",
        ApiKey = "apikey",
        Local = false,
    };
}
