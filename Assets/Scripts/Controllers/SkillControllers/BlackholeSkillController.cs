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
    [SerializeField] private UI_SkillTreeSlot blackholeSkillSlot;
    [SerializeField] private int blackholePrice;
    public bool BlackholeSkillUnlocked { get; private set; }

    private BlackholeSkill currentBlackhole;

    protected override void Start()
    {
        base.Start();

        blackholeSkillSlot.GetComponent<Button>().onClick.AddListener(TryUnlockBlackHoleSkill);
        blackholeSkillSlot.SetPriceText(blackholePrice);

    }

    // Check skills
    public override void CheckUnlockedSkills()
    {
        if (blackholeSkillSlot.Unlocked) 
            UnlockBlackhole();
    }

    private void TryUnlockBlackHoleSkill()
    {
        if (UnlockSkill(blackholeSkillSlot, blackholePrice, player.Skill.CloneSkillController.CloneAttackUnlocked))
            UnlockBlackhole();
    }

    private void UnlockBlackhole()
    {
        BlackholeSkillUnlocked = true;
    }
    
    // Logic
    protected override void UseSkill()
    {
        base.UseSkill();

        player.StateMachine.ChangeState(player.BlackholeState);

        UI.Instance.InGame.SetBlackholeCooldown();

        AudioManager.Instance.PlaySFX(3, player.transform);
        AudioManager.Instance.PlaySFX(6, player.transform);
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
