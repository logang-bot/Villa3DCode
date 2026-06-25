# Systems Reference

All scripts live under `Assets/_Project/Scripts/`.

---

## Player Systems

### PlayerMovement.cs
**Path**: `Scripts/Player/PlayerMovement.cs`  
**Attach to**: The player GameObject (also needs a `CharacterController` component)

| Field | Type | Default | Notes |
|---|---|---|---|
| `moveSpeed` | float | 4 | Units per second |
| `rotationSpeed` | float | 10 | Slerp speed toward movement direction |
| `gravity` | float | -20 | Applied manually each frame |

**Behaviour**:
- WASD / arrow keys / gamepad left stick for movement
- Movement is camera-relative (reads `Camera.main.transform`)
- Character rotates to face movement direction via `Quaternion.Slerp`
- Cursor locked on Awake; **Escape** unlocks it
- Input bindings defined in code — no InputActionAsset needed

---

### CameraController.cs
**Path**: `Scripts/Player/CameraController.cs`  
**Attach to**: The CM Camera GameObject (same object as `CinemachineCamera`)

| Field | Type | Default | Notes |
|---|---|---|---|
| `sensitivity` | float | 0.2 | Mouse delta multiplier |
| `verticalMin` | float | -20 | Minimum tilt angle |
| `verticalMax` | float | 60 | Maximum tilt angle |

**Behaviour**:
- Reads `Mouse.current.delta` every frame
- Drives `CinemachineOrbitalFollow.HorizontalAxis.Value` (pan) and `VerticalAxis.Value` (tilt)
- Only active while cursor is locked

---

## Core Systems

### InteractableZone.cs
**Path**: `Scripts/Core/InteractableZone.cs`  
**Attach to**: Any world GameObject with a **Collider** set to **Is Trigger**

| Field | Type | Notes |
|---|---|---|
| `promptText` | string | Text shown in the UI prompt when player enters |
| `targetScene` | string | If set, loads this scene on E press (use this for scene transitions) |
| `onInteract` | UnityEvent | Fired on E press when `targetScene` is empty (use for dialogue, local triggers) |

**Behaviour**:
- Requires the player to be tagged `Player`
- On trigger enter → calls `InteractionPromptUI.Show(promptText)`
- On trigger exit → calls `InteractionPromptUI.Hide()`
- On E press inside zone:
  - `targetScene` set → `SceneTransition.Load(targetScene)`
  - `targetScene` empty → `onInteract.Invoke()`

> **Do not** wire scene loads via `onInteract` UnityEvent — the object reference breaks after scene reload. Always use `targetScene` for scene loading.

---

### InteractionPromptUI.cs
**Path**: `Scripts/Core/InteractionPromptUI.cs`  
**Attach to**: The `InteractionPrompt` panel GameObject inside the UI Canvas

| Inspector Field | Assign |
|---|---|
| `panel` | The `InteractionPrompt` GameObject itself |
| `label` | The `PromptLabel` TextMeshPro child |

**Behaviour**:
- Singleton — one instance per scene
- Hides the panel on Awake
- `InteractionPromptUI.Show(text)` — sets label and activates panel
- `InteractionPromptUI.Hide()` — deactivates panel
- Called directly by `InteractableZone`; no manual wiring needed beyond Inspector fields

---

### SceneTransition.cs
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
