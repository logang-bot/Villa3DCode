# Workflow & Session Log

## How to Resume Work
1. Open **Unity Hub** → open project `Villa3DCode`
2. Open scene: `Assets/_Project/Scenes/Hub/Hub_Zone01.unity`
3. Read `docs/project-overview.md` for current milestone status
4. Read this file for where the last session left off

---

## Environment
- **Unity version**: 6000.0.41f1 (Unity 6)
- **Render pipeline**: URP (Universal Render Pipeline)
- **Project path**: `R:\Development\Unity\Villa3DCode`

---

## What Has Been Built

### PlayerMovement.cs
**Path**: `Assets/_Project/Scripts/Player/PlayerMovement.cs`

Handles all player locomotion. Key details:
- Uses `CharacterController` (not Rigidbody)
- Uses the **new Input System** (`UnityEngine.InputSystem`) — bindings defined in code, no InputActionAsset file needed
- Supports WASD, arrow keys, and gamepad left stick
- Movement is **camera-relative**: the player moves in the direction the camera is facing
- Character smoothly rotates to face the movement direction (`Quaternion.Slerp`)
- Gravity is applied manually (`gravity = -20f`)
- Exposes `moveSpeed`, `rotationSpeed`, `gravity` as serialized fields (tunable in Inspector)

**To attach**: Add to a GameObject that also has a `CharacterController` component. Requires a camera tagged `MainCamera` in the scene.

### Hub_Zone01 Scene
**Path**: `Assets/_Project/Scenes/Hub/Hub_Zone01.unity`

Current contents:
- **Plane** at position `(0, 0, 0)` — the floor
- **Capsule** at position `(0, 1, 0)` — placeholder player character
  - Has `CharacterController` component
  - Has `PlayerMovement` script attached
- **Main Camera** (default, tagged `MainCamera`)
- **Directional Light** (default)

---

## Where We Left Off
- Player movement is working: capsule walks on the plane, rotates toward movement direction
- **Next task**: Set up Cinemachine follow camera so the camera tracks the player smoothly

---

## Next Steps (in order)
1. **Cinemachine camera** — third-person follow cam using Cinemachine Brain + Virtual Camera
2. **Interactable zones** — trigger colliders at points of interest that fire an event
3. **Scene transitions** — fade out/in when moving between zones
4. **Replace capsule** — import an actual anime-style 3D character (VRM format recommended)

---

## Key Decisions Made
- **Unity over Unreal**: Chosen for better anime/toon shader support, lighter iteration, and a larger anime asset ecosystem
- **URP over HDRP**: HDRP is built for photorealism; URP is the right pipeline for cel-shading
- **New Input System**: Installed and used (not the legacy `Input` class) — all future input code should use `UnityEngine.InputSystem`
- **CharacterController over Rigidbody**: Standard choice for character movement in this genre; gives direct control without physics interference
