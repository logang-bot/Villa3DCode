# Project Overview

## Concept
A mystery, horror, and turn-based combat game in a gothic-colonial visual style. Zenless Zone Zero is the reference for movement and world-interaction mechanics; Persona is the reference for combat structure.

The player explores a hub-based map — not open world, not sandbox — with distinct locations to visit, each containing missions, quests, and story clues. Combat is turn-based.

---

## Engine & Stack
- **Engine**: Unity 6 (6000.0.41f1)
- **Render Pipeline**: Universal Render Pipeline (URP)
- **Language**: C#

## Installed Packages
- Cinemachine — camera follow and cutscene transitions
- Input System — keyboard and gamepad input
- AI Navigation — NavMesh pathfinding
- TextMeshPro — UI text for dialogue and menus

---

## Art Direction
- Gothic colonial architecture in the vein of Bloodborne, accented with neon — carried mainly through lighting rather than surface materials
- First hub location is modeled on the central square (Plaza) of Potosí — the real place is the source for the game's history and lore
- Hub-based map with multiple explorable zones
- Atmosphere: mystery and horror tone

---

## Project Structure
```
Assets/_Project/
├── Art/
│   ├── Characters/
│   ├── Environments/
│   └── UI/
├── Audio/
├── Prefabs/
├── Scenes/
│   ├── Hub/        ← walkable exploration map
│   └── Battle/     ← turn-based combat
├── Scripts/
│   ├── Player/
│   ├── Combat/
│   ├── Dialogue/
│   └── Core/
└── Settings/
```

---

## Where to Look Next
- **Milestone status, current stopping point, and next task** → `roadmap.md`
- **How each built system works (scripts + scene wiring)** → `features/`:
  - `features/player-movement.md`
  - `features/camera.md`
  - `features/interaction-system.md`
  - `features/hub-environment.md`

---

## How to Resume Work
1. Open **Unity Hub** → open project `Villa3DCode`
2. Open scene: `Assets/_Project/Scenes/Hub/Hub_Zone01.unity`
3. Read `roadmap.md` for milestone status, session state, and next steps
4. Read the relevant file(s) under `features/` for script and scene wiring details

---

## Environment
- **Unity version**: 6000.0.41f1 (Unity 6)
- **Render pipeline**: URP (Universal Render Pipeline)
- **Project path**: `R:\Development\Unity\Villa3DCode`
