using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DodgeSkillController : SkillController
{
    [SerializeField] private UI_SkillTreeSlot dodgeSkillSlot;
    [SerializeField] private int dodgePrice;
    [SerializeField] private int evasionValue;
    public bool DodgeUnlocked { get; private set; }
    [Space]
    [SerializeField] private UI_SkillTreeSlot dodgeWithCloneSkillSlot;
    [SerializeField] private int dodgeWithClonePrice;
    public bool DodgeWithCloneUnlocked { get; private set; }

    protected override void Start()
    {
        base.Start();

        dodgeSkillSlot.GetComponent<Button>().onClick.AddListener(TryUnlockDodge);
        dodgeSkillSlot.SetPriceText(dodgePrice);

        dodgeWithCloneSkillSlot.GetComponent<Button>().onClick.AddListener(TryUnlockDodgeWithClone);
        dodgeWithCloneSkillSlot.SetPriceText(dodgeWithClonePrice);
    }

    public override void ResetSkillController()
    {
        DodgeUnlocked = false;
        DodgeWithCloneUnlocked = false;

        player.Stats.evasion.RemoveModifier(evasionValue);
        Inventory.Instance.UpdateStatSlotsUI();
    }

    // Check skills
    public override void CheckUnlockedSkills()
    {
        if (dodgeSkillSlot.Unlocked)
            UnlockDodge();
        if (dodgeWithCloneSkillSlot.Unlocked)
            UnlockDodgeWithClone();
    }

    private void TryUnlockDodge()
    {
        if (UnlockSkill(dodgeSkillSlot, dodgePrice, true))
            UnlockDodge();
    }

    private void UnlockDodge()
    {
        DodgeUnlocked = true;

        player.Stats.evasion.AddModifier(evasionValue);
        Inventory.Instance.UpdateStatSlotsUI();
    }

    private void TryUnlockDodgeWithClone()
    {
        if (UnlockSkill(dodgeWithCloneSkillSlot, dodgeWithClonePrice, DodgeUnlocked))
            UnlockDodgeWithClone();
    }

    private void UnlockDodgeWithClone()
    {
        DodgeWithCloneUnlocked = true;
    }
}
