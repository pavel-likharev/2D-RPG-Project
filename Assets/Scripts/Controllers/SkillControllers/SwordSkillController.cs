using UnityEngine;
using UnityEngine.UI;

public class SwordSkillController : SkillController
{
    public enum SwordType
    {
        Regular,
        Bounce,
        Pierce,
        Spin
    }

    public SwordType Sword { get; private set; }

    [Header("Skill info")]
    [SerializeField] private GameObject swordPrefab;
    [SerializeField] private Transform spawnPosition;
    [SerializeField] private Vector2 swordLaunchForce;
    [SerializeField] private float returnSpeed;
    [SerializeField] private float swordGravity;
    [SerializeField] private float freezeDuration;
    private Vector2 finalDir;
    [Space]
    [SerializeField] private UI_SkillTreeSlot throwSwordSlot;
    [SerializeField] private int throwSwordPrice;
    public bool ThrowSwordUnlocked { get; private set; }

    [Header("Bounce info")]
    [SerializeField] private int bounceAmount;
    [SerializeField] private float bounceGravity;
    [SerializeField] private float bouncingForce;
    [Space]
    [SerializeField] private UI_SkillTreeSlot bouncySwordSlot;
    [SerializeField] private int bouncySwordPrice;
    public bool BouncySwordUnlocked { get; private set; }

    [Header("Pierce info")]
    [SerializeField] private int pierceAmount;
    [SerializeField] private float pierceGravity;
    [Space]
    [SerializeField] private UI_SkillTreeSlot pierceSwordSlot;
    [SerializeField] private int pierceSwordPrice;
    public bool PierceSwordUnlocked { get; private set; }

    [Header("Spin info")]
    [SerializeField] private float spinDuration;
    [SerializeField] private float spinGravity;
    [SerializeField] private float maxTravelDistance;
    [SerializeField] private float hitCooldown;
    [Space]
    [SerializeField] private UI_SkillTreeSlot spinSwordSlot;
    [SerializeField] private int spinSwordPrice;
    public bool SpinSwordUnlocked { get; private set; }

    [Header("Passive skills")]
    [SerializeField] private UI_SkillTreeSlot timeStopSlot;
    [SerializeField] private int timeStopPrice;
    public bool TimeStopUnlocked { get; private set; }
    [Space]
    [SerializeField] private UI_SkillTreeSlot vulnerableSlot;
    [SerializeField] private int vulnerablePrice;
    public bool VulnerableUnlocked { get; private set; }

    [Header("Aim dots")]
    [SerializeField] private GameObject dotPrefab;
    [SerializeField] private Transform dotsParent;
    [SerializeField] private int numberOfDots;
    [SerializeField] private float spaceBetweenDots;
    private GameObject[] dots;

