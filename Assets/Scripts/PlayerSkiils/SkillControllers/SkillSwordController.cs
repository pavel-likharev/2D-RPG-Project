using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillSwordController : MonoBehaviour
{
    private const string IS_ROTATION = "IsRotation";

    private Animator animator; 
    private Rigidbody2D rb;
    private CircleCollider2D circleCollider;
    private Player player;

    private bool canRotate = true;
    private bool isReturning;
    private float distanceToDestoy = 1f;
    private float returnSpeed;

    private float freezeDuration;

    [Header("Bounce info")]
    private float bouncingForce;
    private bool isBouncing;
    private int amountOfBounce;
    private List<Transform> enemiesInTarget;
    private float checkRadiusTarger = 10f;
    private int targetIndex;

    [Header("Pierce info")]
    private int pierceAmount;
    private bool isPiercing;

    [Header("Spin info")]
    [SerializeField] private float maxTravelDistance = 7f;
    [SerializeField] private float spinDuration = 2f;
    [SerializeField] private float spinGravity = 0.5f;
    private bool isSpinning;
    private bool wasStopSpin;
    private float spinTimer;
    private float spinRadiusDamage = 1;
    [SerializeField] private float hitCooldown = 0.35f;
    private float hitTimer;
    private float spinDir;
    private float spinMoveSpeed = 1.5f;



    private void Awake()
    {
        animator = GetComponentInChildren<Animator>();
        rb = GetComponent<Rigidbody2D>();
        circleCollider = GetComponent<CircleCollider2D>();
        player = PlayerManager.Instance.Player;
    }

    private void Update()
    {
        if (canRotate)
        {
            transform.right = rb.velocity;
        }

        if (isReturning)
        {
            transform.position = Vector2.MoveTowards(transform.position, player.transform.position, returnSpeed * Time.deltaTime);

            if (Vector2.Distance(transform.position, player.transform.position) < distanceToDestoy)
            {
                player.CatchTheSword();
            }
        }

        BounceLogic();
        SpinLogic();
    }

    public void SetupSword(Vector2 force, float gravityScale, Player player, float freezeDuration, float returnSpeed)
    {
        this.player = player;

        rb.velocity = force;
        rb.gravityScale = gravityScale;

        this.freezeDuration = freezeDuration;
        this.returnSpeed = returnSpeed;

        if (!isPiercing)
            animator.SetBool(IS_ROTATION, true);

        spinDir = Mathf.Clamp(rb.velocity.x, -1, 1);

        Invoke("Destroy", 7);
    }

    public void ReturnSword()
    {
        rb.constraints = RigidbodyConstraints2D.FreezeAll;
        transform.parent = null;

        isReturning = true;
    }

    private void BounceLogic()
    {
        if (isBouncing && enemiesInTarget.Count > 0 && !isReturning)
        {
            transform.position = Vector2.MoveTowards(transform.position, enemiesInTarget[targetIndex].position, bouncingForce * Time.deltaTime);

            if (Vector2.Distance(transform.position, enemiesInTarget[targetIndex].position) < 0.1f)
            {
                int knockbackDir = transform.position.x > enemiesInTarget[targetIndex].position.x ? -1 : 1;
                SkillSwordDamage(enemiesInTarget[targetIndex].GetComponent<Enemy>(), knockbackDir);

                
                targetIndex++;
                amountOfBounce--;

                if (amountOfBounce == 0)
                {
                    isBouncing = false;
                    isReturning = true;
                }

                if (targetIndex >= enemiesInTarget.Count)
                {
                    targetIndex = 0;
                }
            }
        }
    }

    private void SpinLogic()
    {
        if (isSpinning)
        {
            if (Vector2.Distance(player.transform.position, transform.position) > maxTravelDistance && !wasStopSpin)
            {
                StopSpinning();
            }

            if (wasStopSpin)
            {
                spinTimer -= Time.deltaTime;

                transform.position = Vector2.MoveTowards(transform.position, new Vector2(transform.position.x + spinDir, transform.position.y), spinMoveSpeed * Time.deltaTime);

                if (spinTimer <= 0)
                {
                    isReturning = true;
                    isSpinning = false;
                }

                hitTimer -= Time.deltaTime;

                if (hitTimer <= 0)
                {
                    hitTimer = hitCooldown;

                    Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, spinRadiusDamage);

                    foreach (Collider2D hit in colliders)
                    {
                        if (hit.GetComponent<Enemy>() != null)
                        {
                            SkillSwordDamage(hit.GetComponent<Enemy>(), player.MoveDir);
                        }
                    }
                }
            }
        }
    }

    private void StopSpinning()
    {
        wasStopSpin = true;
        rb.constraints = RigidbodyConstraints2D.FreezeAll;
        spinTimer = spinDuration;
    }

    public void SetupBouncing(bool isBouncing, int bounceAmount, float bouncingForce)
    {
        this.isBouncing = isBouncing;
        this.amountOfBounce = bounceAmount;
        this.bouncingForce = bouncingForce;

        enemiesInTarget = new List<Transform>();
    }

    public void SetupPiercing(bool isPiercing, int pierceAmount)
    {
        this.isPiercing = isPiercing;
        this.pierceAmount = pierceAmount;
    }

    public void SetupSpinning(bool isSpinning, float spinDuration, float maxTravelDistance, float hitCooldown)
    {
        this.isSpinning = isSpinning;
        this.spinDuration = spinDuration;
        this.maxTravelDistance = maxTravelDistance;
        this.hitCooldown = hitCooldown;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (isReturning)
            return;

        if (collision.GetComponent<Enemy>() != null)
        {
            Enemy enemy = collision.GetComponent<Enemy>();

            SkillSwordDamage(enemy, player.MoveDir);
        }

        SetupTargetForBounce(collision);

        StuckInto(collision);
    }

    private void SkillSwordDamage(Enemy enemy, int knockbackDir)
    {
        enemy.TakeDamage(knockbackDir);
        enemy.StartCoroutine("FreezeTimeFor", freezeDuration);
    }

    private void Destroy()
    {
        Destroy(gameObject);
    }

    private void SetupTargetForBounce(Collider2D collision)
    {
        if (collision.GetComponent<Enemy>() != null)
        {
            if (isBouncing && enemiesInTarget.Count <= 0)
            {
                Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, checkRadiusTarger);

                foreach (Collider2D hit in colliders)
                {
                    if (hit.GetComponent<Enemy>() != null)
                    {
                        enemiesInTarget.Add(hit.transform);
                    }
                }
            }
        }
    }

    private void StuckInto(Collider2D collision)
    {
        if (isPiercing && collision.GetComponent<Enemy>() != null)
        {
            pierceAmount--;

            if (pierceAmount == 0)
                isPiercing = false;

            return;
        }

        if (isSpinning)
        {
            StopSpinning();
            return;
        }

        canRotate = false;
        circleCollider.enabled = false;

        rb.isKinematic = true;
        rb.constraints = RigidbodyConstraints2D.FreezeAll;

        if (isBouncing && enemiesInTarget.Count > 0)
        {
            return;
        }

        animator.SetBool(IS_ROTATION, false);
        transform.parent = collision.transform;
    }
}
