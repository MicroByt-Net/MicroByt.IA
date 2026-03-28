namespace MicroByt.AI.Core.Common.Exceptions;

public class DeserializeJsonIAException(string message, string json, string jsonSanitized, Exception? inner = null)
    : Exception(message, inner)
{
    public string Json { get; } = json;
    public string JsonSanitized { get; } = jsonSanitized;
}