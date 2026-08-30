# Combat System

Mission build-order item 3 (see `mission-design.md`) — a Persona-style turn-based battle scene: stats, attack/defend/skill actions, Speed-based turn order, win/lose flow, return to hub. Built as generic, content-driven modules per the Reusability Principle — enemies are data assets, not code, so the future final-boss fight (item 6) reuses this system unchanged.

---

## Data model — `Assets/_Project/Scripts/Combat/`

**`CombatantState.cs`** — plain C# class, not a `MonoBehaviour`. The live runtime data (`HP`, `Attack`, `Defense`, `Speed`, `Resource`, `IsDefending`) for one combatant. Both `EnemyDefinition` and `PlayerCombatant` produce one via `CreateState()`, so the turn manager and UI never care whether a combatant came from a data asset or a scene component.

**`EnemyDefinition.cs`** — `ScriptableObject` (`[CreateAssetMenu(menuName = "Combat/Enemy Definition")]`). Fields: `displayName`, `maxHP`, `attack`, `defense`, `speed`, `maxResource`. A new enemy — including a future final boss — is a new asset under `Assets/_Project/Combat/Enemies/`, never new code.

**`PlayerCombatant.cs`** — `MonoBehaviour` on the Battle scene's player object. Same stat fields as `EnemyDefinition`, since there's only ever one player (not authored as a data asset the same way).

---

## Combat logic

**`BattleActionResolver.cs`** — all combat-math tuning is serialized (`minimumDamage`, `skillDamageMultiplier`, `skillResourceCost`, `defendDamageMultiplier`), nothing hardcoded.
- `ResolveAttack`: `damage = max(minimumDamage, attacker.Attack - defender.Defense)`, halved (rounded up) if `defender.IsDefending`.
- `TryResolveSkill`: same formula against `attacker.Attack * skillDamageMultiplier` (rounded), fails (returns `false`, no state change) if `attacker.Resource < skillResourceCost`.
- `ResolveDefend`: sets `IsDefending = true` — halves *all* incoming damage until the defender's own next turn begins (standard Persona Guard), cleared automatically when their turn comes back around.

