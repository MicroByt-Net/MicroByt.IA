# Web Search — Opciones de implementación

Análisis de las alternativas para implementar `WebSearchToolChain` en MicroByt.IA.

## Opciones evaluadas

### Tavily ⭐ (recomendada para agentes IA)

- **Endpoint:** `POST https://api.tavily.com/search`
- **Free tier:** 1.000 req/mes (requiere registro y API key)
- **Respuesta:** contenido completo del artículo + `answer` sintetizado por IA
- **Dependencias extra:** ninguna (solo `HttpClient` + `System.Text.Json`)

```json
// Request
{ "api_key": "...", "query": "...", "max_results": 5 }

// Response
{
  "answer": "Resumen generado por Tavily...",
  "results": [
    { "title": "...", "url": "...", "content": "párrafo limpio del artículo" }
  ]
}
```

**Pros:** diseñada para agentes IA, devuelve contenido procesado y listo para consumir por el LLM, latencia baja.  
**Contras:** 1.000 req/mes en free, servicio de pago si se escala.

---

### Brave Search API

- **Endpoint:** `GET https://api.search.brave.com/res/v1/web/search?q=...`
- **Free tier:** 2.000 req/mes (requiere registro y API key)
- **Respuesta:** título + URL + snippet ~150 caracteres (sin contenido completo)
- **Dependencias extra:** ninguna

**Pros:** free tier más generoso (2.000/mes), índice propio independiente de Google.  
**Contras:** solo devuelve snippets cortos — el LLM tiene poco contexto para razonar sin hacer scraping adicional.

---

### Google Custom Search JSON API

- **Endpoint:** `GET https://www.googleapis.com/customsearch/v1?q=...&key=...&cx=...`
- **Free tier:** 100 req/día (~3.000/mes)
- **Respuesta:** título + URL + snippet (similar a Brave)
- **Setup:** requiere crear un Programmable Search Engine en Google Cloud Console

**Pros:** resultados de Google, free tier razonable.  
**Contras:** configuración tediosa (motor + API key + cx), solo snippets cortos.

---

### DuckDuckGo Instant Answer API

- **Endpoint:** `GET https://api.duckduckgo.com/?q=...&format=json`
- **Free tier:** ilimitado, sin key, sin registro
- **Respuesta:** respuestas directas tipo Wikipedia (definiciones, conversiones...)

**Contras importantes:** no devuelve resultados web reales. Solo responde ante queries con "respuesta directa" conocida. Inútil para búsquedas abiertas o comparativas.

---

### SearXNG (self-hosted)

- **Endpoint:** instancia propia o pública, p.ej. `GET https://searx.be/search?q=...&format=json`
- **Free tier:** ilimitado si se auto-hostea (Docker)
- **Respuesta:** resultados agregados de Google, Bing, DuckDuckGo, etc.

**Pros:** completamente gratuito sin límites, sin key, código open source.  
**Contras:** instancias públicas son inestables (rate limiting, caídas). Para uso serio requiere auto-hospedar.

---

## Comparativa

| API | Gratis | Key | Contenido completo | Ideal para agentes |
|---|---|---|---|---|
| **Tavily** | 1.000/mes | Sí | ✅ | ✅ |
| **Brave** | 2.000/mes | Sí | ❌ (snippet) | Parcial |
| **Google CSE** | 100/día | Sí | ❌ (snippet) | Parcial |
| **DuckDuckGo IA** | Ilimitado | No | ❌ | ❌ |
| **SearXNG self-hosted** | Ilimitado | No | ❌ (snippet) | Parcial |

## Decisión

**Se usa Tavily.**

La API key se lee de la variable de entorno `TAVILY_API_KEY`. La implementación está en `Infrastructure/ToolChains/WebSearchToolChain.cs`.

```bash
# Windows
set TAVILY_API_KEY=tvly-...

# Linux / macOS
export TAVILY_API_KEY=tvly-...
```