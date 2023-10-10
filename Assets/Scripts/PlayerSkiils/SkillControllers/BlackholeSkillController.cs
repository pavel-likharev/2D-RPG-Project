using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlackholeSkillController : SkillController
{
    [SerializeField] private GameObject blackholePrefab;

    [SerializeField] private float maxSize = 15;
    [SerializeField] private float growSpeed = 0.2f;
    [SerializeField] private float shrinkSpeed;
    [SerializeField] private float blackholeDuration;
    [Space]
    [SerializeField] private int attackAmount;
    [SerializeField] private float cloneAttackCooldown;

    BlackholeSkill currentBlackhole;

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
        currentBlackhole = newBlackhole.GetComponent<BlackholeSkill>();

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

    public float GetBlackholeRadius()
    {
        return maxSize / 2;
    }
}
