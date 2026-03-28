using System.Text.Json;

namespace MicroByt.AI.Core.Common.Helpers;

public static class JsonHelper
{
    public static T? Deserialize<T>(string json, JsonSerializerOptions? options = null)
    {
        var trimmed = json.AsSpan().Trim();

        const string jsonFence = "```json";
        const string fence = "```";

        if (trimmed.StartsWith(jsonFence, StringComparison.OrdinalIgnoreCase))
            trimmed = trimmed[jsonFence.Length..].TrimStart();

        if (trimmed.EndsWith(fence, StringComparison.Ordinal))
            trimmed = trimmed[..^fence.Length].TrimEnd();

        return JsonSerializer.Deserialize<T>(trimmed, options);
    }
}