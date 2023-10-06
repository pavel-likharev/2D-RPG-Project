using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillBlackhole : Skill
{
    [SerializeField] private GameObject blackholePrefab;

    [SerializeField] private float maxSize = 10;
    [SerializeField] private float growSpeed = 0.2f;
    [SerializeField] private float shrinkSpeed;
    [SerializeField] private float blackholeDuration;
    [Space]
    [SerializeField] private int attackAmount;
    [SerializeField] private float cloneAttackCooldown;

    SkillBlackholeController currentBlackhole;

    protected override void Awake()
    {
        base.Awake();
    }

    protected override void Start()
    {
        base.Start();
    }

    protected override void Update()
    {
        base.Update();
    }

    protected override void UseSkill()
    {
        base.UseSkill();

        CreateBlackhole();
    }

    public void CreateBlackhole()
    {
        GameObject newBlackhole = Instantiate(blackholePrefab, player.transform.position, Quaternion.identity);
        currentBlackhole = newBlackhole.GetComponent<SkillBlackholeController>();

        currentBlackhole.SetupBlackhole(maxSize, growSpeed, shrinkSpeed, attackAmount, cloneAttackCooldown, blackholeDuration);
    }

    public bool SkillFinished()
    {
        if (!currentBlackhole)
            return false;

        if (currentBlackhole.CanExitState)
        {
            currentBlackhole = null;
            return true;
        }
        return false;
    }
}
