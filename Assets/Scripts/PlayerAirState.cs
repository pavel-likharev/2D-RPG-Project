public class PlayerAirState : PlayerState
{
    public PlayerAirState(Player player, PlayerStateMachine stateMachine, string animationBoolName) : base(player, stateMachine, animationBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();


        if (rb.velocity.y < 0 && player.IsWallDetected())
        {
            stateMachine.ChangeState(player.WallSlideState);
        }

        if (xInput != 0)
        {
            player.SetVelocity(xInput * player.moveSpeed * 0.8f, rb.velocity.y);
        }

        if (player.IsGroundedDetected())
        {
            stateMachine.ChangeState(player.IdleState);
        }
    }
}
