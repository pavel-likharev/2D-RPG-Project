using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerDashState : PlayerState
{
    public PlayerDashState(Player player, PlayerStateMachine stateMachine, string animationBoolName) : base(player, stateMachine, animationBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();

        player.Stats.MakeInvincible(true);

        player.Skill.DashSkillController.CloneOnDash();

        stateTimer = player.DashSkill.dashDuration;

    }

    public override void Exit()
    {
        base.Exit();

        player.Skill.DashSkillController.CloneOnArrival();
        player.SetVelocity(0, rb.velocity.y);

        player.Stats.MakeInvincible(false);
    }

    public override void Update()
    {
        base.Update();

        if (!player.IsGroundedDetected() && player.IsWallDetected())
        {
            stateMachine.ChangeState(player.WallSlideState);
        }

        player.SetVelocity(player.DashSkill.dashForce * player.DashSkill.DashDir, 0);

        if (stateTimer <= 0)
        {
            stateMachine.ChangeState(player.IdleState);
        }

        player.CharacterFX.CreateAfterImage();
    }
}
