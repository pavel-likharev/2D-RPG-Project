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
    private const string IS_COUNTER_ATTACK = "IsCounterAttack";
    private const string IS_AIM_SWORD = "IsAimSword";
    private const string IS_CATCH_SWORD = "IsCatchSword";
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
    public PlayerCounterAttackState CounterAttackState { get; private set; }
    public PlayerAimSwordState AimSwordState { get; private set; }
    public PlayerCatchSwordState CatchSwordState { get; private set; }
    public PlayerBlackholeState BlackholeState { get; private set; }
    public PlayerDeadState DeadState { get; private set; }
    #endregion
    public bool IsBusy { get; private set; }

    [Header("Attack info")]
    public Vector2[] attackMovements;
    public float counterDuration = 0.5f;

    [Header("Move info")]
    public float moveSpeed = 8f;
    public float jumpForce = 15f;
    public float wallJumpForce = 5f;
    public float returnSwordImpact = 12f;
    private float defaultMoveSpeed;
    private float defaultJumpForce;

    [Header("Dash info")]
    public float dashForce = 12f;
    public float dashDuration = 0.3f;
    public float DashDir { get; private set; }
    private float defaultDashForce;

    public SkillManager Skill { get; private set; }
    public GameObject Sword { get; private set; }

    protected override void Awake()
    {
        base.Awake();

        StateMachine = new PlayerStateMachine();

        IdleState = new PlayerIdleState(this, StateMachine, IS_IDLE);
        MoveState = new PlayerMoveState(this, StateMachine, IS_MOVE);
        AirState = new PlayerAirState(this, StateMachine, IS_JUMP);
        JumpState = new PlayerJumpState(this, StateMachine, IS_JUMP);
        DashState = new PlayerDashState(this, StateMachine, IS_DASH);
        DeadState = new PlayerDeadState(this, StateMachine, IS_DEAD);

        WallSlideState = new PlayerWallSlideState(this, StateMachine, IS_WALLSLIDE);
        WallJumpState = new PlayerWallJumpState(this, StateMachine, IS_JUMP);

        PrimaryAttackState = new PlayerPrimaryAttackState(this, StateMachine, IS_ATTACK);
        CounterAttackState = new PlayerCounterAttackState(this, StateMachine, IS_COUNTER_ATTACK);

        AimSwordState = new PlayerAimSwordState(this, StateMachine, IS_AIM_SWORD);
        CatchSwordState = new PlayerCatchSwordState(this, StateMachine, IS_CATCH_SWORD);
        BlackholeState = new PlayerBlackholeState(this, StateMachine, IS_JUMP);
    }

    protected override void Start()
    {
        base.Start();

        StateMachine.Initialize(IdleState);

        Skill = SkillManager.Instance;

        defaultMoveSpeed = moveSpeed;
        defaultJumpForce = jumpForce;
        defaultDashForce = dashForce;
    }

    protected override void Update()
    {
        base.Update();

        StateMachine.CurrentState.Update();

        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            CheckForDashInput();
        }

        if (Input.GetKeyDown(KeyCode.F))
        {
            Skill.CrystalSkillController.CanUseSkill();
        }

        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            Inventory.Instance.UseFlask();
        }
    }

    public override void SlowDownCharacter(float speedPercentage, float duration)
    {
        base.SlowDownCharacter(speedPercentage, duration);

        float value = 1 - speedPercentage;

        moveSpeed *= value;
        jumpForce *= value;
        dashForce *= value;

        Animator.speed *= value;

        Invoke("ReturnDefaultSpeed", duration);
    }

    protected override void ReturnDefaultSpeed()
    {
        base.ReturnDefaultSpeed();

        moveSpeed = defaultMoveSpeed;
        jumpForce = defaultJumpForce;
        dashForce = defaultDashForce;
    }

    public override void Die()
    {
        base.Die();

        StateMachine.ChangeState(DeadState);
    }

    public IEnumerator BusyFor(float seconds)
    {
        IsBusy = true;

        yield return new WaitForSeconds(seconds);

        IsBusy = false;
    }

    public void AssignNewSword(GameObject newSword)
    {
        Sword = newSword;
    }

    public void CatchTheSword()
    {
        StateMachine.ChangeState(CatchSwordState);
        Destroy(Sword);
    }

    public void ExitBlackholeSkill()
    {
        StateMachine.ChangeState(AirState);
    }

    public void AnimationTrigger() => StateMachine.CurrentState.AnimationFinishTrigger();

    private void CheckForDashInput()
    {
        if (!Skill.DashSkillController.dashUnlocked || IsWallDetected())
            return;

        if (SkillManager.Instance.DashSkillController.CanUseSkill())
        {
            DashDir = Input.GetAxisRaw("Horizontal");

            if (DashDir == 0)
                DashDir = MoveDir;

            StateMachine.ChangeState(DashState);
        }
    }
}
