using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBlackholeState : PlayerState
{
    private float flyTime = 0.4f;
    private float flySpeed = 12;
    private bool isSkillUse;

    private float defaultGravity;


    public PlayerBlackholeState(Player player, PlayerStateMachine stateMachine, string animationBoolName) : base(player, stateMachine, animationBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();

        isSkillUse = false;
        stateTimer = flyTime;
        defaultGravity = rb.gravityScale;
        rb.gravityScale = 0;
    }

    public override void Exit()
    {
        base.Exit();

        rb.gravityScale = defaultGravity;
        player.MakeTransparent(false);
    }

    public override void Update()
    {
        base.Update();

        if (stateTimer > 0)
        {
            rb.velocity = new Vector2(0, flySpeed);
        }

        if (stateTimer < 0)
        {
            rb.velocity = new Vector2(0, -0.1f);

            if (!isSkillUse)
            {
                if (player.Skill.BlackholeSkillController.CanUseSkill())
                {
                    isSkillUse = true;
                }
            }
        }

        if (player.Skill.BlackholeSkillController.SkillFinished())
        {
            stateMachine.ChangeState(player.AirState);
        }
    }
}