    protected override void Start()
    {
        base.Start();

        GenerateDots();

        SetupGravity();

        throwSwordSlot.GetComponent<Button>().onClick.AddListener(TryUnlockThrowSword);
        throwSwordSlot.SetPriceText(throwSwordPrice);

        bouncySwordSlot.GetComponent<Button>().onClick.AddListener(TryUnlockBouncySword);
        bouncySwordSlot.SetPriceText(bouncySwordPrice);
        
        pierceSwordSlot.GetComponent<Button>().onClick.AddListener(TryUnlockPierceSword);
        pierceSwordSlot.SetPriceText(pierceSwordPrice);
        
        spinSwordSlot.GetComponent<Button>().onClick.AddListener(TryUnlockSpinSword);
        spinSwordSlot.SetPriceText(spinSwordPrice);

        timeStopSlot.GetComponent<Button>().onClick.AddListener(TryUnlockTimeStop);
        timeStopSlot.SetPriceText(timeStopPrice);
        
        vulnerableSlot.GetComponent<Button>().onClick.AddListener(TryUnlockVulnerable);
        vulnerableSlot.SetPriceText(vulnerablePrice);
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

    public override void ResetSkillController()
    {
        ThrowSwordUnlocked = false;
        BouncySwordUnlocked = false;
        PierceSwordUnlocked = false;
        SpinSwordUnlocked = false;
        TimeStopUnlocked = false;
        VulnerableUnlocked = false;

        Sword = SwordType.Regular;
    }

    // Check skills
    public override void CheckUnlockedSkills()
    {
        if (throwSwordSlot.Unlocked)
            UnlockThrowSword();

        if (bouncySwordSlot.Unlocked)
            UnlockBouncySword();

        if (pierceSwordSlot.Unlocked)
            UnlockPierceSword();

        if (spinSwordSlot.Unlocked)
            UnlockSpinSword();

        if (timeStopSlot.Unlocked)
            UnlockTimeStop();

        if (vulnerableSlot.Unlocked)
            UnlockVulnerable();
    }

    #region Unlock Skills
    private void TryUnlockThrowSword()
    {
        if (UnlockSkill(throwSwordSlot, throwSwordPrice, true))
            UnlockThrowSword();
    }

    private void UnlockThrowSword()
    {
        ThrowSwordUnlocked = true;
    }

    private void TryUnlockBouncySword()
    {
        if (UnlockSkill(bouncySwordSlot, bouncySwordPrice, ThrowSwordUnlocked && !PierceSwordUnlocked && !SpinSwordUnlocked))
            UnlockBouncySword();
    }

    private void UnlockBouncySword()
    {
        BouncySwordUnlocked = true;
        Sword = SwordType.Bounce;
    }

    private void TryUnlockPierceSword()
    {
        if (UnlockSkill(pierceSwordSlot, pierceSwordPrice, ThrowSwordUnlocked && !BouncySwordUnlocked && !SpinSwordUnlocked))
            UnlockPierceSword();
    }

    private void UnlockPierceSword()
    {
        PierceSwordUnlocked = true;
        Sword = SwordType.Pierce;
    }

    private void TryUnlockSpinSword()
    {
        if (UnlockSkill(spinSwordSlot, spinSwordPrice, ThrowSwordUnlocked && !BouncySwordUnlocked && !PierceSwordUnlocked))
            UnlockSpinSword();
    }

    private void UnlockSpinSword()
    {
        SpinSwordUnlocked = true;
        Sword = SwordType.Spin;
    }

    private void TryUnlockTimeStop()
    {
        if (UnlockSkill(timeStopSlot, timeStopPrice, ThrowSwordUnlocked))
            UnlockTimeStop();
    }

    private void UnlockTimeStop()
    {
        TimeStopUnlocked = true;
    }

    private void TryUnlockVulnerable()
    {
        if (UnlockSkill(vulnerableSlot, vulnerablePrice, TimeStopUnlocked))
            UnlockVulnerable();
    }

    private void UnlockVulnerable()
    {
        VulnerableUnlocked = true;
    }

    #endregion


    // Logic
    protected override void UseSkill()
    {
        base.UseSkill();

        CreateSword();
    }

    public void SetCooldown()
    {
        cooldownTimer = cooldown;
    }

    private void SetupGravity()
    {
        if (Sword == SwordType.Bounce)
        {
            swordGravity = bounceGravity;
        }
        else if (Sword == SwordType.Pierce)
        {
            swordGravity = pierceGravity;
        }
        else if (Sword == SwordType.Spin)
        {
            swordGravity = spinGravity;
        }
    }

    public void CreateSword()
    {
        GameObject newSword = Instantiate(swordPrefab, spawnPosition.position, transform.rotation);
        SwordSkill skill = newSword.GetComponent<SwordSkill>();

        if (Sword == SwordType.Bounce)
        {
            skill.SetupBouncing(true, bounceAmount, bouncingForce);
        }
        else if (Sword == SwordType.Pierce)
        {
            skill.SetupPiercing(true, pierceAmount);
        }
        else if (Sword == SwordType.Spin)
        {
            skill.SetupSpinning(true, spinDuration, maxTravelDistance, hitCooldown);
        }

        skill.SetupSword(finalDir, swordGravity, player, freezeDuration, returnSpeed);
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
