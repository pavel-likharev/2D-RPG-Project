using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class DashSkillController : SkillController
{
    [Header("Dash")]
    public bool dashUnlocked;
    [SerializeField] private int dashPrice;
    [SerializeField] private UI_SkillTreeSlot dashUnlockSlot;

    [Header("Clone on dash")]
    public bool cloneOnDashUnlocked;
    [SerializeField] private int cloneOnDashPrice;
    [SerializeField] private UI_SkillTreeSlot cloneOnDashUnlockSlot;

    [Header("Clone on arrival")]
    public bool cloneOnArrivalUnlocked;
    [SerializeField] private int cloneOnArrivalPrice;
    [SerializeField] private UI_SkillTreeSlot cloneOnArrivalUnlockSlot;

    protected override void Start()
    {
        base.Start();

        dashUnlockSlot.GetComponent<Button>().onClick.AddListener(UnlockDashSkill);
        cloneOnDashUnlockSlot.GetComponent<Button>().onClick.AddListener(UnlockCloneOnDashSkill);
        cloneOnArrivalUnlockSlot.GetComponent<Button>().onClick.AddListener(UnlockCloneOnArrivalSkill);
    }

    public void CloneOnDash()
    {
        if (cloneOnDashUnlocked)
        {
            SkillManager.Instance.CloneSkillController.CreateClone(player.transform, Vector2.zero);
        }
    }

    public void CloneOnArrival()
    {
        if (cloneOnArrivalUnlocked)
        {
            SkillManager.Instance.CloneSkillController.CreateClone(player.transform, Vector2.zero);
        }
    }

    private void UnlockDashSkill()
    {
        UnlockSkill(dashUnlockSlot, dashPrice, ref dashUnlocked);
    }

    private void UnlockCloneOnDashSkill()
    {
        if (dashUnlocked)
            UnlockSkill(cloneOnDashUnlockSlot, cloneOnDashPrice, ref cloneOnDashUnlocked);
        
    }

    private void UnlockCloneOnArrivalSkill()
    {
        if (cloneOnDashUnlocked)
            UnlockSkill(cloneOnArrivalUnlockSlot, cloneOnArrivalPrice, ref cloneOnArrivalUnlocked);
    }

    protected override void UseSkill()
    {
        base.UseSkill();
    }
}
