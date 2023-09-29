using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonMoveState : SkeletonGroundedState
{
    public SkeletonMoveState(Enemy enemy, EnemyStateMachine stateMachine, string animationBoolName, EnemySkeleton enemySkeleton) : base(enemy, stateMachine, animationBoolName, enemySkeleton)
    {
    }

    public override void Enter()
    {
        base.Enter();

        stateTimer = 4f;
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();

        if (stateTimer <= 0)
        {
            stateMachine.ChangeState(enemySkeleton.IdleState);
        }

        enemySkeleton.SetVelocity(enemy.moveSpeed * enemySkeleton.MoveDir, rb.velocity.y);
    }
}
