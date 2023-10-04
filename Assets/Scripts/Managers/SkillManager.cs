using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillManager : MonoBehaviour
{
    public static SkillManager Instance { get; private set; }

    public SkillDash SkillDash { get; private set; }
    public SkillClone SkillClone { get; private set; }
    public SkillSword SkillThrowSword { get; private set; }

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

        SkillDash = GetComponent<SkillDash>();
        SkillClone = GetComponent<SkillClone>();
        SkillThrowSword = GetComponent<SkillSword>();
    }
}
