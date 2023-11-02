using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStats : CharacterStats
{
    private Enemy enemy;
    private ItemDrop myDrop;

    [Header("Level details")]
    [SerializeField] private int level = 1;

    [Range(0f, 1f)][SerializeField] private float percantageModifier = 0.4f;

    private void Awake()
    {
        enemy = GetComponent<Enemy>();
        myDrop = GetComponent<ItemDrop>();
    }

    protected override void Start()
    {
        ApplyLevelModifiers();

        base.Start();
    }

    private void ApplyLevelModifiers()
    {
        Modify(strength);
        Modify(agility);
        Modify(intelligence);
        Modify(vitality);

        Modify(damage);
        Modify(critChance);
        Modify(critPower);

        Modify(maxHealth);
        Modify(armor);
        Modify(evasion);
        Modify(magicResistance);

        Modify(fireDamage);
        Modify(iceDamage);
        Modify(lightingDamage);
    }

    private void Modify(Stat stat)
    {
        for (int i = 0; i < level; i++)
        {
            float modifier = stat.GetValue() * percantageModifier;
            stat.AddModifier(Mathf.RoundToInt(modifier));
        }
    }

    protected override void Die()
    {
        base.Die();

        enemy.Die();
        myDrop.GenerateDrop();
    }
}
