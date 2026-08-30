# First Mission Design

## Purpose
This is the first full vertical-slice mission for the game — a complete, playable case built **mechanics-first, with no art or atmosphere polish**. Its job is to prove that navigation, dialogue, investigation, sneaking, and turn-based combat all work together end to end, and to get something real in front of the player for feedback before investing further in content or visuals. Everything else on the roadmap (art passes, additional hub zones, further story content) waits behind this working.

This doc is the north star for that effort — the story and the build order don't need to be re-explained each session; start here. For the story turned into a concrete, playable step-by-step sequence (spoilers included, cross-referenced against the build order), see `mission-walkthrough.md`. For what the game does today in plain terms, see `game-mechanics.md`.

---

## Reusability Principle
**Every system built for this mission must be a generic, reusable module — never hardcoded to this specific case.** This mission is the *first* mission, not the *only* one: the dialogue runner, the clue/evidence tracker, the sneak/observation mechanic, the combat system, and the event sequencer all need to work for any future mission's content, not just this one's.

Concretely: this story's specific text, NPC names, clue data, and encounter placements are *content* — they should live in data (dialogue files, ScriptableObjects, scene wiring), not be baked into the mechanics' code. A future second mission should be buildable by authoring new content against these same systems, not by copy-pasting or forking scripts. If a build-order item's implementation only makes sense for this one case, that's a signal to find the general version of the mechanic underneath it.

---

## Story

### Act 1 — The Case
The player is a detective (gender-flexible) hired by a male client from high society to investigate his fiancée for infidelity. The player gathers proof by talking to NPCs and sneaking around the fiancée and her suspected lover, fighting off the occasional thief or robber along the way (small Persona-style turn-based encounters). Once enough evidence is gathered, the detective hands it over to the client.

### Act 2 — The Reveal
The detective is invited to the couple's engagement ceremony. Neither the fiancée nor the lover appears. After a couple of hours, a banquet dish is brought out — when its contents are revealed, it holds the two lovers' decapitated heads.

### Act 3 — The Hunt
Shocked, the detective starts tracking their old client — now the prime suspect — and discovers he's a serial murderer. Finding him triggers the final confrontation: a Persona-style turn-based boss battle.

---

## Design Pillars This Mission Must Prove
- **Navigation** — ZZZ-style hub exploration (already built, `Hub_Zone01`)
- **Dialogue-driven investigation** — NPC conversations that deliver story and clues
- **Clue/evidence tracking** — a record of what the player has learned/collected
- **Sneak/observation mechanic** — gathering proof on the fiancée and lover without just talking to them
- **Turn-based combat** (Persona-style) — both minor encounters (thieves/robbers) and a final boss
- **A scripted event/reveal sequence** — for the Act 2 ceremony and banquet-dish reveal

**Deferred, explicitly out of scope for now**: atmosphere/lighting mood, audio, and all real character/environment art.

---

## Scope Boundaries
- No real character art or models — placeholder primitives (capsules, cubes) with name labels stand in for every NPC, including the client, fiancée, lover, thieves, and the final boss.
- No lighting/atmosphere pass — default/realtime lighting is fine throughout.
- No audio.
- The only goal is that the mechanics work and the story is playable start to finish. Art is the very last step, done on top of working mechanics once the whole mission is proven out.

---

## Build Order
Mechanics-only sub-projects, each sized to be achievable in a single session. Each one gets its own plan when work starts on it — this list is the sequence, not the detail.

1. **[Done] Dialogue + clue tracking foundation** — Yarn Spinner 3.2.8 (via OpenUPM), a persistent `ClueTracker` singleton, a static-class Yarn command/function bridge (`add_clue`/`has_clue`), and one placeholder NPC (`NPC_Witness`) with a branching test conversation in `Hub_Zone01`. See `features/dialogue-system.md` for the full script/scene reference. Verified in Play mode including the clue surviving a scene reload.
2. **[Done] Sneak/observation mechanic** — real stealth: NPCs get a facing-based vision cone and an Unaware/Suspicious/Alert state machine driven by a continuously decaying awareness meter; a timed `SneakObservationPoint` grants a clue if the player stays unseen (below `Alert`) for its duration; the player gets a crouch toggle that slows detection and movement. See `features/sneak-observation.md`. Verified live in Play mode against all six behavior cases (cone/range, escalation, decay, crouch effect, successful grant, reset-on-Alert).
3. **[Done] Combat system core** — a separate `Battle` scene, Speed-based turn order, Attack/Defend/Skill actions, data-authored enemies (`EnemyDefinition` ScriptableObjects — a future boss is just a new asset), and a win/lose flow back to the hub. See `features/combat-system.md`. Verified live in Play mode against all eight behavior cases (standalone scene, turn order, attack/defend/skill math, resource gating, win, lose, end-to-end hub trigger).
4. **[Done] Act 1 wiring** — the full infidelity case, playable start to finish: the Client gives the brief and accepts the report, an informant gives leads, two sneak targets (fiancée + lover) each grant their own evidence clue, a thief encounter provides incidental danger, and nothing except the Client is visible until the case is accepted. Also resolved the deferred "how do we know what to do when the game starts" question — see `features/mission-state.md`. Verified live in Play mode across the full loop, including the real dialogue-driven "accept the case" path.
5. **Event sequencer + Act 2 reveal** — a lightweight way to script a beat (take control away, move things along, show text/camera beats) for the ceremony scene and the banquet-dish reveal. Needs minimal placeholder blockout space to stage it in — that's structural, not art.
6. **Act 3 — the hunt and final battle** — the suspicion/chase progression and the final confrontation, reusing #3's combat system as a boss fight with its own stakes.

---

## Status
In progress. Decomposition and story agreed 2026-08-30. Items 1 (dialogue + clue tracking), 2 (sneak/observation), 3 (combat system core), and 4 (Act 1 wiring) all completed 2026-08-30 — see `features/dialogue-system.md`, `features/sneak-observation.md`, `features/combat-system.md`, and `features/mission-state.md`. Next up: item 5, the event sequencer + Act 2 reveal.
