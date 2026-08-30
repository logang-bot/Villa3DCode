using Yarn.Unity;

public static class ClueYarnBridge
{
    [YarnCommand("add_clue")]
    public static void AddClue(string id) => ClueTracker.Instance?.AddClue(id);

    [YarnFunction("has_clue")]
    public static bool HasClue(string id) =>
        ClueTracker.Instance != null && ClueTracker.Instance.HasClue(id);
}
