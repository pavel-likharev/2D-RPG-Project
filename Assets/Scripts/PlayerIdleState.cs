using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerIdleState : PlayerGroundedState
{
    public PlayerIdleState(Player player, PlayerStateMachine stateMachine, string animationBoolName) : base(player, stateMachine, animationBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();

        player.ZeroVelocity();
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();

        if (xInput == player.MoveDir && player.IsWallDetected())
        {
            return;
        }

        if (xInput != 0 && !player.IsBusy)
        {
            stateMachine.ChangeState(player.MoveState);
        }
    }
}
