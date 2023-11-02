using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum StatType
{
    strength,
    agility,
    intelligence,
    vitality,
    damage,
    critChance,
    critPower,
    health,
    armor,
    evasion,
    magicResistance,
    fireDamage,
    iceDamage,
    lightingDamage,
}

public class Effect_Buff : Effect
{
    private PlayerStats stats;
    [SerializeField] private StatType buffType;
    [SerializeField] private int buffValue;
    [SerializeField] private float buffDuartion;

    public override void ExecuteEffect(Transform target)
    {
        stats = PlayerManager.Instance.Player.GetComponent<PlayerStats>();

        stats.IncreaseStat(GetStatFromType(), buffValue, buffDuartion);
    }

    private Stat GetStatFromType()
    {
        switch (buffType)
        {
            case StatType.strength:
                return stats.strength;
                
            case StatType.agility:
                return stats.agility;
            case StatType.intelligence:
                return stats.intelligence;
            case StatType.vitality:
                return stats.vitality;
            case StatType.damage:
                return stats.damage;
            case StatType.critChance:
                return stats.critChance;
            case StatType.critPower:
                return stats.critPower;
            case StatType.health:
                return stats.maxHealth;
            case StatType.armor:
                return stats.armor;
            case StatType.evasion:
                return stats.evasion;
            case StatType.magicResistance:
                return stats.magicResistance;
            case StatType.fireDamage:
                return stats.fireDamage;
            case StatType.iceDamage:
                return stats.iceDamage;
            case StatType.lightingDamage:
                return stats.lightingDamage;
            default:
                return null;
        }
    }
}
