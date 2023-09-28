using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonBattleState : EnemyState
{
    private EnemySkeleton enemySkeleton;

    private Transform player;
    private int moveDir;

    public SkeletonBattleState(Enemy enemy, EnemyStateMachine stateMachine, string animationBoolName, EnemySkeleton enemySkeleton) : base(enemy, stateMachine, animationBoolName)
    {
        this.enemySkeleton = enemySkeleton;
    }

    public override void Enter()
    {
        base.Enter();

        player = GameObject.Find("Player").gameObject.transform;
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();

        if (player.position.x > enemySkeleton.transform.position.x)
        {
            moveDir = 1;
        }
        else if (player.position.x < enemySkeleton.transform.position.x)
        {
            moveDir = -1;
        }

        enemySkeleton.SetVelocity(enemySkeleton.moveSpeed * moveDir, rb.velocity.y);
    }
}
