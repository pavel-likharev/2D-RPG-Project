using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : CharacterStats
{
    private Player player;

    protected override void Awake()
    {
        base.Awake();

        currentHealth = GetMaxHealthValue();
        player = GetComponent<Player>();
    }

    protected override void Die()
    {
        base.Die();

        player.Die();


        GameManager.Instance.SetLostCurrency(PlayerManager.Instance.GetCurrency());
        PlayerManager.Instance.SetZeroCurrency();
        
        GetComponent<ItemDrop_Player>()?.GenerateDrop();
    }

    protected override void DecreaseHealth(int damage)
    {
        base.DecreaseHealth(damage);

        Inventory.Instance.UseArmor();
    }
}
