---
description: Updates all project docs (project-overview, workflow, systems, scene-setup) to reflect the current state of the codebase after a work session.
---

Update the project documentation to reflect the current state of the codebase. Follow these steps:

1. Read all files in docs/ (project-overview.md, workflow.md, systems.md, scene-setup.md)
2. Read all .cs files under Assets/_Project/Scripts/ to check for changes or new scripts
3. Ask the user: "What changed this session?" if the scope is unclear — do not guess at scene configurations since those can't be read from code

Then update the relevant docs:

**project-overview.md** — mark any newly completed milestones with [x]

**workflow.md** — update:
- "Where We Left Off" to reflect the current stopping point
- "Next Steps" to reflect what remains
- "Key Decisions Made" if any new architectural choices were made this session

**systems.md** — for any new or changed script:
- Add or update its entry with correct fields, defaults, and behaviour description
- Remove entries for deleted scripts

**scene-setup.md** — only update if the user confirms scene changes; do not infer scene state from scripts alone

Rules:
- Do not add placeholder or speculative content
- Keep each doc focused on its purpose (see the doc headers for scope)
- If a section is already accurate, leave it unchanged
- After updating, list which files changed and what was updated in each
