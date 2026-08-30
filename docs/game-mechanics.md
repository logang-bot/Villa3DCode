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
- Can chain multiple conditions together (e.g. only offering a dialogue option once *two* separate clues both exist)

Two real Act 1 NPCs use this today: the Client (who gives the case and takes the report) and an informant (who gives leads once the case is accepted).

→ `features/dialogue-system.md`

---

## Sneak & Observation
NPCs can watch for the player: a facing-based vision cone, with a continuously rising/decaying awareness meter driving Unaware → Suspicious → Alert. Standing unseen inside an observation zone for long enough grants a clue automatically — no button needed, just staying out of sight. Getting fully spotted (`Alert`) cancels the current attempt; the NPC calms back down on its own as awareness decays, no separate cooldown needed. The player has a crouch toggle (**Left Ctrl**) that meaningfully slows detection (and movement) while active.

Two real suspects use this today: the fiancée and her lover, each with their own watcher and observation point, granting distinct evidence clues.

→ `features/sneak-observation.md`

---

## Combat
Turn-based, Persona-style: entering a fight (via a trigger in the hub) loads a separate Battle scene. Turn order goes by Speed, fastest first. Each round the player picks Attack, Defend (halves incoming damage until their next turn), or a Skill (stronger attack, costs SP — fails cleanly with no wasted turn if you don't have enough). Enemies just always attack back. Win or lose, the fight ends and you're returned to the hub after a moment.

One real encounter exists today — a "Thief" ambush along the Act 1 investigation route, incidental danger that grants no evidence. Enemies are authored as data, not code, so future fights (including the eventual final boss) are just new enemy stats, not new systems.

→ `features/combat-system.md`

---

## Clue & Evidence Tracking
A persistent record of what the detective has learned, shared across the whole game session — collected clues survive scene reloads (the game's first save-across-scenes system, technically). Dialogue can grant a clue and later check whether the player already has one, letting conversations react to prior progress.

Currently the only visible way to check collected clues is a small always-on debug list in the corner of the screen — not a real journal UI yet.

→ `features/dialogue-system.md`

---

## Mission Progression
The game now knows what to do when it starts — this used to be a real gap (every test object was live simultaneously with no framing). A generic component (`RequiresClue`) hides world content until a specific clue/flag is present, and reacts live the moment it appears, no reload needed. Today only the Client is visible at boot; accepting his case instantly reveals the informant's real leads, both sneak targets, and the thief encounter. The same mechanism is meant to be reused for every future mission's own progression, not just this one.

→ `features/mission-state.md`

---

## What Isn't Built Yet
Worth being explicit about, since it's easy to assume more exists than does:
- **A real journal/evidence UI** — today it's a debug text list, not a designed screen.
- **Multiple hub zones** — only `Hub_Zone01` exists; no ceremony hall, no additional locations yet.
- **A proper scene-transition system** — scene loads are instant with no fade; the previous fade system was removed for making the Editor unusable.
- **The rest of the mission's NPC roster** — the killer/final-boss confrontation isn't placed yet (Act 3, item 6).
- **NavMesh** — the package is installed but nothing is baked; no AI pathing exists yet.
- **Any real art, lighting mood, or audio** — deliberately deferred until the mechanics above are proven out (see `mission-design.md`'s Scope Boundaries).

---

## See Also
- `mission-design.md` — the story this is all being built toward, and the build order
- `mission-walkthrough.md` — the full step-by-step playthrough guide, spoilers included
- `roadmap.md` — current milestone status and what's next
