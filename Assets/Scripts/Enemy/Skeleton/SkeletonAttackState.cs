using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonAttackState : EnemyState
{
    EnemySkeleton enemySkeleton;

    public SkeletonAttackState(Enemy enemy, EnemyStateMachine stateMachine, string animationBoolName, EnemySkeleton enemySkeleton) : base(enemy, stateMachine, animationBoolName)
    {
        this.enemySkeleton = enemySkeleton;
    }

    public override void Enter()
    {
        base.Enter();
    }

    public override void Exit()
    {
        base.Exit();

        enemySkeleton.lastTimeAttack = Time.time;
    }

    public override void Update()
    {
        base.Update();

        enemySkeleton.SetZeroVelocity();

        if (isTriggerCalled)
        {
            stateMachine.ChangeState(enemySkeleton.BattleState);
        }
    }
}
