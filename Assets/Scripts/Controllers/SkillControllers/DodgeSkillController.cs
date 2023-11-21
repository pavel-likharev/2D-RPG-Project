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

        dodgeSkillSlot.GetComponent<Button>().onClick.AddListener(UnlockDodge);
        dodgeSkillSlot.SetPriceText(dodgePrice);

        dodgeWithCloneSkillSlot.GetComponent<Button>().onClick.AddListener(UnlockDodgeWithClone);
        dodgeWithCloneSkillSlot.SetPriceText(dodgeWithClonePrice);
    }

    private void UnlockDodge()
    {
        DodgeUnlocked = UnlockSkill(dodgeSkillSlot, dodgePrice);

        player.Stats.evasion.AddModifier(evasionValue);
        Inventory.Instance.UpdateStatSlotsUI();

        dodgeSkillSlot.GetComponent<Button>().enabled = false;
    }

    private void UnlockDodgeWithClone()
    {
        if (DodgeUnlocked)
        {
            DodgeWithCloneUnlocked = UnlockSkill(dodgeWithCloneSkillSlot, dodgeWithClonePrice);

            dodgeWithCloneSkillSlot.GetComponent<Button>().enabled = false;
        }
    }
}
