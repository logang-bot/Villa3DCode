# Interaction System

## InteractableZone.cs
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
  - `targetScene` set → `SceneManager.LoadScene(targetScene)` (instant, no fade — the previous fade-transition system was removed; a cleaner replacement is deferred, see `roadmap.md`)
  - `targetScene` empty → `onInteract.Invoke()`

---

## InteractionPromptUI.cs
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

## Scene Wiring — Hub_Zone01
```
Zone_Entrance                    ← example interactable zone
├── Transform: position (3, 0, 3)
├── SphereCollider
│   ├── Is Trigger: ON
│   └── Radius: 2
└── InteractableZone
    ├── Prompt Text: "Press E to interact"
    └── Target Scene: "Hub_Zone01"   ← set to real scene name when ready

Canvas                           ← interaction prompt UI
├── Render Mode: Screen Space - Overlay
└── InteractionPrompt            (child Panel)
    ├── Anchor: bottom-center
    ├── Size: ~400 × 60
    ├── InteractionPromptUI
    │   ├── Panel → InteractionPrompt (self)
    │   └── Label → PromptLabel
    └── PromptLabel (TextMeshPro)
        ├── Text: "Press E to interact"
        └── Alignment: center, font size ~24
```
