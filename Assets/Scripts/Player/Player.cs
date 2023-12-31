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
    public PlayerParryState ParryState { get; private set; }
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

    public DashSkill DashSkill { get; private set; }
    public ParrySkill ParrySkill { get; private set; }
    public SkillManager Skill { get; private set; }
    public GameObject Sword { get; private set; }

    public PlayerFX FX { get; private set; }

    protected override void Awake()
    {
        base.Awake();

        StateMachine = new PlayerStateMachine();

        DashSkill = GetComponent<DashSkill>();
        ParrySkill = GetComponent<ParrySkill>();
        FX = GetComponentInChildren<PlayerFX>();

        IdleState = new PlayerIdleState(this, StateMachine, IS_IDLE);
        MoveState = new PlayerMoveState(this, StateMachine, IS_MOVE);
        AirState = new PlayerAirState(this, StateMachine, IS_JUMP);
        JumpState = new PlayerJumpState(this, StateMachine, IS_JUMP);
        DashState = new PlayerDashState(this, StateMachine, IS_DASH);
        DeadState = new PlayerDeadState(this, StateMachine, IS_DEAD);

        WallSlideState = new PlayerWallSlideState(this, StateMachine, IS_WALLSLIDE);
        WallJumpState = new PlayerWallJumpState(this, StateMachine, IS_JUMP);

        PrimaryAttackState = new PlayerPrimaryAttackState(this, StateMachine, IS_ATTACK);
        ParryState = new PlayerParryState(this, StateMachine, IS_COUNTER_ATTACK);

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
        DashSkill.defaultDashForce = DashSkill.dashForce;
    }

    protected override void Update()
    {
        if (GameManager.Instance.IsGamePause)
        {
            return;
        }

        base.Update();

        StateMachine.CurrentState.Update();
    }

    public override void SlowDownCharacter(float speedPercentage, float duration)
    {
        base.SlowDownCharacter(speedPercentage, duration);

        float value = 1 - speedPercentage;

        moveSpeed *= value;
        jumpForce *= value;
        DashSkill.dashForce *= value;

        Animator.speed *= value;

        Invoke("ReturnDefaultSpeed", duration);
    }

    protected override void ReturnDefaultSpeed()
    {
        base.ReturnDefaultSpeed();

        moveSpeed = defaultMoveSpeed;
        jumpForce = defaultJumpForce;
        DashSkill.dashForce = DashSkill.defaultDashForce;
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

        Skill.SwordSkillController.SetCooldown();
        UI.Instance.InGame.SetSwordCooldown();
    }

    public override void DamageImpact(int knockbackDir)
    {
        if (knockbackPower != Vector2.zero)
            base.DamageImpact(knockbackDir);
    }

    protected override void SetupZeroKnockbackPower()
    {
        knockbackPower = new Vector2(0, 0);
    }

    public void ExitBlackholeSkill()
    {
        StateMachine.ChangeState(AirState);
    }

    public void AnimationTrigger() => StateMachine.CurrentState.AnimationFinishTrigger();


}
