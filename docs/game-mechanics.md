# Game Mechanics & Features

What the game actually does today, in plain terms — for a quick read on "what can the player do right now." For exact script fields and scene wiring, follow the links into `features/`.

---

## Exploration & Movement
The player controls a detective (currently a capsule placeholder) walking around a hub-style map — not open world, camera-relative WASD movement, gamepad support, cursor-locked mouse look.
- WASD / arrow keys / gamepad stick moves relative to the camera
- The character rotates to face its movement direction
- Mouse independently orbits the camera around the character (Cinemachine orbital follow); Escape unlocks the cursor
- Gravity and ground collision are real physics (`CharacterController` against `MeshCollider`s on the imported geometry)

→ `features/player-movement.md`, `features/camera.md`

---

## The Hub World — Hub_Zone01 (Potosí Plaza)
The only scene in the game right now. Built from real art imported from the Blender source (`VillaCity.blend`) — not a greybox: the plaza, cathedral, government palace ("the Moneda"), a secondary square (LPlaza), 12 lit lamp posts, and 9 surrounding city blocks, all with real collision so the player can't fall through the world.

Player spawns at the plaza's south edge, facing the central statue/fountain.

→ `features/hub-environment.md`

---

## Interaction System
Walking near an interactable object shows a context prompt ("Press E to..."); pressing **E** triggers its action. This one system covers three different behaviors, chosen per-object:
- **Scene transition** — E instantly loads another scene (used today only by a test zone that reloads the current scene)
- **Flavor message** — E shows a one-line piece of text on screen for a few seconds, then it auto-hides (used by the three landmark zones below)
- **Custom event** — E fires an arbitrary hook, which is how NPC dialogue gets triggered (see below)

Three real landmarks currently have flavor-text interactions: the cathedral, the Moneda, and the central statue each show a short atmospheric line when investigated.

→ `features/interaction-system.md`

---

## Dialogue System
NPCs can hold real branching conversations, authored in Yarn Spinner's `.yarn` script format (not hardcoded in C#). Talking to an NPC:
- Shows lines one at a time with the speaker's name
- Presents multiple-choice options when the conversation branches
- Can change what the NPC says on a later conversation, based on what's already happened (e.g. "I already told you what I saw")

One placeholder NPC (`NPC_Witness`) exists today, proving this works end-to-end. The actual mission's cast of NPCs (the client, the fiancée, the lover, etc.) hasn't been placed yet — see `mission-walkthrough.md` for what's still to come.

→ `features/dialogue-system.md`

---

## Clue & Evidence Tracking
A persistent record of what the detective has learned, shared across the whole game session — collected clues survive scene reloads (the game's first save-across-scenes system, technically). Dialogue can grant a clue and later check whether the player already has one, letting conversations react to prior progress.

Currently the only visible way to check collected clues is a small always-on debug list in the corner of the screen — not a real journal UI yet.

→ `features/dialogue-system.md`

---

## What Isn't Built Yet
Worth being explicit about, since it's easy to assume more exists than does:
- **Sneaking / observation mechanic** — how the player is meant to gather evidence on suspects without just talking to them. Not started.
- **Combat** — no battle scene, no stats, no turn-based system at all yet. The "thief/robber" encounters and the final boss fight both depend on this.
- **A real journal/evidence UI** — today it's a debug text list, not a designed screen.
- **Multiple hub zones** — only `Hub_Zone01` exists; no ceremony hall, no additional locations yet.
- **A proper scene-transition system** — scene loads are instant with no fade; the previous fade system was removed for making the Editor unusable.
- **NPC roster for the mission** — client, fiancée, lover, and the killer are all still unplaced; only one generic test NPC exists.
- **NavMesh** — the package is installed but nothing is baked; no AI pathing exists yet.
- **Any real art, lighting mood, or audio** — deliberately deferred until the mechanics above are proven out (see `mission-design.md`'s Scope Boundaries).

---

## See Also
- `mission-design.md` — the story this is all being built toward, and the build order
- `mission-walkthrough.md` — the full step-by-step playthrough guide, spoilers included
- `roadmap.md` — current milestone status and what's next
