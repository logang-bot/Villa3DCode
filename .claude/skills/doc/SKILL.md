---
description: Updates all project docs (project-overview, roadmap, mission-design, mission-walkthrough, game-mechanics, and per-feature docs under features/) to reflect the current state of the codebase after a work session.
---

Update the project documentation to reflect the current state of the codebase. Follow these steps:

1. Read all files in docs/ (project-overview.md, roadmap.md, mission-design.md, mission-walkthrough.md, game-mechanics.md, and every file under features/)
2. Read all .cs files under Assets/_Project/Scripts/ to check for changes or new scripts
3. Ask the user: "What changed this session?" if the scope is unclear — do not guess at scene configurations since those can't be read from code
4. If mission-design.md's Build Order (see below) has any [Done] items not yet reflected in mission-walkthrough.md's ✅/🔜 markers, treat that mismatch as part of this session's changes even if nothing else did — the two docs must stay in sync

Then update the relevant docs:

**project-overview.md** — update Concept/Engine & Stack/Installed Packages/Art Direction/Project Structure only if the project's shape itself changed (new package, new top-level folder, etc.)

**roadmap.md** — update:
- Milestones checklist — mark any newly completed items with [x]
- "Where We Left Off" to reflect the current stopping point
- "Next Steps" to reflect what remains
- "Key Decisions Made" only for project-wide decisions (engine/pipeline/input-system level); component-specific decisions go in the relevant feature doc instead

**features/*.md** — one file per feature (player-movement.md, camera.md, interaction-system.md, scene-transitions.md; add a new file for a new feature). For any new or changed script belonging to that feature:
- Add or update its script reference entry with correct fields, defaults, and behaviour description
- Remove entries for deleted scripts
- Add a "Design Decisions" bullet if a new architectural choice was made for that feature this session
- Only update the "Scene Wiring" section if the user confirms scene changes; do not infer scene state from scripts alone

**mission-design.md** — only touch this if a mission build-order item was completed or the story/scope changed this session:
- Mark the completed item `[Done]` in the Build Order list, with a one-sentence summary and a pointer to its new/updated `features/*.md` entry
- Update the Status line at the bottom to name the next not-yet-done item

**mission-walkthrough.md** — whenever a build-order item lands (this is the doc most likely to go stale, since nothing forces it to update):
- Flip every step whose "🧩 Needs" item is now satisfied from 🔜 to ✅ — if the item only builds the mechanic (not the actual mission content for that step), use "✅ ... (mechanic done, content pending)" exactly like the sneak-observation precedent, and still list what content item unlocks the rest
- Update the "Currently Playable" section to describe any new placeholder/test content a player can actually walk up to and try today
- Update the Cross-Reference table's "Unlocks story steps" done-marker for that build-order item

**game-mechanics.md** — whenever a mechanic (not just mission content) is newly built:
- Add or extend a section describing what the player can now do, in plain terms, ending with a `→ features/*.md` pointer
- Remove the corresponding bullet from "What Isn't Built Yet" if one exists there

Rules:
- Do not add placeholder or speculative content
- Keep each doc focused on its purpose (see the doc headers for scope)
- If a section is already accurate, leave it unchanged
- After updating, list which files changed and what was updated in each
