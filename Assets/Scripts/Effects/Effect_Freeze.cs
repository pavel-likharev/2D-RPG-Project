using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Effect_Freeze : Effect
{
    [SerializeField] private float duration;
    [SerializeField] private float peircentHealthEffect;

    private float freezeRadius = 2;

    public override void ExecuteEffect(Transform transform)
    {
        PlayerStats playerStats = PlayerManager.Instance.Player.GetComponent<PlayerStats>();

        if (playerStats.currentHealth < playerStats.GetMaxHealthValue() * peircentHealthEffect)
        {
            Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, freezeRadius);
            foreach (var hit in colliders)
            {
                hit.GetComponent<Enemy>()?.FreezeTimeFor(duration);
            }

            Inventory.Instance.lastTimeUsedArmor = Time.time;
        }
    }
}
