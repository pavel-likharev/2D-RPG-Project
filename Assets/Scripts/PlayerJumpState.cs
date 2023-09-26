using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerJumpState : PlayerState
{
    public PlayerJumpState(Player player, PlayerStateMachine stateMachine, string animationBoolName) : base(player, stateMachine, animationBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();

       player.SetVelocity(rb.velocity.x, player.jumpForce);
        
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();

        if (rb.velocity.y < 0)
        {
            stateMachine.ChangeState(player.AirState);
        }

        if (xInput != 0)
        {
            player.SetVelocity(xInput * player.moveSpeed * 0.8f, rb.velocity.y);
        }

    }
}
