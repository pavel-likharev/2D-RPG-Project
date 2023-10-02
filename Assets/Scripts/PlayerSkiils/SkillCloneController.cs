using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillCloneController : MonoBehaviour
{
    private const string ATTACK_NUMBER = "AttackNumber";

    private float cloneTimer;
    [SerializeField] private float colorLoosingSpeed = 1;
    private SpriteRenderer spriteRenderer;
    private Animator animator;

    [SerializeField] private Transform attackCheck;
    private float attackCheckRadius;
    private int moveDir = 1;

    private Transform closestEnemy;
    private float checkRadiusClosestEnemy = 25f;


    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();

    }

    private void Start()
    {
        attackCheckRadius = PlayerManager.Instance.Player.attackCheckRadius;
        MoveToClosestTarger();
    }

    private void Update()
    {
        if (cloneTimer >= 0) 
        {
            cloneTimer -= Time.deltaTime;
        }

        if (cloneTimer <= 0)
        {
            spriteRenderer.color = new Color(1, 1, 1, spriteRenderer.color.a - (Time.deltaTime * colorLoosingSpeed));
        }

        if (spriteRenderer.color.a <= 0)
        {
            Destroy(gameObject);
        }
    }

    public void SetupClone(Transform newTransform, float cloneDuration, bool isCanAttack)
    {
        transform.position = newTransform.position;
        cloneTimer = cloneDuration;

        if (isCanAttack)
        {
            animator.SetInteger(ATTACK_NUMBER, Random.Range(1, 3));
        }
    }

    private void AnimationTrigger()
    {
        cloneTimer = 0;
    }

    private void AttackTrigger()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(attackCheck.position, attackCheckRadius);

        foreach (Collider2D hit in colliders)
        {
            if (hit.GetComponent<Enemy>() != null)
            {
                hit.GetComponent<Enemy>().TakeDamage(moveDir);
            }
        }
    }

    private void MoveToClosestTarger()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, checkRadiusClosestEnemy);

        float closestDistance = Mathf.Infinity;

        foreach (Collider2D hit in colliders)
        {
            if (hit.GetComponent<Enemy>() != null)
            {
                float distanceToEnemy = Vector2.Distance(transform.position, hit.transform.position);

                if (distanceToEnemy < closestDistance)
                {
                    closestDistance = distanceToEnemy;
                    closestEnemy = hit.transform;
                }
            }
        }

        if (closestEnemy != null)
        {
            if (transform.position.x > closestEnemy.position.x)
            {
                moveDir = -moveDir;
            }
        }
        else
        {
            moveDir = PlayerManager.Instance.Player.MoveDir;
        }

        transform.localScale = new Vector3(moveDir, transform.localScale.y, transform.localScale.z);
    }
}
