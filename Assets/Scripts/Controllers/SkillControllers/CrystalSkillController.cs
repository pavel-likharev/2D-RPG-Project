using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CrystalSkillController : SkillController
{
    [SerializeField] private GameObject crystalPrefab;
    [SerializeField] private float crystalDuration;

    [Header("Crystal info")]
    [SerializeField] private float moveSpeed;
    [SerializeField] private UI_SkillTreeSlot crystalSlot;
    [SerializeField] private int crystalPrice;
    public bool CrystalUnlocked { get; private set; }

    [Header("Crystal -> CloneBlink")]
    [SerializeField] private UI_SkillTreeSlot cloneBlinkSlot;
    [SerializeField] private int cloneBlinkPrice;
    public bool CloneBlinkUnlocked { get; private set; }

    [Header("Explode")]
    [SerializeField] private UI_SkillTreeSlot explodeSlot;
    [SerializeField] private int explodePrice;
    public bool ExplodeUnlocked { get; private set; }

    [Header("Controlled Explode")]
    [SerializeField] private UI_SkillTreeSlot controlledExplodeSlot;
    [SerializeField] private int controlledExplodePrice;
    public bool ControlledExplodeUnlocked { get; private set; }

    [Header("Multi staking crystal")]
    [SerializeField] private int stackAmount;
    [SerializeField] private float multiStackCooldown;
    [SerializeField] private float useTimeWindow;
    [SerializeField] private UI_SkillTreeSlot multiStackSlot;
    [SerializeField] private int multiStackPrice;
    public bool MultiStackUnlocked { get; private set; }

    private List<GameObject> crystals = new List<GameObject>();
    private GameObject currentCrystal;

    protected override void Start()
    {
        base.Start();

        RefilCrystal();

        crystalSlot.GetComponent<Button>().onClick.AddListener(UnlockCrystalSkill);
        cloneBlinkSlot.GetComponent<Button>().onClick.AddListener(UnlockCloneBlinkSkill);
        explodeSlot.GetComponent<Button>().onClick.AddListener(UnlockExplodeSkill);
        controlledExplodeSlot.GetComponent<Button>().onClick.AddListener(UnlockControlledExplodeSkill);
        multiStackSlot.GetComponent<Button>().onClick.AddListener(UnlockMultiStackSkill);
    }

    private void UnlockCrystalSkill()
    {
        CrystalUnlocked = UnlockSkill(crystalSlot, crystalPrice);
    }

    private void UnlockCloneBlinkSkill()
    {
        if (CrystalUnlocked && !ExplodeUnlocked)
            CloneBlinkUnlocked = UnlockSkill(cloneBlinkSlot, cloneBlinkPrice);
    }

    private void UnlockExplodeSkill()
    {
        if (CrystalUnlocked && !CloneBlinkUnlocked)
            ExplodeUnlocked = UnlockSkill(explodeSlot, explodePrice);
    }

    private void UnlockControlledExplodeSkill()
    {
        if (ExplodeUnlocked)
            ControlledExplodeUnlocked = UnlockSkill(controlledExplodeSlot, controlledExplodePrice);
    }

    private void UnlockMultiStackSkill()
    {
        if (ControlledExplodeUnlocked)
            MultiStackUnlocked = UnlockSkill(multiStackSlot, multiStackPrice);
    }

    protected override void UseSkill()
    {
        base.UseSkill();

        if (CrystalUnlocked)
        {
            if (CanUseMultiCrystal())
            {
                return;
            }

            if (currentCrystal == null)
            {
                CreateCrystal();
            }
            else
            {
                if (ControlledExplodeUnlocked)
                    return;

                Vector2 playerPos = player.transform.position;
                player.transform.position = currentCrystal.transform.position;
                currentCrystal.transform.position = playerPos;

                if (CloneBlinkUnlocked)
                {
                    player.Skill.CloneSkillController.CreateClone(currentCrystal.transform, Vector3.zero);
                    Destroy(currentCrystal);
                }
                else
                {
                    currentCrystal.GetComponent<CrystalSkill>()?.FinishedCrystal();
                }

            }
        }
    }

    public void CreateCrystal()
    {
        currentCrystal = Instantiate(crystalPrefab, player.transform.position, Quaternion.identity);
        CrystalSkill crystalSkill = currentCrystal.GetComponent<CrystalSkill>();

        crystalSkill.SetupCrystal(crystalDuration, ExplodeUnlocked, ControlledExplodeUnlocked, moveSpeed, FindClosestEnemy(currentCrystal.transform));
    }

    public void CurrentCrystalChooseRandomTarget() => currentCrystal.GetComponent<CrystalSkill>().ChooseRandomEnemy();

    private bool CanUseMultiCrystal()
    {
        if (MultiStackUnlocked)
        {
            if (crystals.Count > 0)
            {
                if (crystals.Count == stackAmount)
                    Invoke("ResetAbility", useTimeWindow);
                

                cooldown = 0;
                GameObject spawnedCrystal = crystals[crystals.Count - 1];
                GameObject newCrystal = Instantiate(spawnedCrystal, player.transform.position, Quaternion.identity);

                crystals.Remove(spawnedCrystal);

                newCrystal.GetComponent<CrystalSkill>().
                    SetupCrystal(crystalDuration, ExplodeUnlocked, ControlledExplodeUnlocked, moveSpeed, FindClosestEnemy(newCrystal.transform));

                if (crystals.Count <= 0)
                {
                    cooldown = multiStackCooldown;
                    RefilCrystal();
                }

                return true;
            }
           
        }

        return false;
    }

    private void RefilCrystal()
    {
        int addAmount = stackAmount - crystals.Count;

        for (int i = 0; i < addAmount; i++)
        {
            crystals.Add(crystalPrefab);
        }
    }

    private void ResetAbility()
    {
        if (cooldownTimer > 0)
            return;

        cooldownTimer = multiStackCooldown;
        RefilCrystal();
    }
}
