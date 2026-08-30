using UnityEngine;

[CreateAssetMenu(menuName = "Combat/Enemy Definition", fileName = "NewEnemy")]
public class EnemyDefinition : ScriptableObject
{
    [SerializeField] string displayName = "Enemy";
    [SerializeField] int maxHP = 20;
    [SerializeField] int attack = 5;
    [SerializeField] int defense = 2;
    [SerializeField] int speed = 5;
    [SerializeField] int maxResource = 10;

    public string DisplayName => displayName;

    public CombatantState CreateState() => new CombatantState
    {
        DisplayName = displayName,
        IsPlayer = false,
        MaxHP = maxHP,
        HP = maxHP,
        Attack = attack,
        Defense = defense,
        Speed = speed,
        MaxResource = maxResource,
        Resource = maxResource
    };
}
