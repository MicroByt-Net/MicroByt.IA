using System.Text.Json.Serialization;

namespace MicroByt.IA.Core.Entities.Api.OpenAI;

/// <summary>Respuesta de la API para POST /v1/chat/completions.</summary>
public class CompletionsResponse
{
    /// <summary>Identificador único de la completion, con formato "chatcmpl-...".</summary>
    [JsonPropertyName("id")]
    public string Id { get; init; } = string.Empty;

    /// <summary>Tipo de objeto devuelto. Siempre "chat.completion".</summary>
    [JsonPropertyName("object")]
    public string Object { get; init; } = string.Empty;

    /// <summary>Timestamp Unix (segundos) del momento en que se generó la completion.</summary>
    [JsonPropertyName("created")]
    public long Created { get; init; }

    /// <summary>ID del modelo que generó la respuesta.</summary>
    [JsonPropertyName("model")]
    public string Model { get; init; } = string.Empty;

    /// <summary>Lista de respuestas generadas. Contiene más de un elemento si N > 1 en el request.</summary>
    [JsonPropertyName("choices")]
    public IReadOnlyList<Choice> Choices { get; init; } = [];

    /// <summary>Estadísticas de tokens consumidos en el request y la respuesta.</summary>
    [JsonPropertyName("usage")]
    public Usage? Usage { get; init; }

    /// <summary>Huella del estado interno del sistema al momento de la respuesta, útil para reproducibilidad con seed.</summary>
    [JsonPropertyName("system_fingerprint")]
    public string? SystemFingerprint { get; init; }
}

/// <summary>Una de las respuestas alternativas generadas por el modelo.</summary>
public class Choice
{
    /// <summary>Índice de esta choice dentro del array (base 0).</summary>
    [JsonPropertyName("index")]
    public int Index { get; init; }

    /// <summary>Mensaje generado por el modelo para esta choice.</summary>
    [JsonPropertyName("message")]
    public Message Message { get; init; } = new();

    /// <summary>
    /// Razón por la que el modelo dejó de generar tokens.
    /// Valores posibles: "stop", "length", "tool_calls", "content_filter", "function_call".
    /// </summary>
    [JsonPropertyName("finish_reason")]
    public string? FinishReason { get; init; }

    /// <summary>Log-probabilities de los tokens generados. Solo presente si se solicitó en el request.</summary>
    [JsonPropertyName("logprobs")]
    public LogProbs? LogProbs { get; init; }
}

/// <summary>Conteo de tokens utilizados en la request y la response.</summary>
public class Usage
{
    /// <summary>Tokens consumidos por el prompt (mensajes de entrada).</summary>
    [JsonPropertyName("prompt_tokens")]
    public int PromptTokens { get; init; }

    /// <summary>Tokens generados en la respuesta.</summary>
    [JsonPropertyName("completion_tokens")]
    public int CompletionTokens { get; init; }

    /// <summary>Total de tokens: prompt + completion.</summary>
    [JsonPropertyName("total_tokens")]
    public int TotalTokens { get; init; }

    /// <summary>Desglose adicional de los tokens del prompt (caché, audio, etc.).</summary>
    [JsonPropertyName("prompt_tokens_details")]
    public PromptTokenDetails? PromptTokensDetails { get; init; }

    /// <summary>Desglose adicional de los tokens generados (razonamiento, audio, predicciones, etc.).</summary>
    [JsonPropertyName("completion_tokens_details")]
    public CompletionTokenDetails? CompletionTokensDetails { get; init; }
}

/// <summary>Desglose de los tokens del prompt por tipo.</summary>
public class PromptTokenDetails
{
    /// <summary>Tokens del prompt que fueron servidos desde caché (reducen latencia y costo).</summary>
    [JsonPropertyName("cached_tokens")]
    public int CachedTokens { get; init; }

    /// <summary>Tokens de audio incluidos en el prompt.</summary>
    [JsonPropertyName("audio_tokens")]
    public int AudioTokens { get; init; }
}

/// <summary>Desglose de los tokens generados en la completion por tipo.</summary>
public class CompletionTokenDetails
{
    /// <summary>Tokens usados internamente por el modelo para razonamiento (no visibles en la respuesta final).</summary>
    [JsonPropertyName("reasoning_tokens")]
    public int ReasoningTokens { get; init; }

    /// <summary>Tokens de audio generados en la respuesta.</summary>
    [JsonPropertyName("audio_tokens")]
    public int AudioTokens { get; init; }

    /// <summary>Tokens de predicción especulativa que coincidieron con la salida real.</summary>
    [JsonPropertyName("accepted_prediction_tokens")]
    public int AcceptedPredictionTokens { get; init; }

    /// <summary>Tokens de predicción especulativa que no coincidieron y fueron descartados.</summary>
    [JsonPropertyName("rejected_prediction_tokens")]
    public int RejectedPredictionTokens { get; init; }
}

/// <summary>Log-probabilities de los tokens generados en una choice.</summary>
public class LogProbs
{
    /// <summary>Log-probabilities de cada token del contenido generado.</summary>
    [JsonPropertyName("content")]
    public IReadOnlyList<LogProbContent>? Content { get; init; }

    /// <summary>Log-probabilities de los tokens del mensaje de rechazo, si aplica.</summary>
    [JsonPropertyName("refusal")]
    public IReadOnlyList<LogProbContent>? Refusal { get; init; }
}

/// <summary>Log-probability de un token individual generado por el modelo.</summary>
public class LogProbContent
{
    /// <summary>El token generado como string.</summary>
    [JsonPropertyName("token")]
    public string Token { get; init; } = string.Empty;

    /// <summary>Log-probability del token. Valores más cercanos a 0 indican mayor certeza.</summary>
    [JsonPropertyName("logprob")]
    public float LogProb { get; init; }

    /// <summary>Representación del token como lista de bytes UTF-8. Útil para tokens con caracteres especiales.</summary>
    [JsonPropertyName("bytes")]
    public IReadOnlyList<int>? Bytes { get; init; }

    /// <summary>Los N tokens alternativos más probables en esta posición, junto con sus log-probabilities.</summary>
    [JsonPropertyName("top_logprobs")]
    public IReadOnlyList<TopLogProb>? TopLogProbs { get; init; }
}

/// <summary>Token alternativo con su log-probability, parte del top-N en una posición dada.</summary>
public class TopLogProb
{
    /// <summary>El token alternativo como string.</summary>
    [JsonPropertyName("token")]
    public string Token { get; init; } = string.Empty;

    /// <summary>Log-probability de este token alternativo.</summary>
    [JsonPropertyName("logprob")]
    public float LogProb { get; init; }

    /// <summary>Representación del token alternativo como lista de bytes UTF-8.</summary>
    [JsonPropertyName("bytes")]
    public IReadOnlyList<int>? Bytes { get; init; }
}
