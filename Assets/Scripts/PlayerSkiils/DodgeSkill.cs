using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DodgeSkill : MonoBehaviour
{
    private Player player;

    private void Start()
    {
        player = GetComponent<Player>();

        player.Stats.OnEvasion += Stats_OnEvasion;
    }

    private void Stats_OnEvasion(object sender, TransformTargetEventsArgs e)
    {
        CreateCloneOnDodge(e.target.transform);
    }

    private void CreateCloneOnDodge(Transform target)
    {
        if (player.Skill.DodgeSkillController.DodgeWithCloneUnlocked)
            player.Skill.CloneSkillController.CreateClone(target, new Vector2(player.MoveDir, 0));
    }
}
