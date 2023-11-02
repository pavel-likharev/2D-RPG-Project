using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationTriggers : MonoBehaviour
{
    private Player player => PlayerManager.Instance.Player;

    private void AnimationTrigger()
    {
        player.AnimationTrigger();
    }

    private void AttackTrigger()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(player.attackCheck.position, player.attackCheckRadius);

        foreach (Collider2D hit in colliders)
        {
            if (hit.GetComponent<Enemy>() != null)
            {
                EnemyStats target = hit.GetComponent<EnemyStats>();

                player.Stats.DoDamage(target, player.MoveDir);

                Inventory.Instance.GetEquipment(EquipmentType.Weapon)?.ApplyEffect(target.transform);
            }
        }
    }

    private void SkillSwordTrigger()
    {
        SkillManager.Instance.SwordSkillController.CanUseSkill();
    }
}
