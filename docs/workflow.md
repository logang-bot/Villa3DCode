# Workflow & Session Log

## How to Resume Work
1. Open **Unity Hub** → open project `Villa3DCode`
2. Open scene: `Assets/_Project/Scenes/Hub/Hub_Zone01.unity`
3. Read `project-overview.md` for milestone status
4. Read this file for session state and next steps
5. Read `systems.md` for script details
6. Read `scene-setup.md` for Inspector/hierarchy wiring

---

## Environment
- **Unity version**: 6000.0.41f1 (Unity 6)
- **Render pipeline**: URP (Universal Render Pipeline)
- **Project path**: `R:\Development\Unity\Villa3DCode`

---

## Where We Left Off
Phase 1 navigation is complete. All systems are working:
- WASD moves the capsule camera-relative
- Mouse orbits the camera around the character independently
- Walking into a trigger zone shows a prompt; pressing E triggers the action
- Pressing E on a zone with a `targetScene` fades to black and loads the scene

**Next task**: Replace the capsule placeholder with an actual anime-style 3D character model.

---

## Next Steps (in order)
1. **Replace capsule** — import an anime-style 3D character (VRM or FBX); re-attach `PlayerMovement` and `CharacterController` to it
2. **Multiple hub zones** — create `Hub_Zone02`+ scenes with their own layouts and zone triggers
3. **Dialogue system** — Yarn Spinner or Ink integration for NPC conversations and story clues
4. **Quest/clue tracking** — data structure to track what the player has discovered

---

## Key Decisions Made
- **Unity over Unreal**: Better anime/toon shader support, lighter iteration, larger anime asset ecosystem
- **URP over HDRP**: HDRP is for photorealism; URP is correct for cel-shading
- **New Input System**: All input code uses `UnityEngine.InputSystem` — never the legacy `Input` class
- **CharacterController over Rigidbody**: Direct control without physics interference; standard for this genre
- **Cinemachine OrbitalFollow + RotationComposer**: Decouples camera orbit from character rotation; `ThirdPersonFollow` caused jitter and moved the camera on WASD because it is relative to the character's local space
- **Manual camera input (CameraController.cs) over CinemachineInputAxisController**: The auto-binding picked up keyboard input instead of mouse
- **SceneTransition as DontDestroyOnLoad singleton**: Fade-out coroutine must complete before the scene unloads; a scene-local object can't survive that
- **InteractableZone uses `SceneTransition.Load()` (static), not UnityEvent for scene loading**: UnityEvent stores an object reference — after reload the duplicate `TransitionCanvas` is destroyed, breaking the reference; a static call has nothing to break
