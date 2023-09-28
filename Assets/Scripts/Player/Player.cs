using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Player : Character
{
    #region Consts name states 
    private const string IS_JUMP = "IsJump";
    private const string IS_DASH = "IsDash";
    private const string IS_WALLSLIDE = "IsWallSlide";
    #endregion

    #region States
    public PlayerStateMachine StateMachine { get; private set; }
    public PlayerIdleState IdleState { get; private set; }
    public PlayerMoveState MoveState { get; private set; }
    public PlayerAirState AirState { get; private set; }
    public PlayerJumpState JumpState { get; private set; }
    public PlayerDashState DashState { get; private set; }
    public PlayerWallSlideState WallSlideState { get; private set; }
    public PlayerWallJumpState WallJumpState { get; private set; }
    public PlayerPrimaryAttackState PrimaryAttackState { get; private set; }
    #endregion

    public bool IsBusy { get; private set; }

    [Header("Attack info")]
    public Vector2[] attackMovements;

    [Header("Move info")]
    public float moveSpeed = 8f;
    public float jumpForce = 15f;
    public float wallJumpForce = 5f;

    [Header("Dash info")]
    [SerializeField] private float dashCooldown = 1f;
    private float dashCooldownTimer;
    public float dashForce = 12f;
    public float dashDuration = 0.3f;
    public float DashDir { get; private set; }    



    protected override void Awake()
    {
        base.Awake();

        StateMachine = new PlayerStateMachine();
        IdleState = new PlayerIdleState(this, StateMachine, IS_IDLE);
        MoveState = new PlayerMoveState(this, StateMachine, IS_MOVE);
        AirState = new PlayerAirState(this, StateMachine, IS_JUMP);
        JumpState = new PlayerJumpState(this, StateMachine, IS_JUMP);
        DashState = new PlayerDashState(this, StateMachine, IS_DASH);
        WallSlideState = new PlayerWallSlideState(this, StateMachine, IS_WALLSLIDE);
        WallJumpState = new PlayerWallJumpState(this, StateMachine, IS_JUMP);
        PrimaryAttackState = new PlayerPrimaryAttackState(this, StateMachine, IS_ATTACK);

    }

    protected override void Start()
    {
        base.Start();

        StateMachine.Initialize(IdleState);
    }

    protected override void Update()
    {
        base.Update();

        StateMachine.CurrentState.Update();

        CheckTimers();
        CheckForDashInput();
    }

    public IEnumerator BusyFor(float seconds)
    {
        IsBusy = true;

        yield return new WaitForSeconds(seconds);

        IsBusy = false;
    }
    private void CheckTimers()
    {
        if (dashCooldownTimer > 0)
        {
            dashCooldownTimer -= Time.deltaTime;
        }
    }

    public void AnimationTrigger() => StateMachine.CurrentState.AnimationFinishTrigger();

    private void CheckForDashInput()
    {
        if (IsWallDetected())
            return;

        if (Input.GetKeyDown(KeyCode.LeftShift) && dashCooldownTimer <= 0)
        {
            DashDir = Input.GetAxisRaw("Horizontal");

            if (DashDir == 0)
                DashDir = MoveDir;

            StateMachine.ChangeState(DashState);
            dashCooldownTimer = dashCooldown;
        }
    }





    
}
