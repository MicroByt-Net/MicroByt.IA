# Memoria corta en Telegram — MiniClaw.NET

En Telegram no existe evento de "cerrar sesión".
La sesión hay que definirla artificialmente por inactividad.

---

## El problema

El hilo de Telegram es infinito y continuo.
Enviar todo el historial al LLM supera el límite de contexto.
No enviar nada hace que el agente pierda el hilo de la conversación.

---

## Estrategias disponibles

### A — Ventana deslizante ✅ MVP

Envía siempre los últimos N mensajes al LLM como contexto inmediato.

- N recomendado: 15–20 mensajes según presupuesto de tokens
- Simple, predecible, sin coste extra de llamadas al LLM
- Pierde contexto si la conversación es muy larga
- Todos los mensajes se persisten en BD igualmente

```csharp
// Recuperar los últimos N mensajes de la sesión activa
var messages = await db.Messages
    .Where(m => m.SessionId == session.Id)
    .OrderByDescending(m => m.CreadoEn)
    .Take(15)
    .OrderBy(m => m.CreadoEn)
    .ToListAsync();
```

---

### B — Sesión por inactividad ✅ MVP

Nueva `AgentSession` si el usuario lleva más de X minutos sin escribir.

- Umbral recomendado: 30 minutos de inactividad
- Al detectar nueva sesión → se puede resumir la anterior con el LLM
- El resumen se guarda en `AgentSession.ResumenFinal`
- La siguiente sesión arranca con ese resumen como contexto inicial

```csharp
// Detectar si hay que abrir nueva sesión
var session = await db.AgentSessions
    .Where(s => s.UserId == userId)
    .OrderByDescending(s => s.UltimoMensajeEn)
    .FirstOrDefaultAsync();

var inactivo = session is null ||
               DateTime.UtcNow - session.UltimoMensajeEn > TimeSpan.FromMinutes(30);

if (inactivo)
    session = await AbrirNuevaSesionAsync(userId, chatId);
```

---

### C — Resumen rolling (avanzado)

Cuando el historial supera X tokens, el LLM lo comprime.
Siempre se envía: `resumen_anterior + últimos N mensajes`.
El contexto nunca explota aunque la conversación sea muy larga.
Añade una llamada extra al LLM → mayor latencia y coste.

---

## Recomendación para el MVP

**A + B combinadas:**

| Capa | Qué hace |
|---|---|
| Ventana deslizante | Envía los últimos 15 mensajes como contexto inmediato |
| Sesión por inactividad | Nueva sesión tras 30 min sin mensajes |
| Resumen al cerrar sesión | El LLM resume la sesión anterior y se guarda en BD |
| Memoria larga | El resumen + preferencias se inyectan en el system prompt |

---

## Modelo de dominio

```csharp
public class AgentSession
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string UserId { get; set; } = default!;
    public long ChatId { get; set; }              // ID del chat de Telegram
    public DateTime IniciadaEn { get; set; } = DateTime.UtcNow;
    public DateTime UltimoMensajeEn { get; set; } = DateTime.UtcNow;
    public string? ResumenFinal { get; set; }     // generado al cerrar sesión
    public ICollection<Message> Messages { get; set; } = [];
}

public class Message
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid SessionId { get; set; }
    public string Role { get; set; } = default!;  // "user" | "assistant"
    public string Content { get; set; } = default!;
    public DateTime CreadoEn { get; set; } = DateTime.UtcNow;
}
```

---

## Cómo se construye el contexto en cada llamada

```csharp
private async Task<string> BuildSystemPromptAsync(string userId)
{
    // 1. Memoria larga: preferencias y perfil persistidos
    var memoria = await memoryService.GetAllAsync(userId);
    var memoriaTexto = string.Join("\n", memoria.Select(m => $"- {m.Clave}: {m.Valor}"));

    // 2. Resumen de la sesión anterior (si existe)
    var sessionAnterior = await db.AgentSessions
        .Where(s => s.UserId == userId && s.ResumenFinal != null)
        .OrderByDescending(s => s.IniciadaEn)
        .Skip(1).FirstOrDefaultAsync();
    var resumen = sessionAnterior?.ResumenFinal ?? string.Empty;

    // 3. Skill activa
    var skill = await skillLoader.LoadAsync(skillActiva);

    return $"""
        Eres MiniClaw, un asistente personal.

        ## Lo que sabes del usuario
        {memoriaTexto}

        ## Resumen de la conversación anterior
        {resumen}

        ## Comportamiento activo
        {skill}
        """;
}

private async Task<List<object>> BuildMessagesAsync(Guid sessionId)
{
    // Contexto inmediato: últimos 15 mensajes de la sesión activa
    return await db.Messages
        .Where(m => m.SessionId == sessionId)
        .OrderByDescending(m => m.CreadoEn)
        .Take(15)
        .OrderBy(m => m.CreadoEn)
        .Select(m => (object)new { role = m.Role, content = m.Content })
        .ToListAsync();
}
```

---

## Flujo completo por cada mensaje de Telegram

```
1. Llega update de Telegram (userId + chatId + texto)
2. Comprobar sesión activa → si inactivo >30min, abrir nueva
3. Guardar mensaje del usuario en BD (Message, role: "user")
4. BuildSystemPrompt → memoria larga + resumen anterior + skill
5. BuildMessages → últimos 15 mensajes de la sesión
6. Llamar al LLM con system + messages + tools
7. Ejecutar tool calls si las hay (save_memory, notes_write, etc.)
8. Guardar respuesta en BD (Message, role: "assistant")
9. Actualizar AgentSession.UltimoMensajeEn
10. Enviar respuesta al usuario por Telegram
```
