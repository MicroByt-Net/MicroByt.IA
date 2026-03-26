namespace MicroByt.IA.Core.Entities.Data.AI;

public class Provider
{
    public string UrlBase { get; init; } = string.Empty;
    public string ApiKey { get; init; } = string.Empty;
    public bool Local { get; init; }

    // TODO: clase temporal
    public static readonly Provider OpenAI = new()
    {
        UrlBase = "https://api.openai.com",
        ApiKey = "apikey",
        Local = false,
    };
}
