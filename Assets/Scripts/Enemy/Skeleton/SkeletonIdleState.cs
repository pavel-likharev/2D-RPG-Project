using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonIdleState : SkeletonGroundedState
{
    public SkeletonIdleState(Enemy enemy, EnemyStateMachine stateMachine, string animationBoolName, EnemySkeleton enemySkeleton) : base(enemy, stateMachine, animationBoolName, enemySkeleton)
    {
    }

    public override void Enter()
    {
        base.Enter();

        enemySkeleton.ZeroVelocity();

        stateTimer = enemySkeleton.idleTime;
    }

    public override void Exit()
    {
        base.Exit();


    }

    public override void Update()
    {
        base.Update();

        if (stateTimer <= 0f)
        {
            stateMachine.ChangeState(enemySkeleton.MoveState);
        }
    }
}
