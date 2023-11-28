using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMoveState : PlayerGroundedState
{
    public PlayerMoveState(Player player, PlayerStateMachine stateMachine, string animationBoolName) : base(player, stateMachine, animationBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();

        AudioManager.Instance.PlaySFX(14);
    }

    public override void Exit()
    {
        base.Exit();

        AudioManager.Instance.StopSFX(14);
    }

    public override void Update()
    {
        base.Update();

        player.SetVelocity(xInput * player.moveSpeed, rb.velocity.y);

        if (xInput == 0 || player.IsWallDetected())
        {
            stateMachine.ChangeState(player.IdleState);
        }
    }
}
