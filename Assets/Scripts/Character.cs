using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Character : MonoBehaviour
{
    #region Consts name states 
    protected const string IS_IDLE = "IsIdle";
    protected const string IS_MOVE = "IsMove";
    protected const string IS_ATTACK = "IsAttack";
    #endregion

    #region Components
    public Animator Animator { get; private set; }
    public Rigidbody2D Rb { get; private set; }

    public CharacterFX CharacterFX { get; private set; }
    #endregion

    public int MoveDir { get; private set; } = 1;
    private bool isMoveRight = true;

    [Header("Collusion info")]
    [SerializeField] protected Transform groundCheck;
    [SerializeField] protected float groundCheckDistance = 1f;
    [SerializeField] protected Transform wallCheck;
    [SerializeField] protected float wallCheckDistance = 1f;
    protected LayerMask groundLayer;

    [Header("Attack info")]
    public Transform attackCheck;
    public float attackCheckRadius;

    protected virtual void Awake()
    {
        Animator = GetComponentInChildren<Animator>();
        Rb = GetComponent<Rigidbody2D>();
        CharacterFX = GetComponentInChildren<CharacterFX>();

        groundLayer = LayerMask.GetMask("Ground");
    }

    protected virtual void Start()
    {
  
    }

    protected virtual void Update()
    {
        
    }

    public void TakeDamage()
    {
        Debug.Log(this + " was damage");
        CharacterFX.StartCoroutine("HitFX");
    }

    #region Collision
    public bool IsGroundedDetected() => Physics2D.Raycast(groundCheck.position, Vector2.down, groundCheckDistance, groundLayer);
    public bool IsWallDetected() => Physics2D.Raycast(wallCheck.position, Vector2.right * MoveDir, wallCheckDistance, groundLayer);
    protected virtual void OnDrawGizmos()
    {
        Gizmos.DrawLine(groundCheck.position, new Vector3(groundCheck.position.x, groundCheck.position.y - groundCheckDistance));
        Gizmos.DrawLine(wallCheck.position, new Vector3(wallCheck.position.x + wallCheckDistance * MoveDir, wallCheck.position.y));
        Gizmos.DrawWireSphere(attackCheck.position, attackCheckRadius);
    }
    #endregion

    #region Flip
    public void FlipSprite()
    {       
        MoveDir = -MoveDir;
        isMoveRight = !isMoveRight;
        transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
    }

    public void FlipSpriteController(float xDir)
    {
        if (xDir > 0 && !isMoveRight)
            FlipSprite();
        else if (xDir < 0 && isMoveRight)
            FlipSprite();
    }
    #endregion

    #region Velocity
    public void SetVelocity(float xVelocity, float yVelocity)
    {
        Rb.velocity = new Vector2(xVelocity, yVelocity);
        FlipSpriteController(xVelocity);
    }

    public void SetZeroVelocity() => Rb.velocity = new Vector2(0, 0);
    #endregion
}
