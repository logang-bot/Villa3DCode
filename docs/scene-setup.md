# Scene Setup Reference

Inspector and hierarchy configurations for every scene. Use this to rebuild a scene from scratch or verify an existing one.

---

## Hub_Zone01
**Path**: `Assets/_Project/Scenes/Hub/Hub_Zone01.unity`

### Hierarchy & Components

```
Hub_Zone01
├── Directional Light
│
├── Plane
│   └── Transform: position (0, 0, 0)
│
├── Capsule                          ← placeholder player
│   ├── Transform: position (0, 1, 0)
│   ├── Tag: Player
│   ├── CharacterController          (default settings)
│   └── PlayerMovement
│       ├── Move Speed: 4
│       ├── Rotation Speed: 10
│       └── Gravity: -20
│
├── Main Camera
│   ├── Tag: MainCamera
│   └── CinemachineBrain             (default settings)
│
├── CM Camera
│   ├── CinemachineCamera
│   │   └── Tracking Target → Capsule
│   ├── CinemachineOrbitalFollow
│   │   ├── Orbit Style: Sphere
│   │   └── Radius: 5
│   ├── CinemachineRotationComposer  (default — auto-aims at tracking target)
│   └── CameraController
│       ├── Sensitivity: 0.2
│       ├── Vertical Min: -20
│       └── Vertical Max: 60
│
├── Zone_Entrance                    ← example interactable zone
│   ├── Transform: position (3, 0, 3)
│   ├── SphereCollider
│   │   ├── Is Trigger: ON
│   │   └── Radius: 2
│   └── InteractableZone
│       ├── Prompt Text: "Press E to interact"
│       └── Target Scene: "Hub_Zone01"   ← set to real scene name when ready
│
├── Canvas                           ← interaction prompt UI
│   ├── Render Mode: Screen Space - Overlay
│   └── InteractionPrompt            (child Panel)
│       ├── Anchor: bottom-center
│       ├── Size: ~400 × 60
│       ├── InteractionPromptUI
│       │   ├── Panel → InteractionPrompt (self)
│       │   └── Label → PromptLabel
│       └── PromptLabel (TextMeshPro)
│           ├── Text: "Press E to interact"
│           └── Alignment: center, font size ~24
│
└── TransitionCanvas                 ← scene transition overlay
    ├── Render Mode: Screen Space - Overlay
    ├── Sort Order: 100
    ├── CanvasGroup                  ← this is the overlay field on SceneTransition
    ├── SceneTransition
    │   ├── Overlay → CanvasGroup (on TransitionCanvas root)
    │   └── Fade Duration: 0.5
    └── Image (child)
        ├── Anchor: stretch full screen
        ├── Color: black (0, 0, 0, 255)
        └── Raycast Target: OFF
```

### Notes
- `TransitionCanvas` Sort Order 100 ensures it renders above all other UI
- `CanvasGroup` is on the **root** `TransitionCanvas`, not on the Image child — this controls alpha for the entire canvas
- The Image child is purely visual; the CanvasGroup is what gets animated
- `TransitionCanvas` persists via `DontDestroyOnLoad` — only one instance exists at runtime even as scenes change

---

## Adding a New Hub Zone

When creating `Hub_Zone02` or later zones:

1. **File → New Scene** → save to `Assets/_Project/Scenes/Hub/Hub_Zone0X.unity`
2. Add the scene to **Build Settings** (File → Build Settings → Add Open Scenes)
3. Copy `TransitionCanvas` from Hub_Zone01 into the new scene (the DontDestroyOnLoad check will destroy the duplicate at runtime, but it must exist in the scene for the first load)
4. Add a return trigger zone pointing back to `Hub_Zone01` (or whichever zone the player came from)
5. The player character, camera, and other scene objects need to be re-created or saved as Prefabs first

> **Prefab tip**: Before building multiple zones, consider turning the `Capsule` + `CM Camera` + `Main Camera` setup into Prefabs so they can be dropped into any scene without re-wiring.
