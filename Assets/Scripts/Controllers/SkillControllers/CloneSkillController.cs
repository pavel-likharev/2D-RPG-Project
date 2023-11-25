using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CloneSkillController : SkillController
{
    [Header("Clone info")]
    [SerializeField] private GameObject clonePrefab;
    [SerializeField] private float cloneDuration;
    private float multiplierAttack;

    [Header("Clone Attack Skill")]
    [SerializeField] private UI_SkillTreeSlot cloneAttackSkillSlot;
    [SerializeField] private int cloneAttackPrice;
    private float defaultMultiplierAttack = 0.3f;
    public bool CloneAttackUnlocked { get; private set; }

    [Header("Aggresive Skill")]
    [SerializeField] private UI_SkillTreeSlot agressiveSkillSlot;
    [SerializeField] private int agressivePrice;
    private float agressiveMultiplierAttack = 0.8f;
    private bool canApplyHitEffect;
    public bool AgressiveUnlocked { get; private set; }

    [Header("Diplicate info")]
    [SerializeField] private UI_SkillTreeSlot duplicateSkillSlot;
    [SerializeField] private int duplicatePrice;
    [SerializeField] private float chanceDuplicate;
    public bool DuplicateUnlocked { get; private set; }

    [Header("Crystal clone info")]
    [SerializeField] private UI_SkillTreeSlot crystalCloneSkillSlot;
    [SerializeField] private int crystalClonePrice;
    public bool CrystalCloneUnlocked { get; private set; }

    [Space]

    private float xOffset = 2f;
    private float createDelay = 0.4f;

    protected override void Start()
    {
        base.Start();

        cloneAttackSkillSlot.GetComponent<Button>().onClick.AddListener(UnlockCloneAttack);
        cloneAttackSkillSlot.SetPriceText(cloneAttackPrice);

        agressiveSkillSlot.GetComponent<Button>().onClick.AddListener(UnlockAgressive);
        agressiveSkillSlot.SetPriceText(agressivePrice);
        
        duplicateSkillSlot.GetComponent<Button>().onClick.AddListener(UnlockDuplicate);
        duplicateSkillSlot.SetPriceText(duplicatePrice);
        
        crystalCloneSkillSlot.GetComponent<Button>().onClick.AddListener(UnlockCrystalClone);
        crystalCloneSkillSlot.SetPriceText(crystalClonePrice);
    }

    public override void CheckUnlockedSkills()
    {
        CloneAttackUnlocked = cloneAttackSkillSlot.Unlocked;
        AgressiveUnlocked = agressiveSkillSlot.Unlocked;
        DuplicateUnlocked = duplicateSkillSlot.Unlocked;
        CrystalCloneUnlocked = crystalCloneSkillSlot.Unlocked;
    }

    private void UnlockCloneAttack()
    {
        CloneAttackUnlocked = UnlockSkill(cloneAttackSkillSlot, cloneAttackPrice);
        multiplierAttack = defaultMultiplierAttack;

        cloneAttackSkillSlot.GetComponent<Button>().enabled = false;
    }

    private void UnlockAgressive()
    {
        if (CloneAttackUnlocked)
        {
            AgressiveUnlocked = UnlockSkill(agressiveSkillSlot, agressivePrice);
            canApplyHitEffect = true;
            multiplierAttack = agressiveMultiplierAttack;

            agressiveSkillSlot.GetComponent<Button>().enabled = false;
        }
    }

    private void UnlockDuplicate()
    {
        if (AgressiveUnlocked && !CrystalCloneUnlocked)
        {
            DuplicateUnlocked = UnlockSkill(duplicateSkillSlot, duplicatePrice);
            multiplierAttack = defaultMultiplierAttack;

            duplicateSkillSlot.GetComponent<Button>().enabled = false;
            crystalCloneSkillSlot.GetComponent<Button>().enabled = false;
        }

    }

    private void UnlockCrystalClone()
    {
        if (AgressiveUnlocked && !DuplicateUnlocked)
        {
            CrystalCloneUnlocked = UnlockSkill(crystalCloneSkillSlot, crystalClonePrice);

            duplicateSkillSlot.GetComponent<Button>().enabled = false;
            crystalCloneSkillSlot.GetComponent<Button>().enabled = false;
        }
    }

    public void CreateClone(Transform clonePosition, Vector3 offset, float optionalMultiplierAttack = 1)
    {
        float currentMultiplier;

        if (optionalMultiplierAttack != 1)
            currentMultiplier = optionalMultiplierAttack;
        else
            currentMultiplier = multiplierAttack;
        

        if (CrystalCloneUnlocked)
        {
            SkillManager.Instance.CrystalSkillController.CreateCrystal();
            return;
        }

        GameObject newClone = Instantiate(clonePrefab, clonePosition.position + offset, Quaternion.identity);

        newClone.GetComponent<CloneSkill>().SetupClone(cloneDuration, CloneAttackUnlocked, currentMultiplier, canApplyHitEffect, FindClosestEnemy(newClone.transform), DuplicateUnlocked, chanceDuplicate);
    }

    public void CreateCloneWithDelay(Transform target)
    {
        StartCoroutine(CreateCloneFor(target, new Vector3(xOffset * player.MoveDir, 0)));
    }

    private IEnumerator CreateCloneFor(Transform enemy, Vector3 offset)
    {
        yield return new WaitForSeconds(createDelay);
            CreateClone(enemy.transform, offset);
    }
}
