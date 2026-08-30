# Mission State & Act 1 Wiring

Mission build-order item 4 (see `mission-design.md`). Assembles items 1–3 (dialogue+clues, sneak/observation, combat) into the playable Act 1 loop, and answers a question deferred from earlier in the project: "how do we know what to do when the game starts?"

---

## The gating decision: reuse `ClueTracker`, no new persistent store

Milestone progress (`act1_case_accepted`, `act1_complete`) is stored as more clue IDs in the existing `ClueTracker` (`Scripts/Dialogue/ClueTracker.cs`) — not a new, dedicated `MissionState` system. `ClueTracker` already has everything a milestone flag needs: a presence query (`HasClue`), a live `ClueAdded` event, and bidirectional Yarn wiring. A second persistent singleton would duplicate that and is speculative complexity the content doesn't need yet (YAGNI) — worth introducing later, non-breakingly, only if a future mission needs ordered/branching progression that flag-presence can't express.

**Naming convention** (documentation only, no code enforces it): `act1_`-prefixed IDs for milestone/progression flags, `evidence_`/`lead_`-prefixed IDs for narrative content clues. Keeps the two kinds of state visually distinguishable in the debug clue list without needing separate storage.

---

## RequiresClue.cs — the one new reusable piece
**Path**: `Scripts/Core/RequiresClue.cs`
**Attach to**: A dedicated, always-active controller GameObject — never the GameObject(s) it toggles

| Field | Type | Notes |
|---|---|---|
| `clueId` | string | The flag that must be present |
| `targets` | GameObject[] | Everything to activate/deactivate together |

**Behaviour**: on `Start()` (guarantees `ClueTracker.Instance` already exists — same reasoning as `BattleStateMachine`'s `Start()`-not-`Awake()` choice in `combat-system.md`), and again live whenever `ClueTracker.ClueAdded` fires matching `clueId`, sets every target's `SetActive` to whether `ClueTracker.Instance.HasClue(clueId)` is true. Subscribes/unsubscribes in `OnEnable`/`OnDisable` (mirrors `ClueDebugListUI.cs`), which correctly detaches on scene reload since `ClueTracker` survives via `DontDestroyOnLoad` but `RequiresClue` instances don't.

**Critical rule — never attach `RequiresClue` to a GameObject it also gates.** An inactive GameObject never runs `Awake`/`OnEnable`/`Start` until something *external* reactivates it. If `RequiresClue` disabled its own object, it would kill its own ability to ever hear the unlocking event, and the object could never come back without a scene reload. This is why `Gate_Act1Content` below is its own empty, always-active object.

**Fail-closed convention**: every gated object's `Active` checkbox is authored `false` in the scene itself, so if `Start()` ever doesn't run (see the Play-mode timing gotcha in `combat-system.md`), content stays inert rather than leaking open.

Fully generic — verified via grep that no mission-specific string (character names, clue IDs) appears in the file.

---

## Division of responsibility
- **`RequiresClue` (world gating)** decides whether a GameObject *exists in the world at all* — used for the fiancée, the lover, and the thief encounter, none of which should be present before the case starts.
- **Yarn `<<if has_clue(...)>>` conditionals (dialogue gating)** decide *what an always-present NPC says* — used for the Client and the informant, both of whom stay visible throughout, only their dialogue content changes.

---

## Act 1 content

**The Client** (`NPC_Client`, new, always active) — `Assets/_Project/Dialogue/Client.yarn`, node `Client_Start`. A four-branch `<<if>>/<<elseif>>/<<else>>` chain:
1. `act1_complete` → case-closed line.
2. `evidence_fiancee_seen && evidence_lover_seen` → report available; selecting it grants `act1_complete`.
3. `act1_case_accepted` (alone) → idle "any word yet?" line.
4. Otherwise → the initial brief; accepting grants `act1_case_accepted`.

**The informant** (`NPC_Informant`, repurposed from `NPC_Witness`) — `Informant.yarn`, node `Informant_Start`. Three branches: brush-off before `act1_case_accepted`, real leads (grants `lead_informant_tip`, optional flavor only — never checked by the Client) once accepted, "already told you" after the tip is given.

**Two sneak targets** — `Watcher_Fiancee`/`SneakObservationPoint_Fiancee` (repurposed from `Watcher_Suspect`/`SneakObservationPoint_Test`, grants `evidence_fiancee_seen`) and `Watcher_Lover`/`SneakObservationPoint_Lover` (new, identical recipe, 8 units east, grants `evidence_lover_seen`). Both watcher *and* observation point are listed in `Gate_Act1Content`'s targets — gating only the watcher would leave its `AlertStateMachine.CurrentState` frozen at its enum-default `Unaware`, letting an active observation point grant the clue for free against an absent watcher.

**The thief encounter** (`CombatEncounterTrigger_Thief`, repurposed from `CombatEncounterTrigger_Test`) — unchanged mechanically. Defeating it grants nothing; sneaking (evidence) and combat (danger) stay parallel, not chained.

---

## Scene Wiring — Hub_Zone01
```
Gate_Act1Content                  ← new, scene-root, always active, no collider
└── RequiresClue
    ├── Clue Id: "act1_case_accepted"
    └── Targets: [Watcher_Fiancee, SneakObservationPoint_Fiancee,
                  Watcher_Lover, SneakObservationPoint_Lover,
                  CombatEncounterTrigger_Thief]   ← all 5 authored Active=false in-scene

NPC_Client                        ← new, always active
├── Transform: position (0, 1, -11)
├── SphereCollider (Is Trigger: ON, Radius: 2.5)
├── InteractableZone (Prompt "Press E to talk", On Interact → NpcDialogueTrigger.BeginDialogue)
├── NpcDialogueTrigger (Dialogue Runner → Dialogue System, Start Node "Client_Start")
└── NameLabel ("Client")

Watcher_Lover                     ← new, same recipe as Watcher_Fiancee (see sneak-observation.md)
├── Transform: position (16, 1, -10), rotation (0, 180, 0)
├── VisionSensor / AlertStateMachine (same defaults)
├── Eye (child, local position (0, 0.6, 0.55), LOCAL ROTATION IDENTITY — see the rotation gotcha in sneak-observation.md)
└── NameLabel ("Lover (Julian)")

SneakObservationPoint_Lover       ← new
├── Transform: position (16, 1, -15)
└── SneakObservationPoint (Target → Watcher_Lover's AlertStateMachine, Clue Id "evidence_lover_seen")
```

---

## Design Decisions
- **Reused `ClueTracker` instead of a dedicated `MissionState`** — see the gating decision above. The clearest sign this was right: adding Act 1's entire progression needed zero new persistent-state code, only one new *gating* component plus content.
- **A single shared `Gate_Act1Content` controller** for all five gated objects, rather than one `RequiresClue` per object — this build's content only ever needs one flag, so one controller is simplest; a future mission needing different subsets behind different flags would just add more controller objects, which doesn't reduce the component's generality.
- **Both real-dialogue-path and direct-injection testing used together**: the "Accept the case" flow was driven through actual Yarn dialogue (E-press → advance → select option) to prove the full wiring, while the single-vs-both-evidence-clue branch states were tested via direct `ClueTracker.AddClue` injection (since clues can't be un-granted, precise ordering for the elseif chain needed fresh Play sessions) — both are legitimate, complementary verification, not one substituting for the other.
