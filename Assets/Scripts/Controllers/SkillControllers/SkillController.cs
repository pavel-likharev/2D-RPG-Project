using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillController : MonoBehaviour
{
    [SerializeField] protected float cooldown;
    protected float checkRadiusClosestEnemy = 25f;
    protected float cooldownTimer;
    protected Player player;

    protected virtual void Awake()
    {
    }

    protected virtual void Start()
    {
        player = PlayerManager.Instance.Player;
    }

    protected virtual void Update()
    {
        if (cooldownTimer >= 0)
        {
            cooldownTimer -= Time.deltaTime;
        }
    }

    public virtual void CheckUnlockedSkills()
    {
    }

    protected bool UnlockSkill(UI_SkillTreeSlot skillSlot, int price)
    {
        if (!PlayerManager.Instance.HaveEnoughCurrency(price))
            return false;
        
        skillSlot.UnlockSlot();

        return true;
    }

    public bool CanUseSkill()
    {
        if (cooldownTimer <= 0)
        {
            UseSkill();
            cooldownTimer = cooldown;

            return true;
        }
        return false;
    }

    protected virtual void UseSkill()
    {
    }

    public float GetCooldown() => cooldown;

    public bool IsCooldown() => cooldownTimer > 0;

    protected virtual Transform FindClosestEnemy(Transform checkTransform)
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(checkTransform.position, checkRadiusClosestEnemy);


        float closestDistance = Mathf.Infinity;
        Transform closestEnemy = null;

        foreach (Collider2D hit in colliders)
        {
            if (hit.GetComponent<Enemy>() != null)
            {
                float distanceToEnemy = Vector2.Distance(checkTransform.position, hit.transform.position);

                if (distanceToEnemy < closestDistance)
                {
                    closestDistance = distanceToEnemy;
                    closestEnemy = hit.transform;
                }
            }
        }

        return closestEnemy;
    }
}
