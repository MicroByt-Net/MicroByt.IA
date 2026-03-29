# Tools — MiniClaw.NET

Las tools son capacidades de ejecución registradas en el `ToolRegistry`.
El LLM las invoca mediante tool calling cuando necesita actuar sobre el mundo.
Cada tool tiene nombre, descripción y JSON Schema de parámetros.

---

## web_search

**Nombre:** `web_search`
**Propósito:** Buscar información actualizada en la web.
**Cuándo la usa el LLM:** El usuario pide datos recientes, comparaciones o noticias.

```json
{
  "name": "web_search",
  "description": "Busca información actualizada en la web y devuelve un resumen de los resultados.",
  "input_schema": {
    "type": "object",
    "properties": {
      "query": {
        "type": "string",
        "description": "Términos de búsqueda. Ej: 'arquitecturas de agentes en .NET 2024'"
      }
    },
    "required": ["query"]
  }
}
```

---

## notes_read

**Nombre:** `notes_read`
**Propósito:** Leer las notas guardadas del usuario.
**Cuándo la usa el LLM:** El usuario pide ver, buscar o resumir sus notas.

```json
{
  "name": "notes_read",
  "description": "Lee las notas persistidas del usuario. Puede filtrar por término de búsqueda.",
  "input_schema": {
    "type": "object",
    "properties": {
      "filtro": {
        "type": "string",
        "description": "Término opcional para filtrar notas. Vacío devuelve todas."
      }
    },
    "required": []
  }
}
```

---

## notes_write

**Nombre:** `notes_write`
**Propósito:** Guardar o actualizar una nota del usuario.
**Cuándo la usa el LLM:** El usuario pide apuntar, guardar o registrar algo.

```json
{
  "name": "notes_write",
  "description": "Guarda o actualiza una nota del usuario en la base de datos.",
  "input_schema": {
    "type": "object",
    "properties": {
      "titulo": {
        "type": "string",
        "description": "Título breve de la nota. Ej: 'Módulos de MiniClaw'"
      },
      "contenido": {
        "type": "string",
        "description": "Cuerpo completo de la nota."
      }
    },
    "required": ["titulo", "contenido"]
  }
}
```

---

## task_create

**Nombre:** `task_create`
**Propósito:** Crear una tarea o recordatorio.
**Cuándo la usa el LLM:** El usuario pide crear, apuntar o programar una tarea.

```json
{
  "name": "task_create",
  "description": "Crea una tarea pendiente con título, descripción opcional y fecha límite.",
  "input_schema": {
    "type": "object",
    "properties": {
      "titulo": {
        "type": "string",
        "description": "Título de la tarea. Ej: 'Implementar MemoryService'"
      },
      "descripcion": {
        "type": "string",
        "description": "Detalle opcional de la tarea."
      },
      "fecha_limite": {
        "type": "string",
        "description": "Fecha y hora en ISO 8601. Ej: '2024-12-01T18:00:00Z'"
      }
    },
    "required": ["titulo"]
  }
}
```

---

## save_memory

**Nombre:** `save_memory`
**Propósito:** Guardar un dato duradero del usuario en memoria larga.
**Cuándo la usa el LLM:** Detecta una preferencia, hecho o dato de perfil que debe persistir entre sesiones.

```json
{
  "name": "save_memory",
  "description": "Guarda un dato importante del usuario en memoria persistente entre sesiones: preferencias, hechos relevantes o datos de perfil.",
  "input_schema": {
    "type": "object",
    "properties": {
      "tipo": {
        "type": "string",
        "description": "Categoría del dato: 'preferencia', 'hecho' o 'perfil'."
      },
      "clave": {
        "type": "string",
        "description": "Identificador único del dato. Ej: 'estilo_respuesta'"
      },
      "valor": {
        "type": "string",
        "description": "Valor a guardar. Ej: 'técnico, breve, en español'"
      }
    },
    "required": ["tipo", "clave", "valor"]
  }
}
```

---

## Registro en `ToolRegistry`

```csharp
public static class ToolRegistry
{
    public static IEnumerable<object> All =>
    [
        WebSearchDefinition,
        NotesReadDefinition,
        NotesWriteDefinition,
        TaskCreateDefinition,
        SaveMemoryDefinition
    ];
}
```

El `Orchestrator` pasa `ToolRegistry.All` en cada llamada al LLM.
El LLM elige qué tool invocar según el contexto. El Orchestrator
ejecuta la tool y devuelve el `tool_result` para que el LLM
genere la respuesta final en lenguaje natural.
