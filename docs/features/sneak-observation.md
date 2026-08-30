# Sneak / Observation System

Mission build-order item 2 (see `mission-design.md`). Real stealth, not a proximity check: NPCs get a facing-based vision cone and a continuously decaying awareness meter driving an Unaware → Suspicious → Alert state machine. Built as generic, content-driven modules per the Reusability Principle — none of these scripts know about this mission's specific NPCs or clues.

---

## AlertLevel.cs
**Path**: `Scripts/Stealth/AlertLevel.cs`

Enum: `Unaware`, `Suspicious`, `Alert`.

---

## IConcealmentProvider.cs
**Path**: `Scripts/Stealth/IConcealmentProvider.cs`

```csharp
public interface IConcealmentProvider { float RiseRateMultiplier { get; } }
```
Decouples `Stealth/` from any Player-specific type. `AlertStateMachine` looks for this interface on its target via `GetComponent`, rather than referencing `PlayerConcealment` directly — a future mission's watcher could point at any target, concealed or not.

---

## VisionSensor.cs
**Path**: `Scripts/Stealth/VisionSensor.cs`
**Attach to**: Any NPC that should be able to spot the player (or another target)

| Field | Type | Notes |
|---|---|---|
| `eye` | Transform | Raycast/cone origin; falls back to the NPC's own transform if unset |
| `viewAngle` | float | Default 100° (total cone width) |
| `viewDistance` | float | Default 12 |
| `targetHeightOffset` | float | Default 0.5 — raises the line-of-sight check point above the target's pivot (chest height for the player capsule, whose pivot is at its vertical center) |
| `obstructionMask` | LayerMask | Default Everything — no differentiated physics layers exist in the project yet |

**Behaviour**: `CanSee(Transform target)` — pure detection, no state. Checks distance, then cone angle, then line-of-sight via `Physics.Raycast`. **Gotcha hit during implementation**: the raycast target point sits *inside* the target's own collider (chest height, not the surface), so a naive "did anything block the ray" check always reports blocked — the ray hits the target's own collider surface short of the actual point. Fixed by checking whether the raycast hit belongs to the target itself (`hit.transform == target || hit.transform.IsChildOf(target)`), in which case LOS is clear — only a *different* object blocks the view.

---

## AlertStateMachine.cs
**Path**: `Scripts/Stealth/AlertStateMachine.cs`
**Attach to**: The same NPC as `VisionSensor`

| Field | Type | Notes |
|---|---|---|
| `visionSensor` | VisionSensor | The sensor to poll |
| `target` | Transform | Left empty, auto-resolves to the `Player`-tagged object in `Awake` |
| `riseRate` | float | Default 0.35/sec |
| `decayRate` | float | Default 0.2/sec |
| `suspiciousThreshold` | float | Default 0.34 (on a 0–1 awareness scale; `Alert` is fixed at 1.0) |

| Public member | Notes |
|---|---|
| `CurrentState` | `AlertLevel`, polled every frame by `SneakObservationPoint` |
| `Awareness01` | 0–1 meter, clamped |

