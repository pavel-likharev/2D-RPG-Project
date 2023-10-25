using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStats : CharacterStats
{
    private Enemy enemy;

    private void Awake()
    {
        enemy = GetComponent<Enemy>();
    }

    public override void TakeDamage(int damage)
    {
        base.TakeDamage(damage);
        enemy.DamageEffect(Player.Instance.MoveDir);
    }

    protected override void Die()
    {
        base.Die();
        enemy.Die();
    }
}
