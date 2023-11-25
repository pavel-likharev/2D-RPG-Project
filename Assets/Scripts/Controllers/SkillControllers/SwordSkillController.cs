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

    public SwordType swordType;

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

    protected override void Awake()
    {
        base.Awake();
    }

    protected override void Start()
    {
        base.Start();

        GenerateDots();

        SetupGravity();

        throwSwordSlot.GetComponent<Button>().onClick.AddListener(UnlockThrowSword);
        throwSwordSlot.SetPriceText(throwSwordPrice);

        bouncySwordSlot.GetComponent<Button>().onClick.AddListener(UnlockBouncySword);
        bouncySwordSlot.SetPriceText(bouncySwordPrice);
        
        pierceSwordSlot.GetComponent<Button>().onClick.AddListener(UnlockPierceSword);
        pierceSwordSlot.SetPriceText(pierceSwordPrice);
        
        spinSwordSlot.GetComponent<Button>().onClick.AddListener(UnlockSpinSword);
        spinSwordSlot.SetPriceText(spinSwordPrice);

        timeStopSlot.GetComponent<Button>().onClick.AddListener(UnlockTimeStop);
        timeStopSlot.SetPriceText(timeStopPrice);
        
        vulnerableSlot.GetComponent<Button>().onClick.AddListener(UnlockVulnerable);
        vulnerableSlot.SetPriceText(vulnerablePrice);
        }

    public override void CheckUnlockedSkills()
    {
        ThrowSwordUnlocked = throwSwordSlot.Unlocked;
        BouncySwordUnlocked = bouncySwordSlot.Unlocked;
        PierceSwordUnlocked = pierceSwordSlot.Unlocked;
        SpinSwordUnlocked = spinSwordSlot.Unlocked;
        TimeStopUnlocked = timeStopSlot.Unlocked;
        VulnerableUnlocked = vulnerableSlot.Unlocked;
    }
    #region Unlock Skills
    private void UnlockThrowSword()
    {
        ThrowSwordUnlocked = UnlockSkill(throwSwordSlot, throwSwordPrice);

        throwSwordSlot.GetComponent<Button>().enabled = false;
    }
    
    private void UnlockBouncySword()
    {
        if (ThrowSwordUnlocked && !PierceSwordUnlocked && !SpinSwordUnlocked)
        {
            BouncySwordUnlocked = UnlockSkill(bouncySwordSlot, bouncySwordPrice);
            swordType = SwordType.Bounce;

            DisableButtons();
        }
    }

    private void UnlockPierceSword()
    {
        if (ThrowSwordUnlocked && !BouncySwordUnlocked && !SpinSwordUnlocked)
        {
            PierceSwordUnlocked = UnlockSkill(pierceSwordSlot, pierceSwordPrice);
            swordType = SwordType.Pierce;

            DisableButtons();
        }
    }

    private void UnlockSpinSword()
    {
        if (ThrowSwordUnlocked && !BouncySwordUnlocked && !PierceSwordUnlocked)
        {
            SpinSwordUnlocked = UnlockSkill(spinSwordSlot, spinSwordPrice);
            swordType = SwordType.Spin;

            DisableButtons();
        }
    }

    private void UnlockTimeStop()
    {
        if (ThrowSwordUnlocked)
        {
            TimeStopUnlocked = UnlockSkill(timeStopSlot, timeStopPrice);

            timeStopSlot.GetComponent<Button>().enabled = false;
        }
    }

    private void UnlockVulnerable()
    {
        if (TimeStopUnlocked)
        {
            VulnerableUnlocked = UnlockSkill(vulnerableSlot, vulnerablePrice);

            vulnerableSlot.GetComponent<Button>().enabled = false;
        }
    }

    private void DisableButtons()
    {
        bouncySwordSlot.GetComponent<Button>().enabled = false;
        pierceSwordSlot.GetComponent<Button>().enabled = false;
        spinSwordSlot.GetComponent<Button>().enabled = false;
    }
    #endregion

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

    public void SetCooldown()
    {
        cooldownTimer = cooldown;
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
        SwordSkill skill = newSword.GetComponent<SwordSkill>();

        if (swordType == SwordType.Bounce)
        {
            skill.SetupBouncing(true, bounceAmount, bouncingForce);
        }
        else if (swordType == SwordType.Pierce)
        {
            skill.SetupPiercing(true, pierceAmount);
        }
        else if (swordType == SwordType.Spin)
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
