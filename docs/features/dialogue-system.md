# Dialogue & Clue Tracking System

Foundation for mission build-order item 1 (see `mission-design.md`). Built as generic, content-driven modules per the Reusability Principle — none of these scripts know anything about this specific mission's NPCs or clues.

**Package**: Yarn Spinner for Unity, installed via the OpenUPM scoped registry (`https://package.openupm.com`, scope `dev.yarnspinner.unity`). Resolved version: **3.2.8**.

---

## ClueTracker.cs
**Path**: `Scripts/Dialogue/ClueTracker.cs`
**Attach to**: A dedicated `ClueTracker` GameObject, scene root

The project's first persistent (`DontDestroyOnLoad`) singleton. Mission-agnostic — no Yarn or UI references.

| Member | Notes |
|---|---|
| `Instance` (static) | The live singleton, set in `Awake` |
| `ClueAdded` (static event, `Action<string>`) | Fires when a new clue ID is added |
| `AddClue(string id)` | No-ops if already present; fires `ClueAdded` otherwise |
| `HasClue(string id)` | Query |

**Behaviour**: `Awake` destroys any duplicate instance (guards against `Zone_Entrance`'s self-reload of `Hub_Zone01` spawning a second tracker) and calls `DontDestroyOnLoad`. Verified in Play mode: exactly one instance survives a scene reload, and `HasClue` still returns correctly afterward.

---

## ClueYarnBridge.cs
**Path**: `Scripts/Dialogue/ClueYarnBridge.cs`

The only file referencing both `Yarn.Unity` and `ClueTracker`. A static class (no `MonoBehaviour`) — Yarn Spinner's `[YarnCommand]`/`[YarnFunction]` auto-discovery picked up both members correctly with zero manual registration, confirmed in Play mode (no "unknown command" errors when `<<add_clue "...">>` ran).

| Yarn-facing name | C# member |
|---|---|
| `<<add_clue "id">>` | `AddClue(string id)` → `ClueTracker.Instance?.AddClue(id)` |
| `has_clue("id")` | `HasClue(string id)` → `ClueTracker.Instance.HasClue(id)` |

This is the only place `.yarn` content touches C# — everything else is data.

---

## NpcDialogueTrigger.cs
**Path**: `Scripts/Dialogue/NpcDialogueTrigger.cs`
**Attach to**: Any NPC GameObject, alongside an `InteractableZone`

| Field | Type | Notes |
|---|---|---|
| `dialogueRunner` | `DialogueRunner` | The scene's `DialogueRunner` |
| `startNode` | string | Yarn node name to start |

`BeginDialogue()` (public) guards on `dialogueRunner.IsDialogueRunning` and calls `dialogueRunner.StartDialogue(startNode)` (fire-and-forget — `StartDialogue` returns a `YarnTask` as of Yarn Spinner 3.1+, not awaited here). Wired to `InteractableZone.onInteract` via the Inspector — `InteractableZone` itself is untouched and stays dialogue-agnostic. A second NPC is just a second GameObject with a different `startNode`; no new code.

---

## Debug/ClueDebugListUI.cs
**Path**: `Scripts/Dialogue/Debug/ClueDebugListUI.cs`
**Attach to**: `ClueDebugPanel` (Canvas child)

Explicit placeholder (folder + name both signal it) — subscribes to `ClueTracker.ClueAdded` and lists collected clue IDs in a TMP label, so the system's state is visible without opening the console. **Known limitation**: only reflects clues added after it starts listening in the current scene load; doesn't backfill from `ClueTracker` on a fresh scene. A future real journal UI will need `ClueTracker` to expose an enumerable snapshot too.

---

## Content — `Assets/_Project/Dialogue/`
Kept separate from `Scripts/Dialogue/` so content stays data, not code.

- `MissionDialogue.yarnproject` — the Yarn project asset. **Gotcha**: `baseLanguage` is a required JSON field (`[JsonRequired]` in `Yarn.Compiler.Project`) alongside `projectFileVersion`; omitting it fails silently as "invalid JSON" with no other diagnostic. Minimal working file:
  ```json
  {
    "projectFileVersion": 4,
    "baseLanguage": "en",
    "sourceFiles": ["**/*.yarn"]
  }
  ```
- `Informant.yarn` (renamed from `Witness.yarn` when its NPC was repurposed for Act 1 — see `mission-state.md`) — node `Informant_Start`, exercising `add_clue`/`has_clue` with a three-way branch (brush-off before `act1_case_accepted`, real leads once accepted, "already told you" after `lead_informant_tip` is granted).
- `Client.yarn` — node `Client_Start`, the Act 1 mission-giver: a four-way `<<if>>/<<elseif>>/<<else>>` chain gating on `act1_complete`, then a conjunction of two evidence clues (`has_clue(...) && has_clue(...)`), then `act1_case_accepted`, then the initial brief. First use of `<<elseif>>` and `&&` in this project's content — both compiled and ran cleanly.

---

## Scene Wiring — Hub_Zone01
```
Dialogue System                  ← scaffolded via GameObject → Yarn Spinner → Dialogue System
├── DialogueRunner
│   ├── Yarn Project → MissionDialogue.yarnproject
│   ├── Auto Start: OFF (started explicitly via NpcDialogueTrigger)
│   └── Variable Storage → InMemoryVariableStorage (same GameObject)
├── Canvas                       ← separate from the scene's main Canvas
│   ├── Line Presenter
│   └── Options Presenter
├── Markup Processors
├── Line Advancer
└── EventSystem                  ← auto-disabled; scene's existing EventSystem is used instead

ClueTracker                      ← scene-root, DontDestroyOnLoad
└── ClueTracker

NPC_Informant (renamed from NPC_Witness)  ← placeholder NPC (capsule primitive), NameLabel "Renata"
├── Transform: position (-8, 1, -10) — open plaza pavement, clear of tree colliders and existing zones
├── SphereCollider (Is Trigger: ON, Radius: 2.5)
├── InteractableZone
│   ├── Prompt Text: "Press E to talk"
│   └── On Interact → NpcDialogueTrigger.BeginDialogue
└── NpcDialogueTrigger
    ├── Dialogue Runner → Dialogue System
    └── Start Node: "Informant_Start"

NPC_Client                       ← Act 1's mission-giver, same recipe, see mission-state.md

Canvas                            ← existing scene Canvas
└── ClueDebugPanel                ← top-left, small always-visible list
    ├── ClueDebugListUI
    │   └── List Label → ClueListLabel
    └── ClueListLabel (TextMeshProUGUI)
```

---

## Gotcha — `LineAdvancer.RequestNextLine()` and MCP testing
Calling `RequestNextLine()` multiple times in a row *within a single `execute_code` call* only registers as one advance — Unity's Update loop doesn't tick between synchronous calls inside the same C# execution, so rapid repeated requests all land in the same "frame" and only the first is meaningful. Each real advance needs its own `execute_code` round-trip (the round-trip's wall-clock gap is what lets Unity's loop actually run a frame). When testing multi-line dialogue via MCP, expect roughly one line of progress per tool call, not per `RequestNextLine()` call.

---

## Design Decisions
- **Static-class Yarn command/function bridge over a `MonoBehaviour`**: confirmed via Play-mode testing that Yarn Spinner's `[YarnCommand]`/`[YarnFunction]` auto-discovery works identically for a bare static class as for an instance method — no scene placement or manual `AddCommandHandler` registration needed. Future clue/state bridges for other missions should follow the same pattern unless a concrete need for instance state arises.
- **`ClueTracker` as the project's first `DontDestroyOnLoad` singleton**: verified with a real scene-reload test (via `Zone_Entrance`) rather than trusting the pattern blindly — exactly one instance survives, and `HasClue` round-trips correctly afterward.
- **Yarn Spinner's built-in Line/Options Presenters used as-is** for this foundation pass, rather than custom dialogue UI — pragmatic for a "bare-bones" milestone; visual styling can come later without touching the underlying wiring.
