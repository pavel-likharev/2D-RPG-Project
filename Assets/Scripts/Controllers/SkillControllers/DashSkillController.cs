using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class DashSkillController : SkillController
{
    [Header("Dash")]
    [SerializeField] private int dashPrice;
    [SerializeField] private UI_SkillTreeSlot dashUnlockSlot;
    public bool DashUnlocked { get; private set; }

    [Header("Clone on dash")]
    [SerializeField] private int cloneOnDashPrice;
    [SerializeField] private UI_SkillTreeSlot cloneOnDashUnlockSlot;
    public bool CloneOnDashUnlocked { get; private set; }

    [Header("Clone on arrival")]
    [SerializeField] private int cloneOnArrivalPrice;
    [SerializeField] private UI_SkillTreeSlot cloneOnArrivalUnlockSlot;
    public bool CloneOnArrivalUnlocked { get; private set; }

    protected override void Start()
    {
        base.Start();

        dashUnlockSlot.GetComponent<Button>().onClick.AddListener(TryUnlockDash);
        dashUnlockSlot.SetPriceText(dashPrice);

        cloneOnDashUnlockSlot.GetComponent<Button>().onClick.AddListener(TryUnlockCloneOnDash);
        cloneOnDashUnlockSlot.SetPriceText(cloneOnDashPrice);

        cloneOnArrivalUnlockSlot.GetComponent<Button>().onClick.AddListener(TryUnlockCloneOnArrival);
        cloneOnArrivalUnlockSlot.SetPriceText(cloneOnArrivalPrice);
    }

    // Check skills
    public override void CheckUnlockedSkills()
    {
        if (dashUnlockSlot.Unlocked)
            UnlockDash();
        
        if (cloneOnDashUnlockSlot.Unlocked)
            UnlockCloneOnDash();

        if (cloneOnArrivalUnlockSlot.Unlocked)
            UnlockCloneOnArrival();
    }

    private void TryUnlockDash()
    {
        if (UnlockSkill(dashUnlockSlot, dashPrice, true))
            UnlockDash();
    }

    private void UnlockDash()
    {
        DashUnlocked = true;
    }

    private void TryUnlockCloneOnDash()
    {
        if (UnlockSkill(cloneOnDashUnlockSlot, cloneOnDashPrice, DashUnlocked))
            UnlockCloneOnDash();
    }

    private void UnlockCloneOnDash()
    {
        CloneOnDashUnlocked = true;
    }

    private void TryUnlockCloneOnArrival()
    {
        if (UnlockSkill(cloneOnArrivalUnlockSlot, cloneOnArrivalPrice ,CloneOnDashUnlocked))
            UnlockCloneOnArrival();
        
    }

    private void UnlockCloneOnArrival()
    {
        CloneOnArrivalUnlocked = true;
    }

    // Logic
    protected override void UseSkill()
    {
        base.UseSkill();

        if (DashUnlocked)
        {
            player.DashSkill.SetupDash();
            player.StateMachine.ChangeState(player.DashState);

            UI.Instance.InGame.SetDashCooldown();
        }
    }

    public void CloneOnDash()
    {
        if (CloneOnDashUnlocked)
        {
            SkillManager.Instance.CloneSkillController.CreateClone(player.transform, Vector2.zero);
        }
    }

    public void CloneOnArrival()
    {
        if (CloneOnArrivalUnlocked)
        {
            SkillManager.Instance.CloneSkillController.CreateClone(player.transform, Vector2.zero);
        }
    }
}
