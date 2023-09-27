using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Player : MonoBehaviour
{
    #region Consts name states 
    private const string IS_IDLE = "IsIdle";
    private const string IS_MOVE = "IsMove";
    private const string IS_JUMP = "IsJump";
    private const string IS_DASH = "IsDash";
    private const string IS_WALLSLIDE = "IsWallSlide";
    private const string IS_ATTACK = "IsAttack";
    #endregion
    #region Components
    public Animator Animator { get; private set; }
    public Rigidbody2D Rb { get; private set; }
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
    public int MoveDir { get; private set; } = 1;
    private bool isMoveRight = true;

    [Header("Dash info")]
    [SerializeField] private float dashCooldown = 1f;
    private float dashCooldownTimer;
    public float dashForce = 12f;
    public float dashDuration = 0.3f;
    public float DashDir { get; private set; }    

    [Header("Collusion info")]
    [SerializeField] protected LayerMask groundLayer;
    [SerializeField] protected Transform groundCheck;
    [SerializeField] protected Transform wallCheck;
    [SerializeField] protected float groundCheckDistance = 1f;
    [SerializeField] protected float wallCheckDistance = 1f;

    private void Awake()
    {
        StateMachine = new PlayerStateMachine();
        IdleState = new PlayerIdleState(this, StateMachine, IS_IDLE);
        MoveState = new PlayerMoveState(this, StateMachine, IS_MOVE);
        AirState = new PlayerAirState(this, StateMachine, IS_JUMP);
        JumpState = new PlayerJumpState(this, StateMachine, IS_JUMP);
        DashState = new PlayerDashState(this, StateMachine, IS_DASH);
        WallSlideState = new PlayerWallSlideState(this, StateMachine, IS_WALLSLIDE);
        WallJumpState = new PlayerWallJumpState(this, StateMachine, IS_JUMP);
        PrimaryAttackState = new PlayerPrimaryAttackState(this, StateMachine, IS_ATTACK);

        Animator = GetComponentInChildren<Animator>();
        Rb = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        StateMachine.Initialize(IdleState);
    }

    private void Update()
    {
        StateMachine.CurrentState.Update();

        CheckTimers();
        CheckForDashInput();
    }

    public IEnumerator BusyFor(float seconds)
    {
        IsBusy = true;
        Debug.Log(IsBusy);

        yield return new WaitForSeconds(seconds);
        Debug.Log(IsBusy);
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

    #region Velocity
    public void SetVelocity(float xVelocity, float yVelocity)
    {
        Rb.velocity = new Vector2(xVelocity, yVelocity);
        FlipSpriteController(xVelocity);
    }

    public void ZeroVelocity() => Rb.velocity = new Vector2(0, 0);
    #endregion

    #region Flip
    public void FlipSprite()
    {
        MoveDir = -MoveDir;
        isMoveRight = !isMoveRight;
        transform.localScale = new Vector3(MoveDir, transform.localScale.y);
    }

    public void FlipSpriteController(float xDir)
    {
        if (xDir > 0 && !isMoveRight)
            FlipSprite();
        else if (xDir < 0 && isMoveRight) 
            FlipSprite();
    }
    #endregion

    #region Collision
    public bool IsGroundedDetected() => Physics2D.Raycast(groundCheck.position, Vector2.down, groundCheckDistance, groundLayer);
    public bool IsWallDetected() => Physics2D.Raycast(wallCheck.position, Vector2.right * MoveDir, wallCheckDistance, groundLayer);
    protected virtual void OnDrawGizmos()
    {
        Gizmos.DrawLine(groundCheck.position, new Vector3(groundCheck.position.x, groundCheck.position.y - groundCheckDistance));
        Gizmos.DrawLine(wallCheck.position, new Vector3(wallCheck.position.x + wallCheckDistance, wallCheck.position.y));
    }
    #endregion
}
