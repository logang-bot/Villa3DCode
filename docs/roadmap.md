# Roadmap & Status

## Milestones

### Phase 1 — Navigation (Current)
- [x] Player movement script (CharacterController + new Input System)
- [x] First hub scene with floor plane and capsule placeholder character
- [x] Follow camera (Cinemachine — orbital, mouse-controlled)
- [x] Interactable points of interest (trigger zones + E prompt)
- [ ] Scene transition system — removed (fade/DontDestroyOnLoad version made the Editor view messy); scenes load instantly for now, cleaner replacement deferred
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
Phase 1 navigation is complete, and `Hub_Zone01`'s entire Potosí city site is now built from real models imported from `VillaCity.blend` — all primitive blockout has been retired and the full import is done:
- WASD moves the capsule camera-relative
- Mouse orbits the camera around the character independently
- Walking into a trigger zone shows a prompt; pressing E triggers the action
- Pressing E on a zone with a `targetScene` loads it instantly via `SceneManager.LoadScene` — the old fade-transition system (`TransitionCanvas`/`SceneTransition.cs`) was removed since its always-opaque overlay made the Editor Scene/Game views unusable while working; a cleaner transition system is deferred to later, see `features/interaction-system.md`
- All 19 landmark groups (~115 mesh objects) are imported and placed: the plaza, cathedral, government palace ("Moneda"), the secondary LPlaza square + connecting terrace/staircase, the site ground (two terraces), 12 lamp posts with real Point Lights, and 9 surrounding city blocks — see `features/hub-environment.md` and the `hub-zone01-coordinate-mapping` memory for the full layout and the coordinate-mapping gotchas hit along the way
- Player spawn remains at the plaza's south edge (0, 1, -14), which reads correctly against the real geometry

**Next task**: Seed interaction zones at the plaza landmarks (cathedral, government palace, fountain/statue) — first real use of the `onInteract` UnityEvent path.

---

## Next Steps (in order)
The full city site is in; remaining art work is texture/material passes and any missing entrance/facing detail (deferred — see `features/hub-environment.md` known limitations). Mechanics work resumes now that the world exists to build them against.

1. **Seed interaction zones at plaza landmarks** — place an `InteractableZone` at each real landmark with placeholder prompt text; first real use of the `onInteract` UnityEvent path (currently only `targetScene` is exercised) — see `features/interaction-system.md`. Also relocate the existing `Zone_Entrance` test zone out of the plaza/statue area.
2. **Bake a NavMesh** over the real city geometry — `AI Navigation` is installed but unused; bake now so future NPCs can path the site immediately after art
3. **Multiple hub zones** — create `Hub_Zone02`+ scenes with their own layouts and zone triggers; can reuse the capsule placeholder. Add each new scene to Build Settings so `SceneManager.LoadScene` can find it by name.
4. **Redesign the scene transition system** — needed once multiple hub zones exist; the removed version's always-opaque overlay made the Editor unusable while working on a single scene, so the replacement should avoid that (e.g. only opaque during an actual transition, not sitting on top of the Editor view by default)
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
