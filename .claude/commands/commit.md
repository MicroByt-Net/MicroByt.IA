---
description: "Sugiere y ejecuta commits con mensajes descriptivos en formato Conventional Commits"
allowed-tools:
  - "Bash(git status:*)"
  - "Bash(git diff:*)"
  - "Bash(git add:*)"
  - "Bash(git commit:*)"
  - "Bash(git log:*)"
---

# Comando: /commit

Analiza los cambios pendientes y genera un mensaje de commit siguiendo Conventional Commits.

## Proceso

1. Ejecuta `git status` para ver qué archivos han cambiado
2. Ejecuta `git diff` (unstaged) y `git diff --cached` (staged)
3. Revisa `git log --oneline -5` para seguir las convenciones del proyecto
4. Genera 3 candidatos de mensaje de commit
5. Selecciona el más apropiado y explica brevemente el motivo
6. Pregunta al usuario si desea proceder con ese mensaje o modificarlo
7. Si confirma, ejecuta el commit (solo archivos ya staged; no hace `git add` automáticamente)

## Formato del mensaje

```
<tipo>(<ámbito opcional>): <descripción corta en imperativo>

<cuerpo opcional: qué cambió y por qué>
```

## Tipos permitidos

| Tipo       | Cuándo usarlo                          |
|------------|----------------------------------------|
| `feat`     | Nueva funcionalidad                    |
| `fix`      | Corrección de bug                      |
| `refactor` | Reestructuración sin cambio de lógica  |
| `docs`     | Solo documentación                     |
| `test`     | Tests nuevos o modificados             |
| `chore`    | Tareas de mantenimiento, deps, config  |
| `style`    | Formato, espacios (sin lógica)         |
| `perf`     | Mejoras de rendimiento                 |

## Reglas

- Modo imperativo: "add" no "added", "fix" no "fixed"
- Primera línea ≤ 72 caracteres
- Sin punto final en el título
- NO añadir footer de Claude Code
- Idioma del mensaje: igual al del proyecto (detectar por historial)
