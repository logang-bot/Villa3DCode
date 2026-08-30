# Interaction System

## InteractableZone.cs
**Path**: `Scripts/Core/InteractableZone.cs`
**Attach to**: Any world GameObject with a **Collider** set to **Is Trigger**

| Field | Type | Notes |
|---|---|---|
| `promptText` | string | Text shown in the UI prompt when player enters |
| `targetScene` | string | If set, loads this scene on E press (use this for scene transitions) |
| `interactMessage` | string | Flavor/lore text shown via `InteractionPromptUI.ShowMessage` on E press, when `targetScene` is empty. Empty by default — no message shown |
| `messageDuration` | float | Default 4 — seconds `interactMessage` stays up before auto-hiding |
| `onInteract` | UnityEvent | Always fires on E press when `targetScene` is empty (alongside `interactMessage`, if set) — use for dialogue, quest flags, local triggers |

**Behaviour**:
- Requires the player to be tagged `Player`
- On trigger enter → calls `InteractionPromptUI.Show(promptText)`
- On trigger exit → calls `InteractionPromptUI.Hide()`
- On E press inside zone:
  - `targetScene` set → `SceneManager.LoadScene(targetScene)` (instant, no fade — the previous fade-transition system was removed; a cleaner replacement is deferred, see `roadmap.md`)
  - `targetScene` empty → if `interactMessage` is non-empty, `InteractionPromptUI.ShowMessage(interactMessage, messageDuration)` fires first, then `onInteract.Invoke()` always fires

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
- `InteractionPromptUI.Show(text)` — sets label and activates panel (hover prompt; cancels any pending auto-hide)
- `InteractionPromptUI.Hide()` — deactivates panel (cancels any pending auto-hide)
- `InteractionPromptUI.ShowMessage(text, duration)` — sets label, activates panel, and starts a coroutine that hides the panel again after `duration` seconds (used for E-press flavor text; superseded by a `Show`/`Hide` call or a new `ShowMessage` call in the meantime)
- Called directly by `InteractableZone`; no manual wiring needed beyond Inspector fields

---

## Scene Wiring — Hub_Zone01
```
Zone_Entrance                    ← test/scene-reload zone, relocated off the plaza
├── Transform: position (0, 0, 25)   ← open ground between the main plaza and Building_North
├── SphereCollider
│   ├── Is Trigger: ON
│   └── Radius: 2
└── InteractableZone
    ├── Prompt Text: "Press E to interact"
    └── Target Scene: "Hub_Zone01"   ← set to real scene name when ready

Zone_Cathedral                   ← first real onInteract landmark zone
├── Transform: position (0, 0, -27)   ← in front of the cathedral's door-facing side (verified via MeshRenderer bounds, not the group root's Transform.position)
├── SphereCollider (Is Trigger: ON, Radius: 5)
└── InteractableZone
    ├── Prompt Text: "Press E to investigate the cathedral"
    ├── Target Scene: (empty)
    ├── Interact Message: "The cathedral looms over the plaza, its doors long sealed."
    └── Message Duration: 4

Zone_GovPalace                   ← landmark zone at the Moneda (government palace)
├── Transform: position (56, 0, -48)   ← in front of the palace's north-facing apron/balcony feature
├── SphereCollider (Is Trigger: ON, Radius: 6)
└── InteractableZone
    ├── Prompt Text: "Press E to investigate the Moneda"
    ├── Target Scene: (empty)
    ├── Interact Message: "Even by night, lantern-light seeps from the Moneda's shuttered windows."
    └── Message Duration: 4

Zone_Statue                      ← landmark zone at the plaza's central monument
├── Transform: position (0, 0, 0)   ← Statue_Central's own position, no bounds correction needed
├── SphereCollider (Is Trigger: ON, Radius: 3)
└── InteractableZone
    ├── Prompt Text: "Press E to investigate the statue"
    ├── Target Scene: (empty)
    ├── Interact Message: "Weather has worn the statue's face to a blank, watching stare."
    └── Message Duration: 4

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

All three landmark zones are scene-root objects (not children of `Plaza_District`), so they use plain world coordinates and don't need the 180°-yaw position compensation that `Plaza_District`'s children require.

---

## Design Decisions
- **`interactMessage` + `onInteract` kept separate but both fire**: `interactMessage` covers the immediate need (flavor text with no dialogue system yet) via a reused `InteractionPromptUI` path; `onInteract` still always invokes so a future quest-flag/dialogue listener can hook in later without touching `InteractableZone` again.
- **Auto-hide via a coroutine on the `InteractionPromptUI` singleton, not per-zone**: keeps the timer logic in one place and lets `Show`/`Hide` cancel a stale pending auto-hide (e.g. walking out of a zone right after pressing E, or moving straight into a different zone).
