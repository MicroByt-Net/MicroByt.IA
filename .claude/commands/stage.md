---
description: "Muestra los ficheros unstaged y permite seleccionar cuáles añadir al stage"
allowed-tools:
  - "Bash(git status:*)"
  - "Bash(git diff:*)"
  - "Bash(git add:*)"
---

# Comando: /stage

Muestra los ficheros con cambios fuera del stage y ayuda al usuario a añadir los que quiera.

## Proceso

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

7. Confirma con `git status --short` el resultado final

## Reglas

- Nunca añadir ficheros al stage sin confirmación explícita del usuario
- Nunca usar `git add .` ni `git add -A`
- Si no hay ficheros unstaged ni untracked, indicarlo y terminar
- Si un fichero está eliminado (`D`), añadirlo igualmente con `git add` para que el borrado quede staged