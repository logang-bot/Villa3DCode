# Roadmap & Status

## Milestones

### Phase 1 — Navigation (Current)
- [x] Player movement script (CharacterController + new Input System)
- [x] First hub scene with floor plane and capsule placeholder character
- [x] Follow camera (Cinemachine — orbital, mouse-controlled)
- [x] Interactable points of interest (trigger zones + E prompt)
- [x] Landmark interaction zones seeded (cathedral, government palace, statue) with `onInteract` flavor-text messages
- [ ] Scene transition system — removed (fade/DontDestroyOnLoad version made the Editor view messy); scenes load instantly for now, cleaner replacement deferred
- [ ] Replace capsule with actual character model
- [ ] Multiple hub zones (expand beyond Hub_Zone01)

### Phase 2 — World & Story
- [ ] Multiple hub zones
- [x] Dialogue system (Yarn Spinner) — foundation done, see `features/dialogue-system.md`
- [x] Quest/clue tracking system — foundation done (`ClueTracker`), see `features/dialogue-system.md`
- [x] NPC placement — Act 1's roster is placed (Client, informant, fiancée, lover); the killer/final-boss confrontation NPC is still ahead (item 6)

### Phase 3 — Combat
- [x] Turn-based battle scene — see `features/combat-system.md`
- [x] Player and enemy stats system — data-authored enemies (`EnemyDefinition`), player stats on `PlayerCombatant`
- [x] Attack, defend, skill actions
- [x] Win/lose conditions and scene return

### Phase 4 — Polish
- [ ] Toon shader tuning
- [ ] Audio (BGM, SFX, ambient)
- [ ] Main menu and save system
- [ ] UI/UX pass

---

