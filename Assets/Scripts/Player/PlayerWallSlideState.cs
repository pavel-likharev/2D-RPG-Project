using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWallSlideState : PlayerState
{
    public PlayerWallSlideState(Player player, PlayerStateMachine stateMachine, string animationBoolName) : base(player, stateMachine, animationBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();

        // When player press button a or d if he do moveskide - he fall from wall 

        //if ((xInput != 0 && xInput != player.MoveDir))
        //{
        //    stateMachine.ChangeState(player.IdleState);
        //}

        if (yInput < 0)
        {
            player.SetVelocity(0, rb.velocity.y);
        }
        else
        {
            player.SetVelocity(0, rb.velocity.y * 0.7f);
        }

        if (player.IsGroundedDetected() || !player.IsWallDetected())
        {
            stateMachine.ChangeState(player.IdleState);
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            stateMachine.ChangeState(player.WallJumpState);
        }


    }
}
