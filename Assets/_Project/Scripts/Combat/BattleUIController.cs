using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BattleUIController : MonoBehaviour
{
    [SerializeField] BattleStateMachine stateMachine;
    [SerializeField] TextMeshProUGUI playerStatusLabel;
    [SerializeField] Button attackButton;
    [SerializeField] Button defendButton;
    [SerializeField] Button skillButton;
    [SerializeField] Transform enemyListContainer;
    [SerializeField] GameObject enemyRowPrefab;

    readonly List<GameObject> spawnedRows = new();

    void Awake()
    {
        attackButton.onClick.AddListener(stateMachine.ChooseAttack);
        defendButton.onClick.AddListener(stateMachine.ChooseDefend);
        skillButton.onClick.AddListener(stateMachine.ChooseSkill);
    }

    void Update()
    {
        RefreshPlayerStatus();
        RefreshActionButtons();
        RefreshEnemyRows();
    }

    void RefreshPlayerStatus()
    {
        CombatantState p = stateMachine.Player;
        if (p == null) return;
        playerStatusLabel.text = $"{p.DisplayName}  HP {p.HP}/{p.MaxHP}  SP {p.Resource}/{p.MaxResource}";
    }

    void RefreshActionButtons()
    {
        bool active = stateMachine.Phase == BattlePhase.AwaitingPlayerAction;
        attackButton.gameObject.SetActive(active);
        defendButton.gameObject.SetActive(active);
        skillButton.gameObject.SetActive(active);
    }

    void RefreshEnemyRows()
    {
        if (stateMachine.Enemies == null) return;
        ClearRows();
        bool targeting = stateMachine.Phase == BattlePhase.AwaitingTarget;
        foreach (CombatantState enemy in stateMachine.Enemies)
            BuildRow(enemy, targeting);
    }

    void ClearRows()
    {
        foreach (GameObject row in spawnedRows) Destroy(row);
        spawnedRows.Clear();
    }

    void BuildRow(CombatantState enemy, bool targetable)
    {
        GameObject row = Instantiate(enemyRowPrefab, enemyListContainer);
        spawnedRows.Add(row);
        TextMeshProUGUI label = row.GetComponentInChildren<TextMeshProUGUI>();
        label.text = $"{enemy.DisplayName}  HP {enemy.HP}/{enemy.MaxHP}";
        Button button = row.GetComponentInChildren<Button>();
        button.interactable = targetable && enemy.IsAlive;
        button.onClick.AddListener(() => stateMachine.ChooseTarget(enemy));
    }
}
