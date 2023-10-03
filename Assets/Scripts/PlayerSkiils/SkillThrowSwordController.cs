using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillThrowSwordController : MonoBehaviour
{
    private const string IS_ROTATION = "IsRotation";

    private Animator animator; 
    private Rigidbody2D rb;
    private CircleCollider2D circleCollider;
    private Player player;

    private bool canRotate = true;
    private bool isReturning;
    private float distanceToDestoy = 1;
    [SerializeField] private float returnSpeed = 20;

    private void Awake()
    {
        animator = GetComponentInChildren<Animator>();
        rb = GetComponent<Rigidbody2D>();
        circleCollider = GetComponent<CircleCollider2D>();
        player = PlayerManager.Instance.Player;
    }

    public void SetupSword(Vector2 dir, float gravityScale, Player player)
    {
        this.player = player;

        rb.velocity = dir;
        rb.gravityScale = gravityScale;

        animator.SetBool(IS_ROTATION, true);
    }

    public void ReturnSword()
    {
        rb.isKinematic = false;
        transform.parent = null;

        isReturning = true;
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
                player.ClearTheSword();
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        canRotate = false;
        circleCollider.enabled = false;

        rb.isKinematic = true;
        rb.constraints = RigidbodyConstraints2D.FreezeAll;

        transform.parent = collision.transform;

        animator.SetBool(IS_ROTATION, false);
    }
}
