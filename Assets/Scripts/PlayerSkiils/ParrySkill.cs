using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ParrySkill : MonoBehaviour
{
    private const string IS_SUCCESSFUL_PARRY = "IsSuccessfulCounterAttack";

    private Player player;

    private void Start()
    {
        player = GetComponent<Player>();
    }

    public bool CheckParry()
    {
        bool result = false;

        Collider2D[] colliders = Physics2D.OverlapCircleAll(player.attackCheck.position, player.attackCheckRadius);

        foreach (Collider2D hit in colliders)
        {
            if (hit.GetComponent<Enemy>() != null)
            {
                if (hit.GetComponent<Enemy>().CanBeStunned())
                {
                    player.Animator.SetBool(IS_SUCCESSFUL_PARRY, true);
                    result = true;

                    player.Skill.ParrySkillController.AttemptRestoreHealth();
                    player.Skill.ParrySkillController.AttemptCreateClone(hit.transform);
                }
            }
        }

        return result;
    }


}
