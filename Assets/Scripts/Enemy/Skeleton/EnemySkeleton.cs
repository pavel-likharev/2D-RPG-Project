using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySkeleton : Enemy
{
    #region States
    public SkeletonIdleState IdleState { get; private set; }
    public SkeletonMoveState MoveState { get; private set; }
    public SkeletonBattleState BattleState { get; private set; }
    public SkeletonAttackState AttackState { get; private set; }
    public SkeletonStunnedState StunnedState { get; private set; }
    public SkeletonDeadState DeadState { get; private set; }
    #endregion

    public EnemyFX FX { get; private set; } 

    protected override void Awake()
    {
        base.Awake();

        IdleState = new SkeletonIdleState(this, StateMachine, IS_IDLE, this);
        MoveState = new SkeletonMoveState(this, StateMachine, IS_MOVE, this);
        BattleState = new SkeletonBattleState(this, StateMachine, IS_MOVE, this);
        AttackState = new SkeletonAttackState(this, StateMachine, IS_ATTACK, this);
        StunnedState = new SkeletonStunnedState(this, StateMachine, IS_STUNNED, this);
        DeadState = new SkeletonDeadState(this, StateMachine, IS_DEAD, this);

        FX = GetComponentInChildren<EnemyFX>();
    }

    protected override void Start()
    {
        base.Start();

        idleTime = 2f;

        StateMachine.Initialize(IdleState);
    }

    protected override void Update()
    {
        base.Update();


        if (Input.GetKeyDown(KeyCode.Z))
        {
            StateMachine.ChangeState(StunnedState);
        }
    }

    public override bool CanBeStunned()
    {
        if (base.CanBeStunned())
        {
            StateMachine.ChangeState(StunnedState);
            return true;
        }

        return false;
    }

    public override void Die()
    {
        base.Die();
        StateMachine.ChangeState(DeadState);
    }
}
