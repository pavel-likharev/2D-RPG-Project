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
    #endregion

    protected override void Awake()
    {
        base.Awake();

        IdleState = new SkeletonIdleState(this, StateMachine, IS_IDLE, this);
        MoveState = new SkeletonMoveState(this, StateMachine, IS_MOVE, this);
        BattleState = new SkeletonBattleState(this, StateMachine, IS_MOVE, this);
        AttackState = new SkeletonAttackState(this, StateMachine, IS_ATTACK, this);
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
    }
}
