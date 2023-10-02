using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCounterAttackState : PlayerState
{
    private const string IS_SUCCESSFUL_COUNTER_ATTACK = "IsSuccessfulCounterAttack";

    public PlayerCounterAttackState(Player player, PlayerStateMachine stateMachine, string animationBoolName) : base(player, stateMachine, animationBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();

        stateTimer = player.counterDuration;
        player.Animator.SetBool(IS_SUCCESSFUL_COUNTER_ATTACK, false);
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();

        player.SetZeroVelocity();

        Collider2D[] colliders = Physics2D.OverlapCircleAll(player.attackCheck.position, player.attackCheckRadius);

        foreach (Collider2D hit in colliders)
        {
            if (hit.GetComponent<Enemy>() != null)
            {
                if (hit.GetComponent<Enemy>().CanBeStunned())
                {
                    stateTimer = 10;
                    player.Animator.SetBool(IS_SUCCESSFUL_COUNTER_ATTACK, true);
                }
            }
        }

        if (stateTimer <= 0 || isTriggerCalled)
        {
            player.StateMachine.ChangeState(player.IdleState);
        }
    }
}
