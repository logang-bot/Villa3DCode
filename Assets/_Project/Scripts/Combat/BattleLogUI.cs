using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class BattleLogUI : MonoBehaviour
{
    [SerializeField] BattleStateMachine stateMachine;
    [SerializeField] TextMeshProUGUI logLabel;
    [SerializeField] int maxLines = 6;

    readonly List<string> lines = new();

    void OnEnable() => stateMachine.OnLog += HandleLog;
    void OnDisable() => stateMachine.OnLog -= HandleLog;

    void HandleLog(string message)
    {
        lines.Add(message);
        if (lines.Count > maxLines) lines.RemoveAt(0);
        logLabel.text = string.Join("\n", lines);
    }
}
