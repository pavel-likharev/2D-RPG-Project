using System;
using System.Xml;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;

public class CharacterStats : MonoBehaviour
{
    public event EventHandler OnHealthChange;
    private CharacterFX fx;

    #region Fields
    [Header("Major stats")]
    public Stat strength; // 1 point increase damage by 1 and crit.power by 1%
    public Stat agility; // 1 point increase evasion by 1 and crit.chance by 1%
    public Stat intelligence; // 1 point increase magic damage by 1 and resistance by 3%
    public Stat vitality; // // 1 point increase health by 3 or 5 point
    protected bool isDead;

    [Header("Offensive stats")]
    public Stat damage;
    public Stat critChance;
    public Stat critPower; // default value 150%

    [Header("Defence stats")]
    public int currentHealth;
    public Stat maxHealth;
    public Stat armor;
    public Stat evasion;
    public Stat magicResistance;

    [Header("Elemental modifies")]
    public Stat fireDamage;
    public Stat iceDamage;
    public Stat lightingDamage;

    public bool isElementalState;
    public bool isIgniting; // does damage over time
    public bool isChilling; // reduce armor by 20%
    public bool isShocking; // reduce accuracy by 20%

    private float igniteTimer;
    private float chillTimer;
    private float shockTimer;
    private float elementalTimer = 2;

    [Header("Elemenatal values")]

    private int igniteDamage;
    private float igniteDamageMultiplier = 0.2f;
    private float igniteDamageTimer;
    private float igniteDamageCooldown = 0.3f;

    private float chillEffectMultiplier = 0.8f;

    private float shockDamageMultiplier = 0.1f;
    private int shockEffectReducer = 20;
    
    [SerializeField] private GameObject thunderStrikePrefab;
    private int thunderDamage;

    private float checkRadiusClosestEnemy = 25f;
    #endregion


    protected virtual void Start()
    {
        fx = GetComponentInChildren<CharacterFX>();

        currentHealth = GetMaxHealtValue();
        critPower.SetDefaultValue(150);
    }

    protected virtual void Update()
    {
        DoElementalEffect();
    }


    public virtual void DoDamage(CharacterStats target, int knockBackDir)
    {
        if (CanTargetAvoidAttack(target))
            return;

        int totalDamage = damage.GetValue() + strength.GetValue();

        if (CanCrit())
            totalDamage = CalculateCriticalDamage(totalDamage);       
        
        totalDamage = CheckTargetArmor(target, totalDamage);

        target.TakeDamage(totalDamage, knockBackDir);
        // DoMagicalDamage(target);
    }
    public virtual void TakeDamage(int damage, int knockBackDir)
    {
        DecreaseHealth(damage);

        GetComponent<Character>().DamageImpact(knockBackDir);
        fx.StartCoroutine("HitFX");

        if (currentHealth <= 0 && !isDead)
            Die();
    }
    protected virtual void DecreaseHealth(int damage)
    {
        currentHealth -= damage;

        OnHealthChange?.Invoke(this, EventArgs.Empty);
    }
    protected virtual void Die()
    {
        isDead = true;
    }

    #region Magical Damage and elements 
    public void DoMagicalDamage(CharacterStats target, int knockBackDir)
    {
        int fireDamage = this.fireDamage.GetValue();
        int iceDamage = this.iceDamage.GetValue();
        int lightingDamage = this.lightingDamage.GetValue();

        int totalMagicalDamage = fireDamage + iceDamage + lightingDamage + intelligence.GetValue();
        totalMagicalDamage = CheckTargetResistance(target, totalMagicalDamage);

        target.TakeDamage(totalMagicalDamage, knockBackDir);

        if (Mathf.Max(fireDamage, iceDamage, lightingDamage) <= 0)
            return;

        ElementalLogic(target, fireDamage, iceDamage, lightingDamage);
    }
    private void DoElementalEffect()
    {
        if (isElementalState)
        {
            if (isIgniting)
            {
                ApplyIgniteDamage();

                ElementalTimer(ref igniteTimer, ref isIgniting);
            }

            if (isChilling)
            {
                ElementalTimer(ref chillTimer, ref isChilling);
            }

            if (isShocking)
            {
                ElementalTimer(ref shockTimer, ref isShocking);
            }

        }
    }

    private void ElementalTimer(ref float timer, ref bool elemental)
    {
        if (timer >= 0)
        {
            timer -= Time.deltaTime;
        }
        else
        {
            elemental = false;
            isElementalState = false;
        }
    }