## Where We Left Off
Phase 1 navigation is complete, `Hub_Zone01`'s entire Potosí city site is built from real imported models, and the interaction system now has its first real gameplay use — three landmark zones with `onInteract` flavor text:
- WASD moves the capsule camera-relative
- Mouse orbits the camera around the character independently
- Walking into a trigger zone shows a prompt; pressing E triggers the action
- Pressing E on a zone with a `targetScene` loads it instantly via `SceneManager.LoadScene` — the old fade-transition system (`TransitionCanvas`/`SceneTransition.cs`) was removed since its always-opaque overlay made the Editor Scene/Game views unusable while working; a cleaner transition system is deferred to later, see `features/interaction-system.md`
- All 19 landmark groups (~115 mesh objects) are imported and placed: the plaza, cathedral, government palace ("Moneda"), the secondary LPlaza square + connecting terrace/staircase, the site ground (two terraces), 12 lamp posts with real Point Lights, and 9 surrounding city blocks — see `features/hub-environment.md` and the `hub-zone01-coordinate-mapping` memory for the full layout and the coordinate-mapping gotchas hit along the way
- Player spawn remains at the plaza's south edge (0, 1, -14), which reads correctly against the real geometry
- `Zone_Cathedral`, `Zone_GovPalace`, and `Zone_Statue` are seeded at the real landmarks with placeholder flavor-text `interactMessage`s (first real use of the `onInteract` path — `InteractableZone`/`InteractionPromptUI` were extended with `interactMessage`/`ShowMessage` for this); the old `Zone_Entrance` test zone was relocated off the plaza to (0, 0, 25), clear of the new landmark zones — see `features/interaction-system.md`
- Runtime testing surfaced a real bug found along the way: none of the imported city geometry had colliders, so the player fell through the world everywhere. Fixed by adding non-convex `MeshCollider`s to all 115 imported mesh sub-objects across all 19 landmark groups — see `features/hub-environment.md`
- Mission build-order item 1 (dialogue + clue tracking foundation) is done: Yarn Spinner 3.2.8 installed, a persistent `ClueTracker` singleton (the project's first `DontDestroyOnLoad`), a static-class Yarn command/function bridge, and a placeholder NPC (`NPC_Witness`) with a working branching conversation in `Hub_Zone01` — verified end-to-end in Play mode, including the clue surviving a scene reload. See `features/dialogue-system.md` and `mission-design.md`.
- Mission build-order item 2 (sneak/observation mechanic) is done: NPCs get a `VisionSensor` (cone + line-of-sight) and an `AlertStateMachine` (Unaware/Suspicious/Alert, driven by a continuously decaying awareness meter), a timed `SneakObservationPoint` grants a clue if the player stays unseen long enough, and the player has a new crouch toggle (`PlayerConcealment`) that slows detection and movement — verified live against all six behavior cases including a real bug found and fixed (the line-of-sight raycast was always self-blocked by the target's own collider). See `features/sneak-observation.md`.
- Mission build-order item 3 (combat system core) is done: a separate `Battle` scene, Speed-based turn order, Attack/Defend/Skill actions, data-authored enemies (`EnemyDefinition` ScriptableObjects — future bosses are just new assets), and a win/lose flow back to the hub — verified live against all eight behavior cases. Found and worked around a real Play-mode timing gotcha (entering Play immediately after a burst of scene/asset creation can leave `Start()`-initialized state broken on the first Play — wait for `editor/state` to report idle first). See `features/combat-system.md`.
- Mission build-order item 4 (Act 1 wiring) is done: the full infidelity case is playable start to finish — a Client NPC (mission-giver), an informant (leads), two sneak targets (fiancée + lover, each granting distinct evidence), a thief encounter, and a report/completion flow, all assembled from items 1–3 with zero new mechanics. Also resolved the deferred "what do we do when the game starts" question: a new generic `RequiresClue` component gates world content on a `ClueTracker` flag (reusing the existing clue system rather than a new persistent store), so only the Client is visible at boot and everything else activates live once the case is accepted. Verified live including the real dialogue-driven "accept the case" path. See `features/mission-state.md`.

**Next task**: Continue the first mission's mechanics build order — see `mission-design.md`, item 5 (event sequencer + Act 2 reveal). This supersedes the "Next Steps" list below as the current priority. **Once the mission's build order is complete, the next step after it is baking a NavMesh** (item 1 in the list below) — don't let it get lost behind the mission work.

---

## Next Steps (in order)
Deprioritized behind the mission work above (`mission-design.md`) — still valid, just not the current focus.

The full city site is in; remaining art work is texture/material passes and any missing entrance/facing detail (deferred — see `features/hub-environment.md` known limitations). Mechanics work resumes now that the world exists to build them against.

1. **Bake a NavMesh** over the real city geometry — `AI Navigation` is installed but unused; bake now so future NPCs can path the site immediately after art
2. **Multiple hub zones** — create `Hub_Zone02`+ scenes with their own layouts and zone triggers; can reuse the capsule placeholder. Add each new scene to Build Settings so `SceneManager.LoadScene` can find it by name.
3. **Redesign the scene transition system** — needed once multiple hub zones exist; the removed version's always-opaque overlay made the Editor unusable while working on a single scene, so the replacement should avoid that (e.g. only opaque during an actual transition, not sitting on top of the Editor view by default)
4. **Combat system** — turn-based battle scene, player/enemy stats, attack/defend/skill actions, win/lose conditions; can prototype with placeholder shapes for enemies
5. **Replace capsule** — import a gothic-colonial-style 3D character (VRM or FBX); re-attach `PlayerMovement` and `CharacterController` to it (see `features/player-movement.md`)
6. **NPC placement** — also blocked on having NPC models
7. **Toon shader tuning**
8. **Audio** — BGM, SFX, ambient
9. **Main menu, save system, and UI/UX pass**

---

## Key Decisions Made
Project-wide decisions. System-specific decisions live with their feature doc under `features/`.

- **Unity over Unreal**: Better anime/toon shader support, lighter iteration, larger anime asset ecosystem
- **URP over HDRP**: HDRP is for photorealism; URP is correct for cel-shading
- **New Input System**: All input code uses `UnityEngine.InputSystem` — never the legacy `Input` class