**Behaviour**: every `Update`, awareness rises while `visionSensor.CanSee(target)` (modulated by the target's `IConcealmentProvider.RiseRateMultiplier` if it has one) and decays uniformly whenever not seen — the decay itself *is* the "cooldown" back to `Unaware`, no separate timer.

---

## SneakObservationPoint.cs
**Path**: `Scripts/Stealth/SneakObservationPoint.cs`
**Attach to**: An empty GameObject with a trigger Collider — follows `InteractableZone`'s trigger/`CompareTag("Player")` convention

| Field | Type | Notes |
|---|---|---|
| `target` | AlertStateMachine | The watcher whose state gates progress |
| `clueId` | string | Granted via `ClueTracker.Instance.AddClue` on completion |
| `requiredDuration` | float | Default 4 — continuous seconds required |
| `completionMessage` | string | Shown via `InteractionPromptUI.ShowMessage` on success |

**Behaviour**: while the player is inside and `target.CurrentState != Alert`, progress accumulates each frame and shows live via `InteractionPromptUI.Show("Observing... N%")` — reuses the existing prompt UI, no new UI built. Reaching `Alert` resets progress to 0 immediately (the "evidence attempt just fails" design). Reaching `requiredDuration` grants the clue once (`completed` guard prevents re-granting).

---

## PlayerConcealment.cs
**Path**: `Scripts/Player/PlayerConcealment.cs`
**Attach to**: The player Capsule

| Field | Type | Notes |
|---|---|---|
| `crouchSpeedMultiplier` | float | Default 0.5 |
| `crouchRiseRateMultiplier` | float | Default 0.4 — how much slower NPC awareness rises while crouched |

Toggle bound to Left Ctrl / gamepad East button, via an in-code `InputAction` (matches `PlayerMovement`'s pattern — no `.inputactions` asset). Implements `IConcealmentProvider`.

**`PlayerMovement.cs` change**: reads a sibling `PlayerConcealment` (optional — `null`-safe, falls back to 1×) and multiplies move speed by its `SpeedMultiplier`.

---

## Scene Wiring — Hub_Zone01
The original test rig (`Watcher_Suspect`/`SneakObservationPoint_Test`) was renamed and repurposed into real Act 1 content once `mission-state.md` landed — same components, same recipe, now representing the fiancée. A second instance (`Watcher_Lover`/`SneakObservationPoint_Lover`) was added identically for the lover. See `mission-state.md` for the current object names, positions, and how they're gated behind the mission's `act1_case_accepted` flag.

```
Watcher_Fiancee                       ← NPC (capsule primitive) — one of two identical watcher instances
├── Transform: position (8, 1, -10), rotation (0, 180, 0) — forward = -Z, faces south toward spawn
├── CapsuleCollider (Height 2, Radius 0.5)
├── VisionSensor (Eye → Eye child, View Angle 100, View Distance 12, Target Height Offset 0.5)
├── AlertStateMachine (Vision Sensor → self, Target empty/auto-resolve, Rise Rate 0.35, Decay Rate 0.2, Suspicious Threshold 0.34)
├── Eye                              ← child empty, local position (0, 0.6, 0.55), LOCAL ROTATION IDENTITY
│                                       (see gotcha below — this is not the default when parented under a 180°-rotated object)
└── NameLabel (world-space TMP)

SneakObservationPoint_Fiancee         ← empty GameObject, trigger zone
├── Transform: position (8, 1, -15)   ← 5 units south of Watcher_Fiancee, on its cone centerline
├── SphereCollider (Is Trigger: ON, Radius: 3)
└── SneakObservationPoint (Target → Watcher_Fiancee's AlertStateMachine, Clue Id "evidence_fiancee_seen", Required Duration 4)

Capsule (existing Player)
└── + PlayerConcealment (defaults)
```

**Gotcha — child rotation under a yaw-rotated parent**: creating `Eye` as a child of a watcher (which has world rotation `(0,180,0)`) with no explicit rotation parameter left its **world** rotation at identity by default — meaning its `forward` pointed the opposite way from the capsule it was supposed to represent. Fixed by explicitly setting `eye.transform.localRotation = Quaternion.identity` so it inherits the parent's world rotation instead. Worth remembering for any future child object placed under a rotated parent via the GameObject-creation tooling — this recurred a second time when `Watcher_Lover` was built and was caught immediately since it was already documented here.

---

## Design Decisions
- **Awareness decay as the sole "cooldown"**: rather than a separate timer after reaching `Alert`, the same continuous decay that drives `Suspicious → Unaware` also drives `Alert → Unaware` — one mechanism for all state transitions, verified live (forced `Alert`, confirmed decay back down over real time).
- **`IConcealmentProvider` interface over a direct `PlayerConcealment` reference**: keeps `Stealth/` fully player-agnostic — `AlertStateMachine.target` doesn't have to be the player, and doesn't need to have concealment at all (`concealment?.RiseRateMultiplier ?? 1f` degrades gracefully).
- **Progress gates on `Alert` only, not `Suspicious`**: a glimpse (`Suspicious`) is tense but not fatal to an observation attempt — only full detection (`Alert`) cancels it. Verified live: an observation completed successfully while the watcher sat in `Suspicious` (crouched, in cone) for the full duration.
