using UnityEngine;

public class PlayerState
{
    private const string VELOCITY_Y = "Velocity.y";

    protected PlayerStateMachine stateMachine;
    protected Player player;
    protected Rigidbody2D rb;

    private string animationBoolName;
    protected float xInput;
    protected float yInput;
    protected float stateTimer;
    protected bool isTriggerCalled;

    public PlayerState(Player player, PlayerStateMachine stateMachine, string animationBoolName)
    {
        this.player = player;
        this.stateMachine = stateMachine;
        this.animationBoolName = animationBoolName;
    }

    public virtual void Enter()
    {
        player.Animator.SetBool(animationBoolName, true);
        rb = player.Rb;
        isTriggerCalled = false;
    }

    public virtual void Update()
    {
        if (stateTimer > 0)
        {
            stateTimer -= Time.deltaTime;
        }

        xInput = Input.GetAxisRaw("Horizontal");
        yInput = Input.GetAxisRaw("Vertical");

        player.Animator.SetFloat(VELOCITY_Y, rb.velocity.y);

        if (Input.GetKeyDown(KeyCode.LeftShift) && !player.IsWallDetected())
        {
            player.Skill.DashSkillController.CanUseSkill();
        }

        if (Input.GetKeyDown(KeyCode.F))
        {
            player.Skill.CrystalSkillController.CanUseSkill();
        }

        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            Inventory.Instance.UseFlask();
        }
    }

    public virtual void Exit()
    {
        player.Animator.SetBool(animationBoolName, false);
    }

    public virtual void AnimationFinishTrigger()
    {
        isTriggerCalled = true;
    }
}
