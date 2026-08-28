---
description: Updates all project docs (project-overview, roadmap, and per-feature docs under features/) to reflect the current state of the codebase after a work session.
---

Update the project documentation to reflect the current state of the codebase. Follow these steps:

1. Read all files in docs/ (project-overview.md, roadmap.md, and every file under features/)
2. Read all .cs files under Assets/_Project/Scripts/ to check for changes or new scripts
3. Ask the user: "What changed this session?" if the scope is unclear — do not guess at scene configurations since those can't be read from code

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

Rules:
- Do not add placeholder or speculative content
- Keep each doc focused on its purpose (see the doc headers for scope)
- If a section is already accurate, leave it unchanged
- After updating, list which files changed and what was updated in each
