using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillManager : MonoBehaviour, ISavePoint
{
    public static SkillManager Instance { get; private set; }

    public SkillController SkillController { get; private set; }
    public DashSkillController DashSkillController { get; private set; }
    public ParrySkillController ParrySkillController { get; private set; }
    public CloneSkillController CloneSkillController { get; private set; }
    public SwordSkillController SwordSkillController { get; private set; }
    public BlackholeSkillController BlackholeSkillController { get; private set; }
    public CrystalSkillController CrystalSkillController { get; private set; }
    public DodgeSkillController DodgeSkillController { get; private set; }

    [SerializeField] private UI_SkillTreeSlot[] skillSlots;

    private SkillController[] skillControllers;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }

        SkillController = GetComponent<SkillController>();

        DashSkillController = GetComponent<DashSkillController>();
        CloneSkillController = GetComponent<CloneSkillController>();
        SwordSkillController = GetComponent<SwordSkillController>();
        BlackholeSkillController = GetComponent<BlackholeSkillController>();
        CrystalSkillController = GetComponent<CrystalSkillController>();
        ParrySkillController = GetComponent<ParrySkillController>();
        DodgeSkillController = GetComponent<DodgeSkillController>();

        skillControllers = GetComponents<SkillController>();
    }

    public void LoadData(GameData data)
    {
        if (data.skills != null)
        {
            foreach (KeyValuePair<string, bool> kvp in data.skills)
            {
                foreach (var slot in skillSlots)
                {
                    if (kvp.Key == slot.GetName())
                    {
                        if (kvp.Value)
                        {
                            slot.UnlockSlot();
                        }
                        break;
                    }
                }
            }
        }

        foreach (var controller in skillControllers)
        {
            controller.CheckUnlockedSkills();
        }
    }

    public void SaveData(ref GameData data)
    {
        data.skills.Clear();

        foreach (var slot in skillSlots) 
        {
            if (data.skills.TryGetValue(slot.GetName(), out bool value))
            {
                data.skills.Remove(slot.GetName());
                data.skills.Add(slot.GetName(), slot.Unlocked);
            }
            else
            {
                data.skills.Add(slot.GetName(), slot.Unlocked);
            }
        }
    }
}
