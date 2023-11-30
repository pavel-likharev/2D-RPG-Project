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

        player = PlayerManager.Instance.Player.transform;

        if (player.GetComponent<PlayerStats>().IsDead)
            stateMachine.ChangeState(enemySkeleton.MoveState);



    }

    public override void Exit()
    {
        base.Exit();
        enemySkeleton.Animator.speed = 1;
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

        if (enemySkeleton.IsPlayerDetected())
        {
            stateTimer = enemySkeleton.battleTime;

            if (enemySkeleton.IsPlayerDetected().distance < enemySkeleton.attackDistance)
            {
                enemySkeleton.Animator.speed = 0;
                if (CanAttack())
                {
                    stateMachine.ChangeState(enemySkeleton.AttackState);
                }
            }
            else
            {
                enemySkeleton.Animator.speed = 1;
                enemySkeleton.SetVelocity(enemySkeleton.moveSpeed * moveDir, rb.velocity.y);
            }

        }
        else
        {
            if (stateTimer <= 0 || Vector2.Distance(enemySkeleton.transform.position, player.position) > enemySkeleton.agressiveDistance)
            {
                stateMachine.ChangeState(enemySkeleton.MoveState);
            }
            enemySkeleton.SetVelocity(enemySkeleton.moveSpeed * moveDir, rb.velocity.y);
        }

        
    }

    private bool CanAttack() => Time.time >= enemySkeleton.lastTimeAttack + enemySkeleton.attackCooldown;
    
}
