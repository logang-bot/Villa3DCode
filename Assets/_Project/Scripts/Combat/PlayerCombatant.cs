using UnityEngine;

public class PlayerCombatant : MonoBehaviour
{
    [SerializeField] string displayName = "Detective";
    [SerializeField] int maxHP = 30;
    [SerializeField] int attack = 6;
    [SerializeField] int defense = 3;
    [SerializeField] int speed = 5;
    [SerializeField] int maxResource = 20;

    public CombatantState CreateState() => new CombatantState
    {
        DisplayName = displayName,
        IsPlayer = true,
        MaxHP = maxHP,
        HP = maxHP,
        Attack = attack,
        Defense = defense,
        Speed = speed,
        MaxResource = maxResource,
        Resource = maxResource
    };
}
