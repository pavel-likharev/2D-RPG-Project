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
    public SpriteRenderer SpriteRenderer { get; private set; }
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

    [Header("Knockback info")]
    [SerializeField] protected Vector2 knockbackDirection;
    [SerializeField] protected float knockbackDuration = 0.07f;
    protected bool isKnocbacked;


    protected virtual void Awake()
    {
        Rb = GetComponent<Rigidbody2D>();
        Animator = GetComponentInChildren<Animator>();
        CharacterFX = GetComponentInChildren<CharacterFX>();
        SpriteRenderer = GetComponentInChildren<SpriteRenderer>();

        groundLayer = LayerMask.GetMask("Ground");
    }

    protected virtual void Start()
    {
  
    }

    protected virtual void Update()
    {
        
    }

    public void TakeDamage(int knockbackDir) // Dir => 1 = right, -1 = left, 0 = nothing
    {
        CharacterFX.StartCoroutine("HitFX");

        StartCoroutine("HitKnockback", knockbackDir);
    }

    protected virtual IEnumerator HitKnockback(int knockbackDir)
    {
        isKnocbacked = true;

        Debug.Log(knockbackDirection.x);
        Debug.Log(knockbackDir);
        Rb.velocity = new Vector2(knockbackDirection.x * knockbackDir, knockbackDirection.y);

        yield return new WaitForSeconds(knockbackDuration);

        isKnocbacked = false;
    }

    public void MakeTransparent(bool isTransparent)
    {
        if (isTransparent)
        {
            SpriteRenderer.color = Color.clear;
        }
        else
        {
            SpriteRenderer.color = Color.white;
        }
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
        if (isKnocbacked)
            return;

        Rb.velocity = new Vector2(xVelocity, yVelocity);
        FlipSpriteController(xVelocity);
    }

    public void SetZeroVelocity() 
    {
        if (isKnocbacked)
            return;

        Rb.velocity = new Vector2(0, 0); 
    }
    #endregion
}
