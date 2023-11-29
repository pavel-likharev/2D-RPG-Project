using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStats : CharacterStats
{
    private Enemy enemy;
    private ItemDrop myDrop;
    public Stat currencyAmount;

    private int currencyMultiplier = 100;

    [Header("Level details")]
    [SerializeField] private int level = 1;

    [Range(0f, 1f)][SerializeField] private float percantageModifier = 0.4f;

    protected override void Awake()
    {
        base.Awake();

        enemy = GetComponent<Enemy>();
        myDrop = GetComponent<ItemDrop>();
    }

    protected override void Start()
    {
        currencyAmount.SetDefaultValue(currencyMultiplier);

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

        Modify(currencyAmount);
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

        PlayerManager.Instance.AddCurrency(currencyAmount.GetValue());

        enemy.Die();
        myDrop.GenerateDrop();

        Destroy(gameObject, 3f);
    }
}
