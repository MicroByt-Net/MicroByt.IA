---
description: "Hace git push de la rama actual, gestionando el upstream si no existe"
allowed-tools:
  - "Bash(git status:*)"
  - "Bash(git branch:*)"
  - "Bash(git push:*)"
  - "Bash(git log:*)"
  - "Bash(git remote:*)"
---

# Comando: /push

Hace push de la rama actual al remoto, con confirmación previa y gestión del upstream.

## Proceso

1. Ejecuta `git branch -vv` para obtener la rama actual y si tiene upstream configurado
2. Ejecuta `git remote -v` para conocer los remotos disponibles
3. Ejecuta `git log @{u}..HEAD --oneline` (o `git log HEAD --oneline -5` si no hay upstream) para mostrar los commits pendientes de subir

4. Muestra un resumen antes de actuar:
   ```
   Rama:    feature/mi-rama
   Remoto:  origin → https://github.com/...
   Commits pendientes:
     abc1234 feat(api): add new endpoint
     def5678 fix(auth): correct token expiry
   ```

5. **Si la rama NO tiene upstream configurado:**
   - Indica que se usará `git push -u origin <rama>` para establecerlo
   - Pide confirmación explícita antes de continuar

6. **Si la rama YA tiene upstream:**
   - Muestra el upstream actual (p.ej. `origin/feature/mi-rama`)
   - Pide confirmación antes de ejecutar `git push`

7. Si el usuario confirma, ejecuta el push correspondiente

8. Muestra el resultado del push (rama remota actualizada, URL, etc.)

## Reglas

- Nunca hacer push sin confirmación explícita del usuario
- Nunca usar `--force` ni `--force-with-lease` salvo que el usuario lo pida explícitamente
- Si no hay commits pendientes de subir, indicarlo y no ejecutar push
- Si la rama actual es `main` o `master`, advertir explícitamente antes de pedir confirmación
- Usar siempre `origin` como remoto por defecto salvo que el usuario indique otro