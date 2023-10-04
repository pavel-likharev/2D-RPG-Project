using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCatchSwordState : PlayerState
{
    private Transform sword;
    public PlayerCatchSwordState(Player player, PlayerStateMachine stateMachine, string animationBoolName) : base(player, stateMachine, animationBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();

        sword = player.Sword.transform;

        if (player.transform.position.x > sword.position.x && player.MoveDir == 1)
        {
            player.FlipSprite();
        }
        else if (player.transform.position.x < sword.position.x && player.MoveDir == -1)
        {
            player.FlipSprite();
        }

        rb.velocity = new Vector2(player.returnSwordImpact * -player.MoveDir, player.transform.position.y);
    }

    public override void Exit()
    {
        base.Exit();

        player.StartCoroutine("BusyFor", 0.1f);
    }

    public override void Update()
    {
        base.Update();



        if (isTriggerCalled)
        {
            stateMachine.ChangeState(player.IdleState);
        }
    }
}