    private void ElementalLogic(CharacterStats target, int fireDamage, int iceDamage, int lightingDamage)
    {
        bool canApplyIgnite = fireDamage > iceDamage && fireDamage > lightingDamage;
        bool canApplyChill = iceDamage > fireDamage && iceDamage > lightingDamage;
        bool canApplyShock = lightingDamage > fireDamage && lightingDamage > iceDamage;

        while (!canApplyIgnite && !canApplyChill && !canApplyShock)
        {
            if (UnityEngine.Random.value < 0.5f && fireDamage > 0)
            {
                canApplyIgnite = true;
                target.ApplyElements(canApplyIgnite, canApplyChill, canApplyShock);
                target.SetupIgniteDamage(Mathf.RoundToInt(fireDamage * igniteDamageMultiplier));
                return;
            }

            if (UnityEngine.Random.value < 0.5f && iceDamage > 0)
            {
                canApplyChill = true;
                target.ApplyElements(canApplyIgnite, canApplyChill, canApplyShock);
                return;
            }

            if (UnityEngine.Random.value < 0.5f && lightingDamage > 0)
            {
                canApplyShock = true;
                target.ApplyElements(canApplyIgnite, canApplyChill, canApplyShock);
                target.SetupThunderDamage(Mathf.RoundToInt(lightingDamage * shockDamageMultiplier));
                return;
            }
        }

        if (canApplyIgnite)
        {
            target.SetupIgniteDamage(Mathf.RoundToInt(fireDamage * igniteDamageMultiplier));
        }

        if (canApplyShock)
        {
            target.SetupThunderDamage(Mathf.RoundToInt(lightingDamage * shockDamageMultiplier));
        }

        target.ApplyElements(canApplyIgnite, canApplyChill, canApplyShock);
    }

    public void SetupIgniteDamage(int value) => igniteDamage = value;
    public void SetupThunderDamage(int value) => thunderDamage = value;

    public void ApplyElements(bool isIgnite, bool isChill, bool isShock)
    {
        if (isElementalState)
        {
            if (isShock)
            {
                if (GetComponent<Player>() != null)
                    return;
                
                HitThunderClosestEnemy();
            }

            return;
        }

        if (isIgnite)
        {
            isElementalState = true;
            igniteTimer = elementalTimer;
            isIgniting = isIgnite;

            fx.IgniteFx(igniteTimer);
        }

        if (isChill)
        {
            isElementalState = true;
            chillTimer = elementalTimer;
            isChilling = isChill;

            float slowDownValue = 0.2f;
            GetComponent<Character>().SlowDownCharacter(slowDownValue, chillTimer);

            fx.ChillFx(chillTimer);
        }

        if (isShock)
        {
            ApplyShockEffect(isShock);
        }
    }
    private void ApplyIgniteDamage()
    {
        
        if (igniteDamageTimer >= 0)
        {
            igniteDamageTimer -= Time.deltaTime;
        }
        else
        {
            Debug.Log(igniteDamage);
            DecreaseHealth(igniteDamage);

            if (currentHealth <= 0 && !isDead)
            {
                Die();
            }

            igniteDamageTimer = igniteDamageCooldown;
        }
    }
    public void ApplyShockEffect(bool isShock)
    {
        if (isShocking)
            return;

        isElementalState = true;
        shockTimer = elementalTimer;
        isShocking = isShock;

        fx.ShockFx(shockTimer);
    }
    private void HitThunderClosestEnemy()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, checkRadiusClosestEnemy);
        float closestDistance = Mathf.Infinity;
        Transform closestEnemy = null;

        foreach (Collider2D hit in colliders)
        {
            if (hit.GetComponent<Enemy>() != null && Vector2.Distance(transform.position, hit.transform.position) > 1)
            {
                float distanceToEnemy = Vector2.Distance(transform.position, hit.transform.position);

                if (distanceToEnemy < closestDistance)
                {
                    closestDistance = distanceToEnemy;
                    closestEnemy = hit.transform;
                }
            }
        }

        if (closestEnemy == null)
            closestEnemy = transform;

        if (closestEnemy != null)
        {
            GameObject thunderStrike = Instantiate(thunderStrikePrefab, transform.position + new Vector3(1, 0), Quaternion.identity);
            thunderStrike.GetComponent<ThunderStrikeContoller>().Setup(closestEnemy.GetComponent<CharacterStats>(), thunderDamage);
        }
    }
    #endregion

    #region Stat calculation
    private bool CanTargetAvoidAttack(CharacterStats target)
    {
        int totalEvasion = target.evasion.GetValue() + target.agility.GetValue();

        if (isShocking)
        {
            totalEvasion -= shockEffectReducer;
        }

        if (UnityEngine.Random.Range(0, 100) < totalEvasion)
        {
            return true;
        }

        return false;
    }
    private int CheckTargetResistance(CharacterStats target, int totalMagicalDamage)
    {
        totalMagicalDamage -= target.magicResistance.GetValue() + (target.intelligence.GetValue() * 3);
        totalMagicalDamage = Mathf.Clamp(totalMagicalDamage, 0, int.MaxValue);
        return totalMagicalDamage;
    }
    private int CheckTargetArmor(CharacterStats target, int totalDamage)
    {
        if (isChilling)
        {
            totalDamage -= Mathf.RoundToInt(target.armor.GetValue() * chillEffectMultiplier);
        }
        else
        {
            totalDamage -= target.armor.GetValue();
        }


        totalDamage = Mathf.Clamp(totalDamage, 0, int.MaxValue);

        return totalDamage;
    }
    private bool CanCrit()
    {
        int totalCriticalChance = critChance.GetValue() + agility.GetValue();

        if (UnityEngine.Random.Range(0, 100) <= (totalCriticalChance))
        {
            return true;
        }

        return false;
    }
    private int CalculateCriticalDamage(int damage)
    {
        float totalCritPower = (critPower.GetValue() + strength.GetValue()) * 0.01f;
        float critDamage = damage * totalCritPower;
        return Mathf.RoundToInt(critDamage);
    }
    public int GetMaxHealtValue() => maxHealth.GetValue() + vitality.GetValue() * 5;
    #endregion
}