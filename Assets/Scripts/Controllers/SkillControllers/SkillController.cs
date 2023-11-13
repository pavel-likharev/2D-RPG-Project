using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    protected void UnlockSkill(UI_SkillTreeSlot skillSlot, int price, ref bool skillUnlocked)
    {
        Debug.Log("Attempt to unlock skill");

        if (!PlayerManager.Instance.HaveEnoughMoney(price))
            return;
        
        skillUnlocked = true;
        skillSlot.UnlockSlot();
        Debug.Log("unlocked");
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
