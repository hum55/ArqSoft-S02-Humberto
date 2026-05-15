# 🐍 Viborita — Juego Snake en Consola

<p align="center">
  <img src="docs/img/viborita_banner.png" alt="Viborita Banner" width="800"/>
</p>

---

## 📋 Descripción

**Viborita** es una implementación del clásico juego **Snake** desarrollado en C# para consola, con gráficos Unicode avanzados y una interfaz visual estilizada. El jugador controla una serpiente que debe comer para crecer y alcanzar 10 puntos sin chocar contra los muros o contra sí misma.

---

## 🎮 Gameplay

<p align="center">
  <img src="docs/img/viborita_gameplay_1.png" alt="Viborita Gameplay" width="600"/>
</p>

### Elementos del Juego

| Símbolo | Elemento | Descripción |
|---------|----------|-------------|
| `@` | Cabeza | Cabeza de la serpiente (color blanco) |
| `█` | Cuerpo | Segmentos del cuerpo (color verde) |
| `◆` | Comida | Alimento para crecer (color rojo) |
| `·` | Vacío | Espacio libre del tablero |

---

## 🕹️ Controles

| Tecla | Acción |
|-------|--------|
| `↑` | Mover arriba |
| `↓` | Mover abajo |
| `←` | Mover izquierda |
| `→` | Mover derecha |
| `Q` | Salir del juego |

---

## 🏗️ Arquitectura

```
Viborita/
├── IMotorViborita.cs         # Motor del juego (lógica principal)
│   ├── LinkedList<(int,int)>  → Cuerpo de la serpiente
│   ├── Avanzar()              → Movimiento por frame
│   ├── CambiarDireccion()     → Entrada del jugador
│   ├── GenerarComida()        → Posicionar alimento aleatorio
│   ├── Ganado()               → Victoria (10 puntos)
│   └── Perdido()              → Colisión
│
├── ConsolaUIViborita.cs      # Interfaz gráfica
│   ├── MostrarTablero()       → Renderizado con Unicode
│   ├── LeerTecla()            → Input no bloqueante
│   └── MostrarMensaje()       → Mensajes del juego
│
└── iMotorjuego.cs            # Interfaz base
    ├── Avanzar()
    ├── CambiarDireccion()
    ├── Ganado()
    └── Perdido()
```

---

## 📸 Capturas de Pantalla

### Inicio del Juego
<p align="center">
  <img src="docs/img/viborita_inicio.png" alt="Viborita Inicio" width="500"/>
</p>

### En Juego
<p align="center">
  <img src="docs/img/viborita_en_juego.png" alt="Viborita En Juego" width="500"/>
</p>

### Victoria
<p align="center">
  <img src="docs/img/viborita_victoria.png" alt="Viborita Victoria" width="500"/>
</p>

---

## 🎯 Características Técnicas

- **Estructura de datos:** `LinkedList<(int x, int y)>` para el cuerpo
- **Renderizado:** Caracteres Unicode (═, ╔, ╗, █, ◆)
- **Input:** Lectura no bloqueante con `Console.KeyAvailable`
- **Colisiones:** Detección contra muros y auto-colisión
- **Velocidad:** 150ms por frame (ajustable)

---

## 🔧 Principios de POO Aplicados

| Principio | Implementación |
|-----------|---------------|
| **Encapsulamiento** | Propiedades privadas con getters públicos |
| **Abstracción** | Interfaz `iMotorJuego` define el contrato |
| **Polimorfismo** | `MotorViborita` implementa `iMotorJuego` |
| **Separación de responsabilidades** | Motor (lógica) separado de UI (renderizado) |

---

## 👨‍💻 Autor

**Humberto Ramírez Gruintal**
Estudiante de Ingeniería en Software — Tecnológico de Software

---

## 🤖 Cláusula de Uso de Inteligencia Artificial
>
> Declaración de uso de herramientas de IA
>
> Yo, Humberto Ramírez Gruintal, estudiante de Ingeniería en Software del Tecnológico de Software, 
> declaro que en el desarrollo de este proyecto se utilizó inteligencia artificial (Claude, de Anthropic) 
> Para la Correcion de Errores y la mejora de interfaz grafica de sistema.
>
> Fecha: Mayo 2026 Institución: Tecnologico de Software

---

<p align="center">
  <i>Desarrollado con ❤️ por Humberto Ramírez Gruintal — Tec de Software, 2026</i>
</p>