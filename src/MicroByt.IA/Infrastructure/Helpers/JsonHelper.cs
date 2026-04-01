using System.Text.Json;
using System.Text.RegularExpressions;
using MicroByt.IA.Infrastructure.Exceptions;

namespace MicroByt.IA.Infrastructure.Helpers;

public static class JsonHelper
{
    // Matches <think>...</think> blocks produced by reasoning models (e.g. DeepSeek R1).
    private static readonly Regex ThinkBlockRegex =
        new(@"<think>[\s\S]*?</think>", RegexOptions.IgnoreCase | RegexOptions.Compiled);

    /// <summary>
    /// Sanitizes raw AI output so it contains only the JSON payload.
    /// Handles: &lt;think&gt; blocks, markdown fences, and leading/trailing prose.
    /// </summary>
    public static string Sanitize(string raw)
    {
        // 1. Strip <think>...</think> blocks (DeepSeek R1, o1-style reasoning).
        var text = ThinkBlockRegex.Replace(raw, string.Empty);

        // 2. Work on a trimmed span to strip markdown code fences.
        var span = text.AsSpan().Trim();

        const string jsonFence = "```json";
        const string fence = "```";

        if (span.StartsWith(jsonFence, StringComparison.OrdinalIgnoreCase))
            span = span[jsonFence.Length..].TrimStart();
        else if (span.StartsWith(fence, StringComparison.Ordinal))
            span = span[fence.Length..].TrimStart();

        if (span.EndsWith(fence, StringComparison.Ordinal))
            span = span[..^fence.Length].TrimEnd();

        // 3. Extract the first JSON object or array, discarding surrounding prose.
        var str = span.ToString();
        var start = FindJsonStart(str);
        if (start >= 0)
        {
            var end = FindJsonEnd(str, start);
            if (end > start)
                str = str[start..(end + 1)];
        }

        // 4. Escape literal control characters inside JSON strings (e.g. 0x0A, 0x0D).
        str = EscapeControlCharsInStrings(str);

        return str;
    }

    public static T? Deserialize<T>(string json, JsonSerializerOptions? options = null)
    {
        var sanitized = Sanitize(json);

        try
        {
            return JsonSerializer.Deserialize<T>(sanitized, options);
        }
        catch (Exception ex)
        {
            throw new DeserializeJsonIAException(
                "Error al deserializar la respuesta del modelo. Revisa la consola para más detalles.",
                json,
                sanitized,
                ex);
        }
    }

    // Escapes literal control characters (0x00–0x1F) that appear inside JSON string
    // values. The AI sometimes emits raw newlines/tabs instead of \n/\t, which makes
    // the JSON invalid per the spec.
    private static string EscapeControlCharsInStrings(string json)
    {
        var sb = new System.Text.StringBuilder(json.Length);
        var inString = false;

        for (var i = 0; i < json.Length; i++)
        {
            var c = json[i];

            if (inString)
            {
                if (c == '\\')
                {
                    // Pass through the escape sequence unchanged.
                    sb.Append(c);
                    if (i + 1 < json.Length)
                        sb.Append(json[++i]);
                    continue;
                }

                if (c == '"')
                {
                    inString = false;
                    sb.Append(c);
                    continue;
                }

                // Literal control character inside a string — escape it.
                if (c < 0x20)
                {
                    sb.Append(c switch
                    {
                        '\b' => @"\b",
                        '\f' => @"\f",
                        '\n' => @"\n",
                        '\r' => @"\r",
                        '\t' => @"\t",
                        _    => $@"\u{(int)c:x4}",
                    });
                    continue;
                }
            }
            else if (c == '"')
            {
                inString = true;
            }

            sb.Append(c);
        }

        return sb.ToString();
    }

    // Returns the index of the first '{' or '[' in the string, or -1.
    private static int FindJsonStart(string text)
    {
        for (var i = 0; i < text.Length; i++)
        {
            if (text[i] is '{' or '[')
                return i;
        }
        return -1;
    }

    // Returns the index of the matching closing delimiter for the opening one at `start`.
    private static int FindJsonEnd(string text, int start)
    {
        var open = text[start];
        var close = open == '{' ? '}' : ']';
        var depth = 0;
        var inString = false;

        for (var i = start; i < text.Length; i++)
        {
            var c = text[i];

            if (inString)
            {
                if (c == '\\') { i++; continue; } // skip escaped char
                if (c == '"') inString = false;
                continue;
            }

            if (c == '"') { inString = true; continue; }
            if (c == open) depth++;
            else if (c == close)
            {
                depth--;
                if (depth == 0) return i;
            }
        }

        return -1;
    }
}