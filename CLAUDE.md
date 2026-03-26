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

MicroByt.IA is a .NET project aimed at building an OpenClaw implementation. The solution file is at `src/MicroByt.IA.slnx`.

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

- `src/` — all source code and the solution file (`MicroByt.IA.slnx`)
- `docs/` — documentation

New C# projects should be placed under `src/` and added to the solution.

## Stack

- Language: C# / .NET
- IDE: Visual Studio (`.slnx` solution format)
- License: Apache 2.0
