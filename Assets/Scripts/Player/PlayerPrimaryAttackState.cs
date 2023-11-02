using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPrimaryAttackState : PlayerState
{
    private const string COMBO_COUNTER = "ComboCounter";

    public int ComboCounter { get; private set; }
    private float lastTimeAttacked;
    private float comboWindow = 2f;

    public PlayerPrimaryAttackState(Player player, PlayerStateMachine stateMachine, string animationBoolName) : base(player, stateMachine, animationBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();

        if (ComboCounter > 2 || Time.time >= lastTimeAttacked + comboWindow)
        {
            ComboCounter = 0;
        }

        player.Animator.SetInteger(COMBO_COUNTER, ComboCounter);

        float attackDir = player.MoveDir;

        xInput = Input.GetAxisRaw("Horizontal");

        if (xInput != 0)
            attackDir = xInput;

        player.SetVelocity(player.attackMovements[ComboCounter].x * attackDir, player.attackMovements[ComboCounter].y);

        stateTimer = 0.1f;
    }

    public override void Exit()
    {
        base.Exit();

        player.StartCoroutine("BusyFor", 0.1f);

        ComboCounter++;
        lastTimeAttacked = Time.time;
    }

    public override void Update()
    {
        base.Update();

        if (stateTimer < 0)
        {
            player.SetZeroVelocity();
        }

        if (isTriggerCalled)
        {
            stateMachine.ChangeState(player.IdleState);
        }
    }
}
