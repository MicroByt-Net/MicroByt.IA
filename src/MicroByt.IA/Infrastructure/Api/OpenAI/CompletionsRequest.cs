using System.Text.Json.Serialization;

namespace MicroByt.IA.Infrastructure.Api.OpenAI;

/// <summary>Body del request para POST /v1/chat/completions.</summary>
public class CompletionsRequest
{
    /// <summary>ID del modelo a usar, por ejemplo "gpt-4o".</summary>
    [JsonPropertyName("model")]
    public string Model { get; init; } = string.Empty;

    /// <summary>Lista de mensajes que conforman la conversación.</summary>
    [JsonPropertyName("messages")]
    public IReadOnlyList<Message> Messages { get; init; } = [];

    /// <summary>
    /// Aleatoriedad de la salida. Valores entre 0 y 2.
    /// Valores más altos generan respuestas más creativas; valores más bajos, más deterministas.
    /// </summary>
    [JsonPropertyName("temperature")]
    public float? Temperature { get; init; }

    /// <summary>Límite máximo de tokens que puede generar el modelo en la respuesta.</summary>
    [JsonPropertyName("max_completion_tokens")]
    public int? MaxCompletionTokens { get; init; }

    /// <summary>
    /// Muestreo por núcleo (nucleus sampling). El modelo considera solo los tokens
    /// cuya probabilidad acumulada alcanza este valor. Alternativa a Temperature.
    /// </summary>
    [JsonPropertyName("top_p")]
    public float? TopP { get; init; }

    /// <summary>Cantidad de respuestas alternativas (choices) a generar por cada mensaje. Por defecto 1.</summary>
    [JsonPropertyName("n")]
    public int? N { get; init; }

    /// <summary>Si es true, la respuesta se envía en streaming como eventos Server-Sent Events (SSE).</summary>
    [JsonPropertyName("stream")]
    public bool? Stream { get; init; }

    /// <summary>
    /// Secuencias de texto donde el modelo debe dejar de generar tokens.
    /// Se pueden indicar hasta 4 cadenas.
    /// </summary>
    [JsonPropertyName("stop")]
    public IReadOnlyList<string>? Stop { get; init; }

    /// <summary>
    /// Penalización por presencia: valores positivos incentivan al modelo a hablar
    /// de temas nuevos. Rango de -2.0 a 2.0.
    /// </summary>
    [JsonPropertyName("presence_penalty")]
    public float? PresencePenalty { get; init; }

    /// <summary>
    /// Penalización por frecuencia: valores positivos reducen la repetición de tokens
    /// ya usados. Rango de -2.0 a 2.0.
    /// </summary>
    [JsonPropertyName("frequency_penalty")]
    public float? FrequencyPenalty { get; init; }

    /// <summary>Identificador del usuario final, útil para detección de abuso.</summary>
    [JsonPropertyName("user")]
    public string? User { get; init; }

    /// <summary>Lista de herramientas (funciones) que el modelo puede invocar.</summary>
    [JsonPropertyName("tools")]
    public IReadOnlyList<RequestTool>? Tools { get; init; }

    /// <summary>
    /// Controla qué herramienta usa el modelo. Valores posibles: "none", "auto", "required",
    /// o el nombre de una herramienta específica.
    /// </summary>
    [JsonPropertyName("tool_choice")]
    public string? ToolChoice { get; init; }

    /// <summary>Formato de la respuesta. Permite forzar salida en JSON con "json_object".</summary>
    [JsonPropertyName("response_format")]
    public ResponseFormat? ResponseFormat { get; init; }

    /// <summary>
    /// Semilla para reproducibilidad. Con el mismo seed y parámetros, el modelo
    /// debería generar la misma respuesta (best effort).
    /// </summary>
    [JsonPropertyName("seed")]
    public int? Seed { get; init; }
}

/// <summary>Mensaje dentro de la conversación.</summary>
public class Message
{
    /// <summary>Rol del autor del mensaje: "system", "user", "assistant" o "tool".</summary>
    [JsonPropertyName("role")]
    public string Role { get; init; } = string.Empty;

    /// <summary>Contenido textual del mensaje. Puede ser null en mensajes del assistant con tool_calls.</summary>
    [JsonPropertyName("content")]
    public string? Content { get; init; }

    /// <summary>Nombre del participante, útil para diferenciar usuarios en conversaciones multi-usuario.</summary>
    [JsonPropertyName("name")]
    public string? Name { get; init; }

    /// <summary>ID de la tool call a la que responde este mensaje (solo para role "tool").</summary>
    [JsonPropertyName("tool_call_id")]
    public string? ToolCallId { get; init; }

    /// <summary>Llamadas a herramientas solicitadas por el assistant en su respuesta.</summary>
    [JsonPropertyName("tool_calls")]
    public IReadOnlyList<ToolCall>? ToolCalls { get; init; }

    /// <summary>Mensaje de rechazo generado por el modelo cuando activa sus mecanismos de seguridad.</summary>
    [JsonPropertyName("refusal")]
    public string? Refusal { get; init; }
}

/// <summary>Llamada a una herramienta solicitada por el modelo en su respuesta.</summary>
public class ToolCall
{
    /// <summary>Identificador único de esta tool call, necesario para correlacionar la respuesta de la tool.</summary>
    [JsonPropertyName("id")]
    public string Id { get; init; } = string.Empty;

    /// <summary>Tipo de la tool call. Actualmente siempre "function".</summary>
    [JsonPropertyName("type")]
    public string Type { get; init; } = "function";

    /// <summary>Función que el modelo solicita ejecutar.</summary>
    [JsonPropertyName("function")]
    public ToolCallFunction Function { get; init; } = new();
}

/// <summary>Función específica dentro de una tool call.</summary>
public class ToolCallFunction
{
    /// <summary>Nombre de la función a invocar.</summary>
    [JsonPropertyName("name")]
    public string Name { get; init; } = string.Empty;

    /// <summary>Argumentos de la función serializados como JSON string.</summary>
    [JsonPropertyName("arguments")]
    public string Arguments { get; init; } = string.Empty;
}

/// <summary>Herramienta disponible para que el modelo pueda invocar.</summary>
public class RequestTool
{
    /// <summary>Tipo de herramienta. Actualmente siempre "function".</summary>
    [JsonPropertyName("type")]
    public string Type { get; init; } = "function";

    /// <summary>Definición de la función que expone esta herramienta.</summary>
    [JsonPropertyName("function")]
    public FunctionDefinition Function { get; init; } = new();
}

/// <summary>Definición de una función que el modelo puede invocar.</summary>
public class FunctionDefinition
{
    /// <summary>Nombre de la función. Debe ser único y descriptivo (sin espacios).</summary>
    [JsonPropertyName("name")]
    public string Name { get; init; } = string.Empty;

    /// <summary>Descripción de qué hace la función, usada por el modelo para decidir cuándo invocarla.</summary>
    [JsonPropertyName("description")]
    public string? Description { get; init; }

    /// <summary>JSON Schema que describe los parámetros que acepta la función.</summary>
    [JsonPropertyName("parameters")]
    public object? Parameters { get; init; }

    /// <summary>Si es true, el modelo debe respetar estrictamente el schema de parámetros definido.</summary>
    [JsonPropertyName("strict")]
    public bool? Strict { get; init; }
}

/// <summary>Formato de salida de la respuesta del modelo.</summary>
public class ResponseFormat
{
    /// <summary>
    /// Tipo de formato: "text" (por defecto) o "json_object" para forzar respuesta en JSON válido.
    /// </summary>
    [JsonPropertyName("type")]
    public string Type { get; init; } = "text";
}