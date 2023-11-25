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

        parryUnlockSlot.GetComponent<Button>().onClick.AddListener(UnlockParrySkill);
        parryUnlockSlot.SetPriceText(parryPrice);

        restoreOnParryUnlockSlot.GetComponent<Button>().onClick.AddListener(UnlockRestoreOnParrySkill);
        restoreOnParryUnlockSlot.SetPriceText(restoreOnParryPrice);

        cloneOnParryUnlockSlot.GetComponent<Button>().onClick.AddListener(UnlockCloneOnParrySkill);
        cloneOnParryUnlockSlot.SetPriceText(cloneOnParryPrice);
    }

    public override void CheckUnlockedSkills()
    {
        ParryUnlocked = parryUnlockSlot.Unlocked;
        RestoreOnParryUnlocked = restoreOnParryUnlockSlot.Unlocked;
        CloneOnParryUnlocked = cloneOnParryUnlockSlot.Unlocked;
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

    private void UnlockParrySkill()
    {
        ParryUnlocked = UnlockSkill(parryUnlockSlot, parryPrice);

        parryUnlockSlot.GetComponent<Button>().enabled = false;
    }

    private void UnlockRestoreOnParrySkill()
    {
        if (ParryUnlocked)
        {
            RestoreOnParryUnlocked = UnlockSkill(restoreOnParryUnlockSlot, restoreOnParryPrice);

            restoreOnParryUnlockSlot.GetComponent<Button>().enabled = false;
        }
    }

    private void UnlockCloneOnParrySkill()
    {
        if (RestoreOnParryUnlocked)
        {
            CloneOnParryUnlocked = UnlockSkill(cloneOnParryUnlockSlot, cloneOnParryPrice);

            cloneOnParryUnlockSlot.GetComponent<Button>().enabled = false;
        }
    }

    protected override void UseSkill()
    {
        base.UseSkill();

        if (ParryUnlocked)
        {
            player.StateMachine.ChangeState(player.ParryState);

            UI.Instance.InGame.SetParryCooldown();
        }
    }
}
