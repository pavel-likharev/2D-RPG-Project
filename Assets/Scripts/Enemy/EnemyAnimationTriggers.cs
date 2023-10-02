using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAnimationTriggers : MonoBehaviour
{
    Enemy enemy => GetComponentInParent<Enemy>();

    private void AnimationTrigger()
    {
        enemy.AnimationTrigger();
    }

    private void AttackTrigger()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(enemy.attackCheck.position, enemy.attackCheckRadius);

        foreach (Collider2D hit in colliders)
        {
            if (hit.GetComponent<Player>() != null)
            {
                hit.GetComponent<Player>().TakeDamage(enemy.MoveDir);
            }
        }
    }

    private void OpenCounterAttackWindow() => enemy.OpenCounterAttackWindow();
    private void CloseCounterAttackWindow() => enemy.CloseCounterAttackWindow();
}
