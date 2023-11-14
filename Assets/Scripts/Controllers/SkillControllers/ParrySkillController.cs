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
        restoreOnParryUnlockSlot.GetComponent<Button>().onClick.AddListener(UnlockRestoreOnParrySkill);
        cloneOnParryUnlockSlot.GetComponent<Button>().onClick.AddListener(UnlockCloneOnParrySkill);
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
    }

    private void UnlockRestoreOnParrySkill()
    {
        if (ParryUnlocked)
            RestoreOnParryUnlocked = UnlockSkill(restoreOnParryUnlockSlot, restoreOnParryPrice);
    }

    private void UnlockCloneOnParrySkill()
    {
        if (RestoreOnParryUnlocked)
            CloneOnParryUnlocked = UnlockSkill(cloneOnParryUnlockSlot, cloneOnParryPrice);
    }

    protected override void UseSkill()
    {
        base.UseSkill();
        
        if (ParryUnlocked)
            player.StateMachine.ChangeState(player.ParryState);
    }
}
