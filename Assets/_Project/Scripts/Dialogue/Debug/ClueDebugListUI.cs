using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ClueDebugListUI : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI listLabel;
    readonly List<string> collected = new();

    void OnEnable() => ClueTracker.ClueAdded += HandleClueAdded;
    void OnDisable() => ClueTracker.ClueAdded -= HandleClueAdded;

    void HandleClueAdded(string id)
    {
        collected.Add(id);
        listLabel.text = string.Join("\n", collected);
    }
}
