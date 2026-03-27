# Prompt: Selección de Skills para una Tarea (Respuesta en JSON)

Actúa como un planificador de tareas experto.  
Te proporcionaré dos elementos:
1. **La tarea del usuario**: una descripción de lo que el usuario necesita lograr.
2. **Los skills del sistema**: una lista de capacidades disponibles que pueden ser utilizadas para llevar a cabo diferentes acciones.

Tu objetivo es **analizar la tarea y seleccionar los skills más relevantes y necesarios** para completarla.

Debes responder **exclusivamente con un objeto JSON** válido, sin texto adicional fuera del JSON.

---

## Estructura del JSON de respuesta

```json
{
  "selected_skills": [
    {
      "skill": "nombre_del_skill",
      "justification": "explicación breve de por qué es necesario"
    }
  ],
  "sequence": [
    {
      "step": 1,
      "skill": "nombre_del_skill",
      "action": "qué se hace en este paso"
    }
  ],
  "observations": {
    "missing_skills": ["skill_faltante_1", "skill_faltante_2"],
    "additional_notes": "consideraciones adicionales o contexto relevante"
  }
}

## Datos de entrada

### Tarea del usuario
Busca en la web comparativas entre Unity IAP y RevenueCat y mándame un resumen claro con pros y contras.

### Skills del sistema

web-research: Investiga información en internet y prioriza fuentes útiles.
comparison-summary: Compara varias opciones y produce una síntesis con pros y contras.
delivery-summary: Empaqueta el resultado en un mensaje breve, claro y accionable.
local-file-work: Trabaja con archivos locales para leer o generar contenido.
general-reasoning: Skill genérica de razonamiento cuando no hay una skill especializada clara.