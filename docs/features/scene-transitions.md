# Scene Transitions

## SceneTransition.cs
**Path**: `Scripts/Core/SceneTransition.cs`
**Attach to**: The `TransitionCanvas` root GameObject

| Inspector Field | Assign |
|---|---|
| `overlay` | The `CanvasGroup` on `TransitionCanvas` root |
| `fadeDuration` | 0.5 (seconds) |

**Behaviour**:
- Singleton with `DontDestroyOnLoad` — survives all scene loads
- On any scene load: fades in automatically (black → transparent) via `SceneManager.sceneLoaded`
- Starts fully opaque so every scene opens with a fade-in
- `SceneTransition.Load(sceneName)` — static, safe to call from anywhere
- Fades out (transparent → black), then calls `SceneManager.LoadScene`

---

## Scene Wiring — Hub_Zone01
```
TransitionCanvas                 ← scene transition overlay
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
4. Add a return trigger zone pointing back to `Hub_Zone01` (or whichever zone the player came from) — see `interaction-system.md`
5. The player character, camera, and other scene objects need to be re-created or saved as Prefabs first — see `player-movement.md` and `camera.md`

> **Prefab tip**: Before building multiple zones, consider turning the `Capsule` + `CM Camera` + `Main Camera` setup into Prefabs so they can be dropped into any scene without re-wiring.

---

## Design Decisions
- **SceneTransition as DontDestroyOnLoad singleton**: Fade-out coroutine must complete before the scene unloads; a scene-local object can't survive that
- **InteractableZone uses `SceneTransition.Load()` (static), not UnityEvent for scene loading**: UnityEvent stores an object reference — after reload the duplicate `TransitionCanvas` is destroyed, breaking the reference; a static call has nothing to break
