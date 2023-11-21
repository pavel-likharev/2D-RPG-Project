using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloneSkill : MonoBehaviour
{
    private const string ATTACK_NUMBER = "AttackNumber";

    private float cloneTimer;
    [SerializeField] private float colorLoosingSpeed = 1;
    private SpriteRenderer spriteRenderer;
    private Animator animator;

    [SerializeField] private Transform attackCheck;
    private float attackCheckRadius;
    private int moveDir = 1;
    private float multiplierAttack;

    private bool canDuplicate;
    private float chanceDuplicate;
    private bool canApplyHitEffect;

    private Transform closestEnemy;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
    }

    private void Start()
    {
        attackCheckRadius = PlayerManager.Instance.Player.attackCheckRadius;
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
            //closestEnemy.GetComponentInChildren<SpriteRenderer>().color = Color.white;
            Destroy(gameObject);
        }
    }

    public void SetupClone(float cloneDuration, bool isCanAttack, float multiplierAttack, bool canApplyHitEffect, Transform closestEnemy, bool canDuplicate, float chanceDuplicate)
    {
        cloneTimer = cloneDuration;
        this.closestEnemy = closestEnemy;
        this.canDuplicate = canDuplicate;
        this.chanceDuplicate = chanceDuplicate;
        this.multiplierAttack = multiplierAttack;
        this.canApplyHitEffect = canApplyHitEffect;

        RotateToClosestTarger();

        if (isCanAttack)
        {
            animator.SetInteger(ATTACK_NUMBER, Random.Range(1, 3));
        }
    }

    #region Animation Setup
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
                PlayerManager.Instance.Player.Stats.DoDamage(hit.GetComponent<CharacterStats>(), moveDir, multiplierAttack);

                if (canApplyHitEffect)
                {
                    Inventory.Instance.GetEquipment(EquipmentType.Weapon)?.ApplyEffect(hit.transform);
                }

                if (canDuplicate)
                {
                    if (Random.Range(0, 100) < chanceDuplicate)
                    { 
                        SkillManager.Instance.CloneSkillController.CreateClone(hit.transform, new Vector2(moveDir, 0));
                    }
                }
            }

        }
    }
    #endregion

    private void RotateToClosestTarger()
    {
        if (closestEnemy != null)
        {
            if (transform.position.x >= closestEnemy.position.x)
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
