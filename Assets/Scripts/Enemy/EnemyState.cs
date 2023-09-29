using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyState
{
    protected EnemyStateMachine stateMachine;
    protected Enemy enemy;

    protected Rigidbody2D rb;

    private string animationBoolName;

    protected float stateTimer;
    protected bool isTriggerCalled;

    public EnemyState(Enemy enemy, EnemyStateMachine stateMachine, string animationBoolName)
    {
        this.enemy = enemy;
        this.stateMachine = stateMachine;
        this.animationBoolName = animationBoolName;
    }

    public virtual void Enter()
    {
        isTriggerCalled = false;

        rb = enemy.Rb;
        enemy.Animator.SetBool(animationBoolName, true);
    }

    public virtual void Update()
    {
        if (stateTimer > 0)
        {
            stateTimer -= Time.deltaTime;
        }
    }

    public virtual void Exit()
    {
        enemy.Animator.SetBool(animationBoolName, false);
    }

    public virtual void AnimationFinishTrigger()
    {
        isTriggerCalled = true;
    }
}
