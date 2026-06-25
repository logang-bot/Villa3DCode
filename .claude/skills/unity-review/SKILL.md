---
description: Reviews all C# scripts for Unity best practices and project-specific patterns (Input System, SceneTransition, CompareTag, TMP, etc.).
---

Review all C# scripts under Assets/_Project/Scripts/ for correctness, Unity best practices, and consistency with this project's established patterns.

## What to check

**Unity-specific pitfalls**
- Component lookups (`GetComponent`, `FindObjectOfType`, `FindWithTag`) inside Update/FixedUpdate — should be cached in Awake/Start
- Missing null guards on singleton `instance` references before use
- Coroutines started on objects that might be inactive or destroyed
- `OnTriggerEnter`/`OnCollisionEnter` without tag checks — can fire on unintended objects
- String-based tag comparisons using `==` instead of `CompareTag`
- Heavy allocations per frame (LINQ, `new` inside Update, string concatenation)
- `Camera.main` called every frame — it does a `FindWithTag` internally; should be cached

**Input**
- Any use of the legacy `Input` class — this project uses `UnityEngine.InputSystem` exclusively
- Input read outside of Update (e.g. in FixedUpdate for non-physics input)

**Project patterns to enforce**
- Scene transitions must use `SceneTransition.Load(sceneName)` — not `SceneManager.LoadScene` directly and not via UnityEvent object references
- Player detection in triggers must use `CompareTag("Player")` — the player tag is "Player"
- All new UI text must use TextMeshPro (`TMPro`) — not the legacy `UnityEngine.UI.Text`
- Camera input reads `Mouse.current` — not `Input.GetAxis("Mouse X")`

**General C# quality**
- Serialized fields that are never assigned in the Inspector and have no code default
- Public fields that should be `[SerializeField] private`
- Magic numbers that should be named constants or serialized fields
- Methods longer than ~30 lines that could be split

## Output format
For each issue found: file path, brief description of the problem, and a one-line fix suggestion.
Group by file. If a file is clean, skip it.
At the end, give a summary: how many issues found, which files are clean.