**`BattleStateMachine.cs`** — the turn loop. `BattlePhase`: `Setup → AwaitingPlayerAction ⇄ AwaitingTarget → Won`/`Lost`. Turn order is built each round via `OrderByDescending(Speed)` over all living combatants (LINQ's stable sort keeps ties in insertion order). Enemy turns run a trivial "always attack the player" AI. Public `Phase`/`Player`/`Enemies` properties are polled by the UI every frame (same style as `AlertStateMachine`); `event Action<string> OnLog` feeds the battle log.

**Setup runs in `Start()`, not `Awake()`** — deliberate, so `BattleLogUI`'s `OnEnable` subscription is guaranteed to be attached before the first log line fires (Unity runs all `Awake`s across a scene before any `Start()`, but `OnEnable` order relative to other objects' `Awake` isn't guaranteed).

---

## Hub → Battle handoff

**`PendingEncounter.cs`** — a **plain static class**, not a `DontDestroyOnLoad` singleton like `ClueTracker`. Deliberate deviation: this is a one-shot, one-way pass (set once right before `SceneManager.LoadScene`, read once in the next scene's `Awake`/`Start`), so a static class survives the scene load for free with no GameObject or lifecycle question. Tradeoff: not visible in the Hierarchy the way `ClueTracker` is, and relies on domain-reload-on-play (the project default) to avoid stale data between Play sessions.

**`CombatEncounterTrigger.cs`** — the fully generic hub-side trigger: `[SerializeField] List<EnemyDefinition> enemies`, wired to `InteractableZone.onInteract` exactly like `NpcDialogueTrigger`. `StartEncounter()` calls `PendingEncounter.Set(enemies)` then loads `Battle`. Any future encounter is a new `InteractableZone` + this component with a different enemy list — zero new code.

**`EnemySpawner.cs`** — reads `PendingEncounter.Enemies ?? fallbackEncounter` (the fallback is what makes `Battle.unity` playable standalone, entered directly with no hub trigger), spawns a placeholder capsule per enemy at a `spawnPoints` transform, and calls `PendingEncounter.Clear()` immediately after — so a Battle reload after a loss doesn't reuse stale handoff data.

---

## UI

Plain uGUI, not code-built `InputAction`s — a battle action menu is a discrete on-screen choice list, and `Button`s get mouse/keyboard/gamepad navigation for free via the same `InputSystemUIInputModule` pattern the project already uses in `Hub_Zone01`.

- **`BattleUIController.cs`** — polls `BattleStateMachine` every frame; shows player HP/SP, enables Attack/Defend/Skill only in `AwaitingPlayerAction`, rebuilds the enemy target list (from the `EnemyRow` prefab) only in `AwaitingTarget`.
- **`BattleLogUI.cs`** — subscribes to `OnLog`, keeps the last 6 lines — same spirit as `ClueDebugListUI.cs`.
- **`BattleResultUI.cs`** — shows "Victory!"/"Defeat..." then returns to `Hub_Zone01` after a short delay, win or lose.

---

## Scene Wiring — `Battle.unity`
```
Directional Light                 ← default, matches Hub
Main Camera (+AudioListener)      ← static, no Cinemachine, (0,4,-9) looking toward the spawn area
EventSystem                       ← InputSystemUIInputModule (new scene has none by default)
Canvas                            ← Screen Space - Overlay
├── PlayerPanel (HP/SP text)
├── EnemyListPanel (Vertical Layout Group, runtime EnemyRow instances)
├── ActionMenu (Button_Attack / Button_Defend / Button_Skill)
├── BattleLogPanel (scrollback text)
└── ResultPanel (hidden by default)
PlayerCombatant                   ← capsule placeholder, (0,1,3), + PlayerCombatant
EnemySpawnPoints                  ← 3 empty Transforms (sized for a future boss+adds, not just 1 enemy)
BattleSystems                     ← BattleStateMachine, EnemySpawner, BattleActionResolver,
                                     BattleUIController, BattleLogUI, BattleResultUI (all on one GameObject)
```
`Prefabs/Combat/EnemyPlaceholder.prefab` (capsule + world-space NameLabel) and `Prefabs/Combat/EnemyRow.prefab` (Button + TMP label) — same placeholder-art style as every other placeholder NPC in the project.

`Battle` is registered in Build Settings (index 1, after `Hub_Zone01`).

## Scene Wiring — Hub_Zone01
Originally built as a standalone test trigger (`CombatEncounterTrigger_Test`); renamed to `CombatEncounterTrigger_Thief` and folded into Act 1 once `mission-state.md` landed (gated behind `act1_case_accepted`, same as the mission's other content) — mechanically unchanged.
```
CombatEncounterTrigger_Thief
├── Transform: position (-8, 1, -15) — open plaza pavement, clear of other zones/NPCs
├── SphereCollider (Is Trigger: ON, Radius: 2.5)
├── InteractableZone (Prompt Text "Press E to confront the thief", On Interact → CombatEncounterTrigger.StartEncounter)
└── CombatEncounterTrigger (Enemies: [Thief.asset])
```

## Data assets
`Assets/_Project/Combat/Enemies/Thief.asset` — `displayName "Thief"`, `maxHP 15`, `attack 4`, `defense 1`, `speed 7` (faster than the player's 5, deliberately, to make turn-order sorting non-trivially testable). Content, not code — mirrors `Dialogue/Client.yarn`/`Informant.yarn`.

---

## Gotcha — Play mode entered right after scene edits
Twice this build (once in `Battle.unity`, once in `Hub_Zone01`), pressing Play immediately after a burst of scene/asset creation produced broken runtime state on the *first* Play — `BattleStateMachine.Enemies`/`Player` stayed `null` despite `Start()` clearly having run (an enemy was spawned), and separately `InteractionPromptUI`'s static `instance` stayed `null` (its own `Awake` apparently didn't fire in time). Both times, exiting Play, waiting for `mcpforunity://editor/state` to report `activity.phase: "idle"`, then re-entering Play fixed it completely — no code change needed. Root cause looks like a stale/still-settling domain reload racing the Play button when Unity hasn't finished importing/GUID-resolving assets created moments earlier. **Always confirm the editor is idle before entering Play mode right after creating new scenes/prefabs/assets.**

---

## Design Decisions
- **`EnemyDefinition` as a `ScriptableObject`, player stats as plain fields on `PlayerCombatant`**: enemies are data-authored (reusable across missions/bosses); the player isn't, since there's only ever one.
- **`CombatantState` as a plain class, not a `MonoBehaviour`**: keeps the turn manager and UI decoupled from whether a combatant is asset-sourced (enemy) or component-sourced (player).
- **Skill resource is a flat SP pool, not a use-once flag**: generalizes to a boss with several differently-costed skills later without a redesign.
- **`PendingEncounter` as a plain static class, not a `DontDestroyOnLoad` singleton**: see Hub → Battle handoff above — deliberate deviation from `ClueTracker`'s pattern, justified by the one-shot/one-way nature of the data.
- **Verified via direct state manipulation, not real-time waits**: every check (turn order, damage math, resource gating, win/lose transitions) was driven by directly reading/forcing `CombatantState`/`BattlePhase` and polling `SceneManager.GetActiveScene().name`, never a fixed sleep — matches this project's established Play-mode verification approach and avoided the flakiness fixed-timing would have introduced.
