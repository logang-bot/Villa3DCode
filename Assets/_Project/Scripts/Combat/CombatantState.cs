using UnityEngine;

public class CombatantState
{
    public string DisplayName;
    public bool IsPlayer;
    public int MaxHP;
    public int HP;
    public int Attack;
    public int Defense;
    public int Speed;
    public int MaxResource;
    public int Resource;
    public bool IsDefending;

    public bool IsAlive => HP > 0;

    public void ApplyDamage(int amount) => HP = Mathf.Max(0, HP - amount);

    public void SpendResource(int amount) => Resource = Mathf.Max(0, Resource - amount);
}
