# Skybound: Guardianes del Viento

**A 2D platformer adventure set on floating islands above a sea of clouds**

[![Unity](https://img.shields.io/badge/Unity-6000.4.1f1-black?logo=unity)](https://unity.com)
[![C#](https://img.shields.io/badge/Language-C%23-239120?logo=csharp)](https://learn.microsoft.com/en-us/dotnet/csharp/)
[![Version](https://img.shields.io/badge/Version-v2.0-green)](https://github.com/imdani30/skybound-guardianes-del-viento/releases)

**Universidad Nacional Abierta y a Distancia — UNAD**  
Game Programming | Course Code: 213027A  
Tutor: Cesar David Monroy Rodriguez | 2026  
Student: Daniel Andres Castro Silva

---

## About the Game

**Skybound: Guardianes del Viento** is a 2D adventure platformer developed in Unity 6 with C#. A mysterious archipelago of floating islands is held in balance by energy crystals. When the crystals disappear, the islands begin to fall. You play as **Aero**, an agile explorer who must recover all the crystals and reach the portal at the end of each level.

---

## Characters

| Character | Description |
|-----------|-------------|
| **Aero** | Main protagonist. Agile explorer with blue jacket, white scarf, and aviator goggles |
| **Neblins** | Flying enemy creatures. Patrol islands and chase Aero when detected |

---

## Controls

| Key | Action |
|-----|--------|
| `A` / `D` | Move left / right |
| `Space` | Jump |
| `Space` (in air) | Double jump |
| `Space` (hold while falling) | Glide |
| `Left Shift` | Wind dash |
| `Esc` | Pause / Resume |

---

## Levels

| Level | Theme | Crystals | Enemies | Difficulty |
|-------|-------|----------|---------|------------|
| **Isla del Amanecer** | Green islands, dawn sky | 8 | 3 Neblins | Easy |
| **Bosque de Nubes** | Misty blue-green forest | 10 | 4 Neblins | Medium |
| **Torre del Viento** | Dark stone tower | 12 | 5 Neblins | Hard |

---

## How to Run

1. Clone this repository
2. Open with Unity Hub — Unity 6 (6000.4.1f1)
3. Open scene: `Assets/Scenes/IslaDelAmanecer.unity`
4. Press Play

---

## Project Structure

```
Assets/
├── Scripts/
│   ├── Core/        GM.cs  AM.cs  CF.cs
│   ├── Player/      PC.cs  VD.cs  CP.cs  PP.cs  BM.cs
│   ├── Enemy/       NB.cs
│   ├── Collectibles/ CR.cs  CM.cs
│   ├── Environment/ MP.cs  WZ.cs  PT.cs
│   ├── UI/          HUD.cs  MM.cs
│   └── Builder/     SceneBuilder.cs  LevelBuilder.cs
├── Sprites/
├── Audio/
└── Scenes/
```

---

## Game Mechanics Implemented

- Player movement, single jump, double jump, glide, wind dash
- Enemy AI (Patrol / Chase states) with OverlapCircle detection
- Crystal collection system with event-driven counter
- Portal activation when all crystals collected → loads next level
- HUD: lives (hearts), crystal counter, dash energy bar
- Pause system (Esc) freezing Time.timeScale
- Level boundary with respawn on exit
- Three complete levels with increasing difficulty
- Sound effects: jump, dash, hurt, collect, portal, game over

---

## Scripts Reference

| Script | Class | Responsibility |
|--------|-------|---------------|
| GM.cs | GM | GameManager Singleton — lives, crystals, pause, scene flow |
| AM.cs | AM | AudioManager — all SFX and background music |
| PC.cs | PC | PlayerController — movement, double jump, glide, dash |
| NB.cs | NB | NeblinAI — Patrol/Chase state machine |
| CR.cs | CR | Crystal — animation + collection trigger |
| PT.cs | PT | Portal — activates on all crystals, loads next scene |
| HUD.cs | HUD | UI — hearts, crystal counter, dash bar, panels |
| BM.cs | BM | BoundaryManager — level perimeter with respawn |

---

## Team

| Name | Role |
|------|------|
| Daniel Andres Castro Silva | Lead Programmer / Architect / Level Design / UI / QA |

---

## References

- Unity Technologies. (2024). *Unity 6 Documentation.* https://docs.unity3d.com
- Unity Technologies. (2024). *Input System Package.* https://docs.unity3d.com/Packages/com.unity.inputsystem
- Maddy Makes Games. (2018). *Celeste.* Maddy Makes Games.
- Team Cherry. (2017). *Hollow Knight.* Team Cherry.

---

*Made with Unity 6 | UNAD — Game Programming 213027A | 2026*
