using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public enum BattlePhase { Setup, AwaitingPlayerAction, AwaitingTarget, Won, Lost }

public class BattleStateMachine : MonoBehaviour
{
    enum PendingActionType { None, Attack, Skill }

    [SerializeField] PlayerCombatant playerCombatant;
    [SerializeField] EnemySpawner enemySpawner;
    [SerializeField] BattleActionResolver resolver;
    [SerializeField] BattleResultUI resultUI;

    List<CombatantState> turnOrder = new();
    int turnIndex;
    PendingActionType pendingAction;

    public event Action<string> OnLog;

    public BattlePhase Phase { get; private set; }
    public CombatantState Player { get; private set; }
    public IReadOnlyList<CombatantState> Enemies { get; private set; }

    // Setup runs in Start, not Awake, so listeners (BattleLogUI's OnEnable
    // subscription) are guaranteed to be attached before the first log fires.
    void Start()
    {
        SetupBattle();
    }

    void SetupBattle()
    {
        Player = playerCombatant.CreateState();
        Enemies = enemySpawner.SpawnEnemies();
        StartRound();
    }

    public void StartRound()
    {
        turnOrder = ActiveCombatants().OrderByDescending(c => c.Speed).ToList();
        turnIndex = 0;
        BeginTurn();
    }

    List<CombatantState> ActiveCombatants()
    {
        var list = new List<CombatantState> { Player };
        list.AddRange(Enemies.Where(e => e.IsAlive));
        return list;
    }

    void BeginTurn()
    {
        CombatantState active = turnOrder[turnIndex];
        active.IsDefending = false;
        if (active.IsPlayer) Phase = BattlePhase.AwaitingPlayerAction;
        else RunEnemyTurn(active);
    }

    void RunEnemyTurn(CombatantState enemy)
    {
        int damage = resolver.ResolveAttack(enemy, Player);
        Log($"{enemy.DisplayName} attacks for {damage} damage.");
        CheckOutcome();
        AdvanceTurn();
    }

    public void ChooseAttack()
    {
        if (Phase != BattlePhase.AwaitingPlayerAction) return;
        pendingAction = PendingActionType.Attack;
        ResolveOrAwaitTarget();
    }

    public void ChooseSkill()
    {
        if (Phase != BattlePhase.AwaitingPlayerAction) return;
        if (Player.Resource < resolver.SkillResourceCost) { Log("Not enough SP."); return; }
        pendingAction = PendingActionType.Skill;
        ResolveOrAwaitTarget();
    }

    void ResolveOrAwaitTarget()
    {
        var alive = Enemies.Where(e => e.IsAlive).ToList();
        if (alive.Count == 1) ChooseTarget(alive[0]);
        else Phase = BattlePhase.AwaitingTarget;
    }

    public void ChooseDefend()
    {
        if (Phase != BattlePhase.AwaitingPlayerAction) return;
        resolver.ResolveDefend(Player);
        Log($"{Player.DisplayName} braces for the next hit.");
        AdvanceTurn();
    }

    public void ChooseTarget(CombatantState target)
    {
        if (pendingAction == PendingActionType.Attack) ResolvePlayerAttack(target);
        else if (pendingAction == PendingActionType.Skill) ResolvePlayerSkill(target);
        pendingAction = PendingActionType.None;
        CheckOutcome();
        AdvanceTurn();
    }

    void ResolvePlayerAttack(CombatantState target)
    {
        int damage = resolver.ResolveAttack(Player, target);
        Log($"{Player.DisplayName} attacks {target.DisplayName} for {damage} damage.");
    }

    void ResolvePlayerSkill(CombatantState target)
    {
        resolver.TryResolveSkill(Player, target, out int damage);
        Log($"{Player.DisplayName} uses a skill on {target.DisplayName} for {damage} damage.");
    }

    void CheckOutcome()
    {
        if (Enemies.All(e => !e.IsAlive)) EndBattle(true);
        else if (!Player.IsAlive) EndBattle(false);
    }

    void EndBattle(bool won)
    {
        Phase = won ? BattlePhase.Won : BattlePhase.Lost;
        Log(won ? "Victory!" : "Defeated...");
        resultUI.ShowResult(won);
    }

    void AdvanceTurn()
    {
        if (Phase == BattlePhase.Won || Phase == BattlePhase.Lost) return;
        turnIndex++;
        while (turnIndex < turnOrder.Count && !turnOrder[turnIndex].IsAlive) turnIndex++;
        if (turnIndex >= turnOrder.Count) StartRound();
        else BeginTurn();
    }

    void Log(string message) => OnLog?.Invoke(message);
}
