---
description: "Ejecuta /stage y /commit en secuencia para stagear ficheros y crear un commit en un solo flujo"
allowed-tools:
  - "Bash(git status:*)"
  - "Bash(git diff:*)"
  - "Bash(git add:*)"
  - "Bash(git commit:*)"
  - "Bash(git log:*)"
---

# Comando: /ship

Ejecuta el flujo completo de staging y commit en una sola sesión interactiva.

## Proceso

### Fase 1 — Stage (equivalente a `/stage`)

1. Ejecuta `git status --short` para obtener el estado de cada fichero
2. Separa los ficheros en dos grupos:
   - **Unstaged** — modificados o eliminados pero no en stage (columna derecha del status)
   - **Untracked** — ficheros nuevos sin seguimiento (`??`)
3. Muestra los dos grupos con índice numerado, por ejemplo:

   ```
   Unstaged (modificados):
     1. src/Foo/Bar.cs
     2. src/Foo/Baz.cs

   Sin seguimiento (nuevos):
     3. src/Foo/NewFile.cs
   ```

4. Pregunta al usuario qué ficheros quiere stagear:
   - Puede indicar números sueltos: `1 3`
   - Puede indicar rangos: `1-3`
   - Puede escribir `all` para stagear todos
   - Puede escribir `none` o pulsar Enter para cancelar sin hacer nada

5. Muestra la lista de ficheros seleccionados y pide confirmación antes de ejecutar `git add`

6. Ejecuta `git add <ficheros seleccionados>` (nunca `git add .` ni `git add -A`)

7. Confirma con `git status --short` el resultado tras el staging

### Fase 2 — Commit (equivalente a `/commit`)

8. Ejecuta `git diff --cached` para revisar los cambios staged
9. Revisa `git log --oneline -5` para seguir las convenciones del proyecto
10. Genera 3 candidatos de mensaje de commit siguiendo Conventional Commits
11. Selecciona el más apropiado y explica brevemente el motivo
12. Pregunta al usuario si desea proceder con ese mensaje o modificarlo
13. Si confirma, ejecuta el commit

## Formato del mensaje de commit

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

- Nunca añadir ficheros al stage sin confirmación explícita del usuario
- Nunca usar `git add .` ni `git add -A`
- Si no hay ficheros unstaged ni untracked al inicio, saltar directamente a la Fase 2
- Si tras el staging no hay nada en stage, indicarlo y terminar sin crear commit
- Modo imperativo en el mensaje: "add" no "added", "fix" no "fixed"
- Primera línea ≤ 72 caracteres
- Sin punto final en el título
- NO añadir footer de Claude Code
- Idioma del mensaje: igual al del proyecto (detectar por historial)
