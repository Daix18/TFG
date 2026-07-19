# Generación Dinámica de Horror en Videojuegos
### Comparativa entre Sistemas Adaptativos y No Adaptativos

**Trabajo de Fin de Grado** · Grado en Diseño y Desarrollo de Videojuegos y Entornos Virtuales · Especialidad de Programación  
UDIT · Curso 2025/2026 · Daniel Izaguirre Montalvo

---

## Descripción

Investigación experimental que compara tres técnicas de inducción dinámica del terror en videojuegos, implementadas en un prototipo desarrollado en Unity 6. El objetivo es analizar cuál de los tres enfoques genera una experiencia emocional más efectiva, sin necesidad de hardware fisiológico especializado.

## Sistemas implementados

| Sistema | Descripción |
|---|---|
| **Baseline** | Sistema no adaptativo. Eventos fijos activados al cruzar zonas predefinidas del escenario. Sirve como condición de referencia. |
| **Dynamic Horror Generation (DHG)** | Director probabilístico que evalúa cada 2 segundos si generar un evento. La probabilidad crece con el tiempo: `P = Pbase + (t × growthFactor)`. Incluye cooldown configurable. |
| **Affective Loop** | Sistema adaptativo basado en el comportamiento del jugador. Calcula el nivel de *arousal* a partir del movimiento de cámara y adapta la intensidad de los eventos en consecuencia. |

## Arquitectura

```
Assets/Scripts/
├── Managers/
│   ├── TechniqueManager.cs     # Selección aleatoria de técnica al inicio de sesión
│   ├── GameManager.cs          # Control de tiempo y sesión
│   ├── AudioManager.cs         # Gestión de audio
│   ├── LightManager.cs         # Control de iluminación dinámica
│   └── DataLogger.cs           # Registro y exportación de datos en CSV
├── Systems/
│   ├── BaseLineSystem.cs       # Condición de referencia
│   ├── DynamicHorrorSystem.cs  # Sistema DHG
│   └── AffectiveLoopSystem.cs  # Sistema Affective Loop
└── Events/
    ├── JumpScare.cs
    └── LightEvent.cs
```

## Recogida de datos

El prototipo registra automáticamente los datos de cada sesión en formato CSV (5 muestras/segundo, independiente del framerate):

- Tiempo transcurrido
- Rotación de cámara (X, Y)
- Delta de movimiento de cámara
- Nivel de arousal
- Tipo de evento activado
- Técnica activa

El archivo se exporta al escritorio al finalizar la sesión con nombre identificativo de técnica y fecha.

## Tecnologías

- **Motor:** Unity 6000.1.11f1
- **Pipeline:** Universal Render Pipeline (URP)
- **Lenguaje:** C#
- **Plataforma:** PC Windows (x64)
- **Documentación:** Doxygen + GraphViz

## Requisitos para abrir el proyecto

1. Instalar Unity Hub y Unity versión **6000.1.11f1**
2. Clonar el repositorio
3. Abrir la carpeta del proyecto desde Unity Hub
4. Unity generará la carpeta `Library` automáticamente en el primer arranque (puede tardar varios minutos)

No se requiere configuración adicional.

## Resultados

El experimento se realizó con 6 participantes (between-subjects). Resultados subjetivos en escala Likert 1–5:

| Sistema | Miedo percibido | Sorpresa | Escalada percibida |
|---|---|---|---|
| Baseline | 4/5 | 5/5 | 4/5 |
| DHG | 4/5 | 3.5/5 | 3/5 |
| Affective Loop | 3.67/5 | 4/5 | 2.67/5 |

El Affective Loop generó experiencias muy distintas entre jugadores activos (hasta 7 jumpscares) y tranquilos (solo eventos leves), confirmando la adaptabilidad del sistema.

## Autor

**Daniel Izaguirre Montalvo**  
[GitHub](https://github.com/Daix18)
