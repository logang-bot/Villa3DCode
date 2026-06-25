# Project Overview

## Concept
A mystery, horror, and turn-based combat game with anime 3D aesthetics, inspired by Persona and Zenless Zone Zero.

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
- Anime-style 3D models (toon/cel-shading via URP)
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

## Milestones

### Phase 1 — Navigation (Current)
- [x] Player movement script (CharacterController + new Input System)
- [x] First hub scene with floor plane and capsule placeholder character
- [ ] Follow camera (Cinemachine)
- [ ] Interactable points of interest (trigger zones)
- [ ] Scene transition system

### Phase 2 — World & Story
- [ ] Multiple hub zones
- [ ] Dialogue system (Yarn Spinner or Ink)
- [ ] Quest/clue tracking system
- [ ] NPC placement

### Phase 3 — Combat
- [ ] Turn-based battle scene
- [ ] Player and enemy stats system
- [ ] Attack, defend, skill actions
- [ ] Win/lose conditions and scene return

### Phase 4 — Polish
- [ ] Toon shader tuning
- [ ] Audio (BGM, SFX, ambient)
- [ ] Main menu and save system
- [ ] UI/UX pass
