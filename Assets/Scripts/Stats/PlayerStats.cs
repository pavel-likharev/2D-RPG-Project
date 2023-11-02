using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : CharacterStats
{
    private Player player;

    private void Awake()
    {
        player = GetComponent<Player>();
    }

    protected override void Die()
    {
        base.Die();

        player.Die();
        GetComponent<ItemDrop_Player>()?.GenerateDrop();
    }

    protected override void DecreaseHealth(int damage)
    {
        base.DecreaseHealth(damage);

        Inventory.Instance.UseArmor();
    }
}
