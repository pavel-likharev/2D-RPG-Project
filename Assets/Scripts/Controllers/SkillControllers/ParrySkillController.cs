using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using UnityEngine;
using UnityEngine.UI;

public class ParrySkillController : SkillController
{
    [Header("Parry")]
    [SerializeField] private int parryPrice;
    [SerializeField] private UI_SkillTreeSlot parryUnlockSlot;
    public bool ParryUnlocked { get; private set; }

    [Header("Parry with restore")]
    [Range(0f, 1f)]
    [SerializeField] private float restorePercent;
    [SerializeField] private int restoreOnParryPrice;
    [SerializeField] private UI_SkillTreeSlot restoreOnParryUnlockSlot;
    public bool RestoreOnParryUnlocked { get; private set ; }

    [Header("Parry with clone")]
    [SerializeField] private int cloneOnParryPrice;
    [SerializeField] private UI_SkillTreeSlot cloneOnParryUnlockSlot;
    public bool CloneOnParryUnlocked { get; private set; }

    protected override void Start()
    {
        base.Start();

        parryUnlockSlot.GetComponent<Button>().onClick.AddListener(TryUnlockParry);
        parryUnlockSlot.SetPriceText(parryPrice);

        restoreOnParryUnlockSlot.GetComponent<Button>().onClick.AddListener(TryUnlockRestoreOnParry);
        restoreOnParryUnlockSlot.SetPriceText(restoreOnParryPrice);

        cloneOnParryUnlockSlot.GetComponent<Button>().onClick.AddListener(TryUnlockCloneOnParry);
        cloneOnParryUnlockSlot.SetPriceText(cloneOnParryPrice);
    }

    public override void ResetSkillController()
    {
        ParryUnlocked = false;
        RestoreOnParryUnlocked = false;
        CloneOnParryUnlocked = false;
    }

    // Check skills
    public override void CheckUnlockedSkills()
    {
        if (parryUnlockSlot.Unlocked)
            UnlockParry();

        if (restoreOnParryUnlockSlot.Unlocked)
            UnlockRestoreOnParry();

        if (cloneOnParryUnlockSlot.Unlocked)
            UnlockCloneOnParry();
    }

    private void TryUnlockParry()
    {
        if (UnlockSkill(parryUnlockSlot, parryPrice, true))
            UnlockParry();

    }

    private void UnlockParry()
    {
        ParryUnlocked = true;
    }

    private void TryUnlockRestoreOnParry()
    {
        if (UnlockSkill(restoreOnParryUnlockSlot, restoreOnParryPrice, ParryUnlocked))
            UnlockRestoreOnParry();
    }

    private void UnlockRestoreOnParry()
    {
        RestoreOnParryUnlocked = true;
    }

    private void TryUnlockCloneOnParry()
    {
        if (UnlockSkill(cloneOnParryUnlockSlot, cloneOnParryPrice, RestoreOnParryUnlocked))
            UnlockCloneOnParry();
    }

    private void UnlockCloneOnParry()
    {
        CloneOnParryUnlocked = true;
    }

    // Logic
    protected override void UseSkill()
    {
        base.UseSkill();

        if (ParryUnlocked)
        {
            player.StateMachine.ChangeState(player.ParryState);

            UI.Instance.InGame.SetParryCooldown();
        }
    }

    public void AttemptRestoreHealth()
    {
        if (RestoreOnParryUnlocked)
            player.Stats.currentHealth += Mathf.RoundToInt(player.Stats.GetMaxHealthValue() * restorePercent);
    }

    public void AttemptCreateClone(Transform target)
    {
        if (CloneOnParryUnlocked)
            player.Skill.CloneSkillController.CreateCloneWithDelay(target);
    }
}
