using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillManager : MonoBehaviour
{
    public static SkillManager Instance { get; private set; }

    public DashSkillController DashSkillController { get; private set; }
    public ParrySkillController ParrySkillController { get; private set; }
    public CloneSkillController CloneSkillController { get; private set; }
    public SwordSkillController SwordSkillController { get; private set; }
    public BlackholeSkillController BlackholeSkillController { get; private set; }
    public CrystalSkillController CrystalSkillController { get; private set; }
    public DodgeSkillController DodgeSkillController { get; private set; }

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(Instance.gameObject);
        }
        else
        {
            Instance = this;
        }

        DashSkillController = GetComponent<DashSkillController>();
        CloneSkillController = GetComponent<CloneSkillController>();
        SwordSkillController = GetComponent<SwordSkillController>();
        BlackholeSkillController = GetComponent<BlackholeSkillController>();
        CrystalSkillController = GetComponent<CrystalSkillController>();
        ParrySkillController = GetComponent<ParrySkillController>();
        DodgeSkillController = GetComponent<DodgeSkillController>();
    }
}
