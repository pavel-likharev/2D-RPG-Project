using UnityEngine;

public class CrystalSkill : MonoBehaviour
{
    private const string EXPLODE = "Explode";

    private Animator animator => GetComponent<Animator>();
    private CircleCollider2D circleCollider => GetComponent<CircleCollider2D>();

    private float crystalDuration;

    private bool canExplode;
    private bool canMove;
    private float moveSpeed;

    private bool canGrow;
    private float growSpeed = 5;
    private float growSize = 3;

    private Transform target;
    private float distanceTrigger = 1f;
    private int moveDir;

    [SerializeField] private LayerMask enemyLayer;

    public void SetupCrystal(float crystalDuration, bool canExplode, bool canMove, float moveSpeed, Transform target)
    {
        this.crystalDuration = crystalDuration;
        this.canExplode = canExplode;
        this.canMove = canMove;
        this.moveSpeed = moveSpeed;
        this.target = target;

        moveDir = GetMoveDir();
    }

    private void Update()
    {
        crystalDuration -= Time.deltaTime;

        if (crystalDuration < 0)
        {
            FinishedCrystal();
        }

        if (canGrow)
        {
            transform.localScale = Vector2.Lerp(transform.localScale, new Vector2(growSize, growSize), growSpeed * Time.deltaTime);
        }

        if (canMove)
        {
            if (target == null)
                return;

            transform.position = Vector2.MoveTowards(transform.position, target.position, moveSpeed * Time.deltaTime);

            if (Vector2.Distance(transform.position, target.position) < distanceTrigger)
            {
                FinishedCrystal();
                canMove = false;
            }
        }
    }

    public void ChooseRandomEnemy()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, SkillManager.Instance.BlackholeSkillController.GetBlackholeRadius(), enemyLayer);

        if (colliders.Length > 0)
            target = colliders[Random.Range(0, colliders.Length)].transform;
    }

    private void AnimationExplodeEvent()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, circleCollider.radius);

        foreach (Collider2D hit in colliders)
        {
            if (hit.GetComponent<Enemy>() != null)
            {
                PlayerManager.Instance.Player.Stats.DoMagicalDamage(hit.GetComponent<CharacterStats>(), moveDir);
            }
        }
    }

    public void FinishedCrystal()
    {
        if (canExplode)
        {
            canGrow = true;
            animator.SetTrigger(EXPLODE);
        }
        else
        {
            SelfDestroy();
        }
    }

    private int GetMoveDir()
    {
        if (target == null)
            return 0;

        if (transform.position.x < target.position.x)
            return 1;
        else
            return -1;
    }

    public void SelfDestroy() => Destroy(gameObject);

}
