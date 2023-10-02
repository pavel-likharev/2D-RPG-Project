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

        SkillManager.Instance.SkillClone.CreateClone(player.transform);

        stateTimer = player.dashDuration;
    }

    public override void Exit()
    {
        base.Exit();

        player.SetVelocity(0, rb.velocity.y);
    }

    public override void Update()
    {
        base.Update();

        if (!player.IsGroundedDetected() && player.IsWallDetected())
        {
            stateMachine.ChangeState(player.WallSlideState);
        }

        player.SetVelocity(player.dashForce * player.DashDir, 0);

        if (stateTimer <= 0)
        {
            stateMachine.ChangeState(player.IdleState);
        }
    }
}
