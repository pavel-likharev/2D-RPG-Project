using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGroundedState : PlayerState
{
    public PlayerGroundedState(Player player, PlayerStateMachine stateMachine, string animationBoolName) : base(player, stateMachine, animationBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        player.SetZeroVelocity();
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();

        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            stateMachine.ChangeState(player.PrimaryAttackState);
        }

        if (Input.GetKeyDown(KeyCode.Q))
        {
            stateMachine.ChangeState(player.CounterAttackState);
        }

        if (Input.GetKeyDown(KeyCode.Mouse1) && HasNoSword())
        {
            stateMachine.ChangeState(player.AimSwordState);
        }

        if (!player.IsGroundedDetected())
        {
            stateMachine.ChangeState(player.AirState);
        }

        if (Input.GetKeyDown(KeyCode.Space) && player.IsGroundedDetected())
        {
            stateMachine.ChangeState(player.JumpState);
        }
    }

    private bool HasNoSword()
    {
        if (!player.Sword)
        {
            return true;
        }

        player.Sword.GetComponent<SkillThrowSwordController>().ReturnSword();
        return false;
    }
}
