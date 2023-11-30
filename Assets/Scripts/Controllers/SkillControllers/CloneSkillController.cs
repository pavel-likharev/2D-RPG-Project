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

        cloneAttackSkillSlot.GetComponent<Button>().onClick.AddListener(TryUnlockCloneAttack);
        cloneAttackSkillSlot.SetPriceText(cloneAttackPrice);

        agressiveSkillSlot.GetComponent<Button>().onClick.AddListener(TryUnlockAgressive);
        agressiveSkillSlot.SetPriceText(agressivePrice);
        
        duplicateSkillSlot.GetComponent<Button>().onClick.AddListener(TryUnlockDuplicate);
        duplicateSkillSlot.SetPriceText(duplicatePrice);
        
        crystalCloneSkillSlot.GetComponent<Button>().onClick.AddListener(TryUnlockCrystalClone);
        crystalCloneSkillSlot.SetPriceText(crystalClonePrice);
    }

    // Check skills
    public override void CheckUnlockedSkills()
    {
        if (cloneAttackSkillSlot.Unlocked)
            UnlockCloneAttack();
        
        if (agressiveSkillSlot.Unlocked)
            UnlockAgressive();
        
        if (duplicateSkillSlot.Unlocked)
            UnlockDuplicate();
        
        if (crystalCloneSkillSlot.Unlocked)
            UnlockCrystalCLone();
    }

    private void TryUnlockCloneAttack()
    {
        if (UnlockSkill(cloneAttackSkillSlot, cloneAttackPrice, true))
            UnlockCloneAttack();
    }

    private void UnlockCloneAttack()
    {
        CloneAttackUnlocked = true;
        multiplierAttack = defaultMultiplierAttack;
    }

    private void TryUnlockAgressive()
    {
        if (UnlockSkill(agressiveSkillSlot, agressivePrice, CloneAttackUnlocked))
            UnlockAgressive();
    }

    private void UnlockAgressive()
    {
        AgressiveUnlocked = true;
        canApplyHitEffect = true;
        multiplierAttack = agressiveMultiplierAttack;
    }

    private void TryUnlockDuplicate()
    {
        if (UnlockSkill(duplicateSkillSlot, duplicatePrice, (AgressiveUnlocked && !CrystalCloneUnlocked)))
        {
            UnlockDuplicate();
        }

    }

    private void UnlockDuplicate()
    {
        DuplicateUnlocked = true;
        multiplierAttack = defaultMultiplierAttack;
    }

    private void TryUnlockCrystalClone()
    {
        if (UnlockSkill(crystalCloneSkillSlot, crystalClonePrice, (AgressiveUnlocked && !DuplicateUnlocked)))
            UnlockCrystalCLone();
    }

    private void UnlockCrystalCLone()
    {
        CrystalCloneUnlocked = true;
    }

    // Logic
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
