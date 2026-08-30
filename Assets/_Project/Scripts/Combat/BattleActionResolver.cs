using UnityEngine;

public class BattleActionResolver : MonoBehaviour
{
    [SerializeField] int minimumDamage = 1;
    [SerializeField] float skillDamageMultiplier = 1.5f;
    [SerializeField] int skillResourceCost = 10;
    [SerializeField] float defendDamageMultiplier = 0.5f;

    public int SkillResourceCost => skillResourceCost;

    public int ResolveAttack(CombatantState attacker, CombatantState defender)
    {
        int damage = DamageFor(attacker.Attack, defender);
        defender.ApplyDamage(damage);
        return damage;
    }

    public bool TryResolveSkill(CombatantState attacker, CombatantState defender, out int damage)
    {
        damage = 0;
        if (attacker.Resource < skillResourceCost) return false;
        attacker.SpendResource(skillResourceCost);
        damage = DamageFor(Mathf.RoundToInt(attacker.Attack * skillDamageMultiplier), defender);
        defender.ApplyDamage(damage);
        return true;
    }

    public void ResolveDefend(CombatantState defender) => defender.IsDefending = true;

    int DamageFor(int rawAttack, CombatantState defender)
    {
        int baseDamage = Mathf.Max(minimumDamage, rawAttack - defender.Defense);
        return defender.IsDefending ? Mathf.CeilToInt(baseDamage * defendDamageMultiplier) : baseDamage;
    }
}
