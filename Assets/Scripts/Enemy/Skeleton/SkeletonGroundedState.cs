using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonGroundedState : EnemyState
{
    protected EnemySkeleton enemySkeleton;
    private Transform player;

    public SkeletonGroundedState(Enemy enemy, EnemyStateMachine stateMachine, string animationBoolName, EnemySkeleton enemySkeleton) : base(enemy, stateMachine, animationBoolName)
    {
        this.enemySkeleton = enemySkeleton;
    }

    public override void Enter()
    {
        base.Enter();

        enemySkeleton.SetZeroVelocity();

        player = PlayerManager.Instance.Player.transform;
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();

        if (!enemySkeleton.IsGroundedDetected() || enemySkeleton.IsWallDetected())
        {
            enemySkeleton.FlipSprite();
        }

        if (enemySkeleton.IsPlayerDetected() || Vector2.Distance(enemySkeleton.transform.position, player.position) < enemySkeleton.visibleDistance)
        {
            stateMachine.ChangeState(enemySkeleton.BattleState);
        }
    }
}
