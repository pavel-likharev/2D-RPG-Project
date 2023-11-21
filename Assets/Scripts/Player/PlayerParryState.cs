using UnityEngine;

public class PlayerParryState : PlayerState
{
    private const string IS_SUCCESSFUL_PARRY = "IsSuccessfulCounterAttack";

    private float animateWindow = 10;

    public PlayerParryState(Player player, PlayerStateMachine stateMachine, string animationBoolName) : base(player, stateMachine, animationBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();

        stateTimer = player.counterDuration;
        player.Animator.SetBool(IS_SUCCESSFUL_PARRY, false);
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();

        player.SetZeroVelocity();

        if (player.ParrySkill.CheckParry())
        {
            player.ParryState.stateTimer = animateWindow;
        }

        if (stateTimer <= 0 || isTriggerCalled)
        {
            player.StateMachine.ChangeState(player.IdleState);
        }
    }
}
