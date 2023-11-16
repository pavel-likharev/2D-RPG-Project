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

        dashUnlockSlot.GetComponent<Button>().onClick.AddListener(UnlockDashSkill);
        cloneOnDashUnlockSlot.GetComponent<Button>().onClick.AddListener(UnlockCloneOnDashSkill);
        cloneOnArrivalUnlockSlot.GetComponent<Button>().onClick.AddListener(UnlockCloneOnArrivalSkill);
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

    private void UnlockDashSkill()
    {
        DashUnlocked = UnlockSkill(dashUnlockSlot, dashPrice);

        dashUnlockSlot.GetComponent<Button>().enabled = false;
    }

    private void UnlockCloneOnDashSkill()
    {
        if (DashUnlocked)
        {
            CloneOnDashUnlocked = UnlockSkill(cloneOnDashUnlockSlot, cloneOnDashPrice);

            cloneOnDashUnlockSlot.GetComponent<Button>().enabled = false;
        }
    }

    private void UnlockCloneOnArrivalSkill()
    {
        if (CloneOnDashUnlocked)
        {
            CloneOnArrivalUnlocked = UnlockSkill(cloneOnArrivalUnlockSlot, cloneOnArrivalPrice);

            cloneOnArrivalUnlockSlot.GetComponent<Button>().enabled = false;
        }
    }

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
}
