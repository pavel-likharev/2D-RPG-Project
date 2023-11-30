using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : CharacterStats
{
    private Player player;

    private float limitForKnockback = 0.3f;

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

        if (damage > GetMaxHealthValue() * limitForKnockback)
        {
            Vector2 knockbackPower = new Vector2(7, 10);
            player.SetupKnockbackPower(knockbackPower);
            AudioManager.Instance.PlaySFX(32);
            player.FX.ScreenShakeOnHugeDamage();
        }

        Inventory.Instance.UseArmor();
    }
}
