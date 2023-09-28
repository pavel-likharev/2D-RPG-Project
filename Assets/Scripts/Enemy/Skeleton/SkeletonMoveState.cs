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

        enemySkeleton.SetVelocity(enemySkeleton.moveSpeed * enemySkeleton.MoveDir, rb.velocity.y);

        if (!enemySkeleton.IsGroundedDetected() || enemySkeleton.IsWallDetected())
        {
            enemySkeleton.FlipSprite();
        }

        if (stateTimer <= 0)
        {
            stateMachine.ChangeState(enemySkeleton.IdleState);
        }
    }
}
