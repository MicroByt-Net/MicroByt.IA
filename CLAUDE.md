# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

# Convenciones del Proyecto

## Git

* Al iniciar una sesión, si hay cambios sin commitear, indícalo y ofrece usar `/commit` para generar un mensaje.
* Sigue siempre el formato **Conventional Commits**: `tipo(ámbito): descripción`
* Commits atómicos: un commit = un propósito claro
* No hagas `git add .` automáticamente; el usuario decide qué stagear
* No añadas footer de autoría de Claude Code a los commits

## Slash commands disponibles

* `/commit` — Analiza los cambios staged y sugiere un mensaje de commit

## Flujo recomendado
```bash
git add <archivos>   # el usuario stagea lo que quiere
/commit              # Claude sugiere el mensaje y confirma antes de ejecutar
```

## Project Overview

MicroByt.IA es una implementación mini de OpenClaw en .NET, ejecutada como aplicación de consola. Reproduce el núcleo de un agente conversacional con soporte de skills, tools y selección dinámica de capacidades mediante LLMs. El objetivo es explorar arquitecturas de agentes IA en .NET de forma práctica y ligera. La solución se encuentra en `src/MicroByt.IA.slnx`.

## Build & Run Commands

Once C# projects are added under `src/`, standard .NET CLI commands apply:
```bash
# Build
dotnet build src/MicroByt.IA.slnx

# Run tests
dotnet test src/MicroByt.IA.slnx

# Run a single test (by filter)
dotnet test src/MicroByt.IA.slnx --filter "FullyQualifiedName~TestClassName.TestMethodName"

# Run a specific project
dotnet run --project src/<ProjectName>/<ProjectName>.csproj
```

## Repository Structure

```
src/
  MicroByt.IA/                         ← proyecto único (consola)
    Domain/
      AI/                              ← AIModel, Provider
      AgentSkills/                     ← Skill, Tool
    Application/
      Interfaces/                      ← ISkillsService, ISkillsAgentService, ...
      Models/                          ← SkillFileCacheEntry, SkillsAgentInput
      Services/                        ← SkillsService, SkillsAgentService, ...
      DependencyInjection.cs           ← AddMicrobytIA()
    Infrastructure/
      Api/OpenAI/                      ← CompletionsRequest, CompletionsResponse
      Exceptions/                      ← DeserializeJsonIAException
      Helpers/                         ← JsonHelper
      Services/                        ← FileSkillCacheService, ProviderChatClientFactory, ...
    API/
      Samples/                         ← SelectSkillsSample
    Program.cs
    MicroByt.IA.csproj
  MicroByt.IA.slnx
docs/
```

## Stack

- Language: C# / .NET
- IDE: Visual Studio (`.slnx` solution format)
- License: Apache 2.0

## Principios de diseño

Este proyecto sigue **Clean Architecture + DDD** y los principios **SOLID**:

- **Domain/** — objetos de dominio puros (entidades, value objects). Sin dependencias externas.
- **Application/** — interfaces, modelos y servicios de casos de uso. Depende solo de Domain.
- **Infrastructure/** — implementaciones concretas (I/O, APIs externas, caché). Depende de Application.
- **API/** — punto de entrada, samples, controladores. Depende de Application e Infrastructure.

### SOLID
- **S** — Una clase, una responsabilidad
- **O** — Abierto a extensión, cerrado a modificación
- **L** — Las implementaciones deben poder sustituir a sus interfaces sin romper el comportamiento
- **I** — Interfaces pequeñas y específicas, no genéricas
- **D** — Depender de abstracciones (interfaces), nunca de implementaciones concretas

## Convenciones de la capa de aplicación

### Servicios

Cada vez que se cree un servicio se deben seguir estos tres pasos obligatoriamente:

**1. Crear la interfaz** en `Application/Interfaces/` con el prefijo `I`:
```csharp
// Application/Interfaces/IMyService.cs
namespace MicroByt.IA.Application.Interfaces;

public interface IMyService
{
    // métodos públicos del servicio
}
```

**2. Implementar la interfaz** en la capa correspondiente:
- Lógica de caso de uso → `Application/Services/`
- Acceso a ficheros, APIs externas, caché → `Infrastructure/Services/`

```csharp
// Application/Services/MyService.cs  (o Infrastructure/Services/)
namespace MicroByt.IA.Application.Services;

public class MyService : IMyService
{
    // implementación
}
```

**3. Registrar el servicio** en `Application/DependencyInjection.cs`, dentro de `AddMicrobytIA()`:
```csharp
services.AddScoped<IMyService, MyService>();
```

> Nunca dejes un servicio sin su interfaz ni sin su registro en `AddMicrobytIA()`.
