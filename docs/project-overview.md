# Project Overview

## Concept
A mystery, horror, and turn-based combat game in a gothic-colonial visual style. Zenless Zone Zero is the reference for movement and world-interaction mechanics; Persona is the reference for combat structure.

The player explores a hub-based map — not open world, not sandbox — with distinct locations to visit, each containing missions, quests, and story clues. Combat is turn-based.

---

## Engine & Stack
- **Engine**: Unity 6 (6000.5.1f1)
- **Render Pipeline**: Universal Render Pipeline (URP)
- **Language**: C#

## Installed Packages
- Cinemachine — camera follow and cutscene transitions
- Input System — keyboard and gamepad input
- AI Navigation — NavMesh pathfinding (installed, not yet baked)
- TextMeshPro — UI text for dialogue and menus
- Yarn Spinner (`dev.yarnspinner.unity`, via OpenUPM) — NPC dialogue and branching conversations

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
├── Dialogue/       ← .yarn scripts + .yarnproject (content, not code)
├── Prefabs/
├── Scenes/
│   ├── Hub/        ← walkable exploration map
│   └── Battle/     ← turn-based combat
├── Scripts/
│   ├── Player/
│   ├── Combat/
│   ├── Dialogue/   ← ClueTracker, ClueYarnBridge, NpcDialogueTrigger
│   └── Core/
└── Settings/
```

---

## Where to Look Next
- **What the game currently does, in plain terms** → `game-mechanics.md`
- **The first full mission (story + mechanics build order)** → `mission-design.md`
- **Full step-by-step mission walkthrough (spoilers, built and planned steps both)** → `mission-walkthrough.md`
- **Milestone status, current stopping point, and next task** → `roadmap.md`
- **How each built system works (scripts + scene wiring)** → `features/`:
  - `features/player-movement.md`
  - `features/camera.md`
  - `features/interaction-system.md`
  - `features/hub-environment.md`
  - `features/dialogue-system.md`
  - `features/sneak-observation.md`
  - `features/combat-system.md`
  - `features/mission-state.md`

---

## How to Resume Work
1. Open **Unity Hub** → open project `Villa3DCode`
2. Open scene: `Assets/_Project/Scenes/Hub/Hub_Zone01.unity`
3. Read `roadmap.md` for milestone status, session state, and next steps
4. Read the relevant file(s) under `features/` for script and scene wiring details

---

## Environment
- **Unity version**: 6000.5.1f1 (Unity 6)
- **Render pipeline**: URP (Universal Render Pipeline)
- **Project path**: `R:\Development\Unity\Villa3DCode`
