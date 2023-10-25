using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonStunnedState : EnemyState
{
    EnemySkeleton enemySkeleton;
    Player player;

    public SkeletonStunnedState(Enemy enemy, EnemyStateMachine stateMachine, string animationBoolName, EnemySkeleton enemySkeleton) : base(enemy, stateMachine, animationBoolName)
    {
        this.enemySkeleton = enemySkeleton;
        
    }

    public override void Enter()
    {
        base.Enter();

        player = PlayerManager.Instance.Player;

        stateTimer = enemySkeleton.stunDuration;

        rb.velocity = new Vector2(enemySkeleton.stunDir.x * player.MoveDir, enemySkeleton.stunDir.y);

        enemySkeleton.CharacterFX.InvokeRepeating("RedColorBlink", 0, 0.1f);
    }

    public override void Exit()
    {
        base.Exit();

        enemySkeleton.CharacterFX.Invoke("CancelColor", 0);
    }

    public override void Update()
    {
        base.Update();

        if (stateTimer <= 0)
        {
            stateMachine.ChangeState(enemySkeleton.IdleState);
        }
    }
}
