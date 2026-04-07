namespace MicroByt.IA.Infrastructure.Options;

/// <summary>Opciones fuertemente tipadas para la sección <c>ApiKeys</c> de appsettings.json.</summary>
public class ApiKeysOptions
{
    public const string SectionName = "ApiKeys";

    public string? Tavily { get; init; }
}