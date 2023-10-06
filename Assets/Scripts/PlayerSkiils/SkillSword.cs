using UnityEngine;

public class SkillSword : Skill
{
    public enum SwordType
    {
        Regular,
        Bounce,
        Pierce,
        Spin
    }

    public SwordType swordType = SwordType.Regular;

    [Header("Skill info")]
    [SerializeField] private GameObject swordPrefab;
    [SerializeField] private Transform spawnPosition;
    [SerializeField] private Vector2 swordLaunchForce;
    [SerializeField] private float returnSpeed;
    [SerializeField] private float swordGravity;
    [SerializeField] private float freezeDuration;
    private Vector2 finalDir;

    [Header("Bounce info")]
    [SerializeField] private int bounceAmount;
    [SerializeField] private float bounceGravity;
    [SerializeField] private float bouncingForce;

    [Header("Pierce info")]
    [SerializeField] private int pierceAmount;
    [SerializeField] private float pierceGravity;

    [Header("Spin info")]
    [SerializeField] private float spinDuration;
    [SerializeField] private float spinGravity;
    [SerializeField] private float maxTravelDistance;
    [SerializeField] private float hitCooldown;



    [Header("Aim dots")]
    [SerializeField] private GameObject dotPrefab;
    [SerializeField] private Transform dotsParent;
    [SerializeField] private int numberOfDots;
    [SerializeField] private float spaceBetweenDots;
    private GameObject[] dots;

    protected override void Awake()
    {
        base.Awake();
    }

    protected override void Start()
    {
        base.Start();

        GenerateDots();

        SetupGravity();
    }

    protected override void Update()
    {
        base.Update();

        if (Input.GetKeyUp(KeyCode.Mouse1))
        {
            finalDir = new Vector2(AimDirection().normalized.x * swordLaunchForce.x, AimDirection().normalized.y * swordLaunchForce.y);
        }

        if (Input.GetKey(KeyCode.Mouse1))
        {
            for (int i = 0; i < numberOfDots; i++)
            {
                dots[i].transform.position = DotsPosition(i * spaceBetweenDots);
            }
        }
    }

    protected override void UseSkill()
    {
        base.UseSkill();

        CreateSword();
    }

    private void SetupGravity()
    {
        if (swordType == SwordType.Bounce)
        {
            swordGravity = bounceGravity;
        }
        else if (swordType == SwordType.Pierce)
        {
            swordGravity = pierceGravity;
        }
        else if (swordType == SwordType.Spin)
        {
            swordGravity = spinGravity;
        }
    }

    public void CreateSword()
    {
        GameObject newSword = Instantiate(swordPrefab, spawnPosition.position, transform.rotation);
        SkillSwordController skillController = newSword.GetComponent<SkillSwordController>();

        if (swordType == SwordType.Bounce)
        {
            skillController.SetupBouncing(true, bounceAmount, bouncingForce);
        }
        else if (swordType == SwordType.Pierce)
        {
            skillController.SetupPiercing(true, pierceAmount);
        }
        else if (swordType == SwordType.Spin)
        {
            skillController.SetupSpinning(true, spinDuration, maxTravelDistance, hitCooldown);
        }

        skillController.SetupSword(finalDir, swordGravity, player, freezeDuration, returnSpeed);
        player.AssignNewSword(newSword);

        DotsActive(false);
    }

    #region Aim
    public Vector2 AimDirection()
    {
        Vector2 playerPosition = player.transform.position;
        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 direction = mousePosition - playerPosition;

        return direction;
    }

    public void DotsActive(bool isActive)
    {
        foreach (GameObject dot in dots)
        {
            dot.SetActive(isActive);
        }
    }

    private void GenerateDots()
    {
        dots = new GameObject[numberOfDots];

        for (int i = 0; i < numberOfDots; i++)
        {
            dots[i] = Instantiate(dotPrefab, player.transform.position, Quaternion.identity, dotsParent);
            dots[i].SetActive(false);
        }
    }

    private Vector2 DotsPosition(float t)
    {
        Vector2 position = (Vector2)player.transform.position + new Vector2(
            AimDirection().normalized.x * swordLaunchForce.x, AimDirection().normalized.y * swordLaunchForce.y) *
            t + 0.5f * (Physics2D.gravity * swordGravity) * (t * t);

        return position;
    }
    #endregion
}
