using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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

    [Header("Blackhole Skill")]
    [SerializeField] private UI_SkillTreeSlot blackholeSKillSlot;
    [SerializeField] private int blackholePrice;
    public bool BlackholeSkillUnlocked { get; private set; }

    BlackholeSkill currentBlackhole;


    protected override void Start()
    {
        base.Start();

        blackholeSKillSlot.GetComponent<Button>().onClick.AddListener(UnlockBlackhole);
        blackholeSKillSlot.SetPriceText(blackholePrice);
    }

    private void UnlockBlackhole()
    {
        if (player.Skill.CloneSkillController.CloneAttackUnlocked)
        {
            BlackholeSkillUnlocked = UnlockSkill(blackholeSKillSlot, blackholePrice);

            blackholeSKillSlot.GetComponent<Button>().enabled = false;
        }
    }

    protected override void UseSkill()
    {
        base.UseSkill();

        CreateBlackhole();
        UI.Instance.InGame.SetBlackholeCooldown();
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
