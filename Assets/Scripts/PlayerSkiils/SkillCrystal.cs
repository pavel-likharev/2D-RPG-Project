using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillCrystal : Skill
{
    [SerializeField] private GameObject crystalPrefab;
    [SerializeField] private float crystalDuration;

    [Header("Explode info")]
    private bool canExplode = true;

    [Header("Move info")]
    [SerializeField] private bool canMove;
    [SerializeField] float moveSpeed;

    [Header("Multi staking crystal")]
    [SerializeField] private bool canUseMultiStacks;
    [SerializeField] private int stackAmount;
    [SerializeField] private float multiStackCooldown;
    [SerializeField] private float useTimeWindow;

    [Header("Crystal - Mirage")]
    [SerializeField] private bool isCloneMirage;

    private List<GameObject> crystals = new List<GameObject>();
    private GameObject currentCrystal;

    protected override void Start()
    {
        base.Start();

        RefilCrystal();
    }

    protected override void UseSkill()
    {
        base.UseSkill();

        if (CanUseMultiCrystal())
        {
            return;
        }

        if (currentCrystal == null)
        {

            currentCrystal = Instantiate(crystalPrefab, player.transform.position, Quaternion.identity);
            SkillCrystalController crystalController = currentCrystal.GetComponent<SkillCrystalController>();

            crystalController.SetupCrystal(crystalDuration, canExplode, canMove, moveSpeed, FindClosestEnemy(currentCrystal.transform));
        }
        else
        {
            if (canMove)
                return;

            Vector2 playerPos = player.transform.position;
            player.transform.position = currentCrystal.transform.position;
            currentCrystal.transform.position = playerPos;

            if (isCloneMirage)
            {
                player.Skill.SkillClone.CreateClone(currentCrystal.transform, Vector3.zero);
                Destroy(currentCrystal);
            }
            else
            {
                currentCrystal.GetComponent<SkillCrystalController>()?.FinishedCrystal();
            }

        }
    }

    private bool CanUseMultiCrystal()
    {
        if (canUseMultiStacks)
        {
            if (crystals.Count > 0)
            {
                if (crystals.Count == stackAmount)
                    Invoke("ResetAbility", useTimeWindow);
                

                cooldown = 0;
                GameObject spawnedCrystal = crystals[crystals.Count - 1];
                GameObject newCrystal = Instantiate(spawnedCrystal, player.transform.position, Quaternion.identity);

                crystals.Remove(spawnedCrystal);

                newCrystal.GetComponent<SkillCrystalController>().
                    SetupCrystal(crystalDuration, canExplode, canMove, moveSpeed, FindClosestEnemy(newCrystal.transform));

                if (crystals.Count <= 0)
                {
                    cooldown = multiStackCooldown;
                    RefilCrystal();
                }

                return true;
            }
           
        }

        return false;
    }

    private void RefilCrystal()
    {
        int addAmount = stackAmount - crystals.Count;

        for (int i = 0; i < addAmount; i++)
        {
            crystals.Add(crystalPrefab);
        }
    }

    private void ResetAbility()
    {
        if (cooldownTimer > 0)
            return;

        cooldownTimer = multiStackCooldown;
        RefilCrystal();
    }
}
