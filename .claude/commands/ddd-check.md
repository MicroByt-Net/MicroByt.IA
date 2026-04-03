---
description: "Analiza el proyecto en busca de violaciones de Clean Architecture y DDD"
allowed-tools:
  - Read
  - Glob
  - Grep
---

# Comando: /ddd-check

Analiza todos los ficheros `.cs` del proyecto y detecta violaciones de las reglas de Clean Architecture y DDD definidas en este proyecto.

## Reglas de arquitectura a verificar

```
Domain      → sin dependencias externas, ni de Application ni de Infrastructure
Application → solo depende de Domain. Sin librerías de terceros (no using de NuGet)
Infrastructure → puede depender de Application y Domain. Librerías de terceros aquí
API         → puede depender de Application e Infrastructure
```

## Proceso

### 1. Recopilar ficheros por capa

Usa Glob para listar todos los `.cs` bajo cada carpeta:
- `src/MicroByt.IA/Domain/**/*.cs`
- `src/MicroByt.IA/Application/**/*.cs`
- `src/MicroByt.IA/Infrastructure/**/*.cs`
- `src/MicroByt.IA/API/**/*.cs`

### 2. Leer cada fichero y analizar

Para cada fichero comprueba:

**a) Namespace correcto**
El namespace debe coincidir con la carpeta. Ejemplos:
- `Domain/AI/AIModel.cs` → `namespace MicroByt.IA.Domain.AI`
- `Application/Services/Foo.cs` → `namespace MicroByt.IA.Application.Services`
- `Infrastructure/Services/Bar.cs` → `namespace MicroByt.IA.Infrastructure.Services`

**b) Dependencias cruzadas prohibidas**

| Fichero está en | No puede importar |
|---|---|
| `Domain/` | `Application`, `Infrastructure`, librerías NuGet |
| `Application/` | `Infrastructure`, librerías NuGet de terceros |
| `Infrastructure/` | — (puede ver todo) |
| `API/` | — (puede ver todo) |

Señales de alerta en los `using`:
- Fichero en `Application/` con `using` de `MicroByt.IA.Infrastructure.*` → VIOLACIÓN
- Fichero en `Application/` con `using` de librerías externas (OpenAI, YamlDotNet, EF Core, HttpClient, etc.) → VIOLACIÓN
- Fichero en `Domain/` con cualquier `using` externo → VIOLACIÓN

**c) Interfaces en la capa correcta**
- Interfaces para servicios sin dependencias externas → `Application/Interfaces/`
- Interfaces que exponen tipos de terceros en su firma → `Infrastructure/Interfaces/`

**d) Implementaciones en la capa correcta**
- Implementación usa librerías de terceros (`using` de NuGet) → debe estar en `Infrastructure/`
- Implementación solo usa tipos de Domain/Application → puede estar en `Application/Services/`

**e) Registro en DI**
- Todo servicio con interfaz debe estar registrado en `Infrastructure/DependencyInjection.cs`
- Comprueba que cada `IXxx` en `Application/Interfaces/` e `Infrastructure/Interfaces/` tiene su `services.AddXxx<IXxx, Xxx>()` correspondiente

**f) Anti-patrones de C#**
- Propiedades con `=> new()` que crean instancias en cada acceso → deben ser `{ get; } = new()`
- Parámetros de constructor primario usados directamente en métodos sin asignar a campo `readonly`

### 3. Presentar resultados

Agrupa los hallazgos por severidad:

**CRÍTICA** — viola las reglas de capas (dependencia prohibida, tipo externo en Application)
**ALTA** — anti-patrón de rendimiento o registro DI incompleto
**BAJA** — namespace incorrecto, convención de código

Para cada violación indica:
- Fichero y línea aproximada
- Descripción del problema
- Corrección recomendada

Si no hay violaciones, indícalo explícitamente: `✅ Sin violaciones detectadas`.

## Reglas del análisis

- Leer TODOS los ficheros `.cs` del proyecto principal (no los de Tests)
- No modificar ningún fichero, solo reportar
- Ignorar comentarios XML (`/// <...>`) al analizar `using`
- Los `using` de `System.*` y `Microsoft.Extensions.*` son aceptables en cualquier capa