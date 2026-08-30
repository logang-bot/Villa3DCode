using System.Collections.Generic;

// One-shot Hub->Battle handoff: set once before LoadScene, read once in the
// next scene's Awake. Plain static (not DontDestroyOnLoad) since it needs no
// GameObject lifecycle. Not visible in the Hierarchy like ClueTracker is;
// relies on domain-reload-on-play (the project default) to avoid stale data.
public static class PendingEncounter
{
    public static List<EnemyDefinition> Enemies { get; private set; }

    public static void Set(List<EnemyDefinition> enemies) => Enemies = enemies;

    public static void Clear() => Enemies = null;
}
