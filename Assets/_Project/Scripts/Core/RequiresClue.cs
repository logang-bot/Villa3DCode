using UnityEngine;

public class RequiresClue : MonoBehaviour
{
    [SerializeField] string clueId;
    [SerializeField] GameObject[] targets;

    void OnEnable() => ClueTracker.ClueAdded += OnClueAdded;
    void OnDisable() => ClueTracker.ClueAdded -= OnClueAdded;

    void Start() => ApplyState();

    void OnClueAdded(string id)
    {
        if (id == clueId) ApplyState();
    }

    void ApplyState()
    {
        bool has = ClueTracker.Instance != null && ClueTracker.Instance.HasClue(clueId);
        foreach (var target in targets) target.SetActive(has);
    }
}
