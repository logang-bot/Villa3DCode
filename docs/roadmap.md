# Roadmap & Status

## Milestones

### Phase 1 — Navigation (Current)
- [x] Player movement script (CharacterController + new Input System)
- [x] First hub scene with floor plane and capsule placeholder character
- [x] Follow camera (Cinemachine — orbital, mouse-controlled)
- [x] Interactable points of interest (trigger zones + E prompt)
- [x] Scene transition system (fade to black, DontDestroyOnLoad)
- [ ] Replace capsule with actual character model
- [ ] Multiple hub zones (expand beyond Hub_Zone01)

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

---

## Where We Left Off
Phase 1 navigation is complete. All systems are working:
- WASD moves the capsule camera-relative
- Mouse orbits the camera around the character independently
- Walking into a trigger zone shows a prompt; pressing E triggers the action
- Pressing E on a zone with a `targetScene` fades to black and loads the scene

**Next task**: Greybox the Potosí plaza — replace Hub_Zone01's placeholder Plane with blockout volumes matching the real plaza layout.

---

## Next Steps (in order)
Mechanics and city layout first, using blockout geometry only — no real textures or Blender models yet. Art & sound last.

1. **Greybox the Potosí plaza** — replace `Hub_Zone01`'s placeholder Plane with blockout volumes (primitives or ProBuilder) matching the real plaza: the cathedral, the government palace, the fountain/monument, the surrounding streets
2. **Seed interaction zones at plaza landmarks** — place an `InteractableZone` at each blockout structure with placeholder prompt text; first real use of the `onInteract` UnityEvent path (currently only `targetScene` is exercised) — see `features/interaction-system.md`
3. **Bake a NavMesh over the greybox** — `AI Navigation` is installed but unused; bake now so future NPCs can path the plaza immediately after art
4. **Multiple hub zones** — create `Hub_Zone02`+ scenes with their own layouts and zone triggers; can reuse the capsule placeholder (see `features/scene-transitions.md` for the setup guide)
5. **Dialogue system** — Yarn Spinner or Ink integration for NPC conversations and story clues
6. **Quest/clue tracking** — data structure to track what the player has discovered
7. **Combat system** — turn-based battle scene, player/enemy stats, attack/defend/skill actions, win/lose conditions; can prototype with placeholder shapes for enemies
8. **Replace capsule** — import a gothic-colonial-style 3D character (VRM or FBX); re-attach `PlayerMovement` and `CharacterController` to it (see `features/player-movement.md`)
9. **NPC placement** — also blocked on having NPC models
10. **Toon shader tuning**
11. **Audio** — BGM, SFX, ambient
12. **Main menu, save system, and UI/UX pass**

---

## Key Decisions Made
Project-wide decisions. System-specific decisions live with their feature doc under `features/`.

- **Unity over Unreal**: Better anime/toon shader support, lighter iteration, larger anime asset ecosystem
- **URP over HDRP**: HDRP is for photorealism; URP is correct for cel-shading
- **New Input System**: All input code uses `UnityEngine.InputSystem` — never the legacy `Input` class
