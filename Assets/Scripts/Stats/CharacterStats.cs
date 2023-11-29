using System;
using System.Collections;
using UnityEngine;

public enum StatType
{
    strength,
    agility,
    intelligence,
    vitality,
    damage,
    critChance,
    critPower,
    health,
    armor,
    evasion,
    magicResistance,
    fireDamage,
    iceDamage,
    lightingDamage,
}
public class CharacterStats : MonoBehaviour
{
    public event EventHandler<TransformTargetEventsArgs> OnEvasion;
    public event EventHandler OnHealthChange;
    private CharacterFX fx;

    public bool IsDead { get; private set; }
    public bool IsVulnerable { get; private set; }
    public bool IsInvincible { get; private set; }

    private float vulnerableModify = 1.1f;

    #region Fields
    [Header("Major stats")]
    public Stat strength; // 1 point increase damage by 1 and crit.power by 1%
    public Stat agility; // 1 point increase evasion by 1 and crit.chance by 1%
    public Stat intelligence; // 1 point increase magic damage by 1 and resistance by 3%
    public Stat vitality; // // 1 point increase health by 3 or 5 point

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

    [SerializeField] private GameObject shockStrikePrefab;
    private int shockDamage;

    private float checkRadiusClosestEnemy = 25f;
    #endregion

    protected virtual void Awake()
    {
        
    }

    protected virtual void Start()
    {
        fx = GetComponentInChildren<CharacterFX>();

        currentHealth = GetMaxHealthValue();
        critPower.SetDefaultValue(150);
    }

    protected virtual void Update()
    {
        DoElementalEffect();
    }

    // Damage
    public virtual void DoDamage(CharacterStats target, int knockBackDir, float multiplierDamage = 1)
    {
        if (CanTargetAvoidAttack(target))
            return;

        int totalDamage = damage.GetValue() + strength.GetValue();

        totalDamage = Mathf.RoundToInt(totalDamage * multiplierDamage);

        bool isCrit = false;

        if (CanCrit())
        {
            totalDamage = CalculateCriticalDamage(totalDamage);
            isCrit = true;
        }

        fx.CreateHitFX(target.transform, isCrit);

        totalDamage = CheckTargetArmor(target, totalDamage);

        target.TakeDamage(totalDamage, knockBackDir);

        // DoMagicalDamage(target);
    }
    public virtual void TakeDamage(int damage, int knockBackDir)
    {
        if (IsInvincible)
            return;

        DecreaseHealth(damage);

        GetComponent<Character>().DamageImpact(knockBackDir);
        fx.StartCoroutine("HitColorFX");

        if (currentHealth <= 0 && !IsDead)
            Die();
    }

    // Vulnerable
    public void MakeVulnerable(float duration)
    {
        StartCoroutine(VulnerableFor(duration));
    }
    public void MakeInvincible(bool isInvincible) => IsInvincible = isInvincible;
    private IEnumerator VulnerableFor(float duration)
    {
        IsVulnerable = true;

        yield return new WaitForSeconds(duration);

        IsVulnerable = false;
    }

    // Stats modify
    public virtual void ChangeStatTemporarily(Stat stat, int value, float duration)
    {
        StartCoroutine(StatModify(stat, value, duration));
    }
    private IEnumerator StatModify(Stat stat, int value, float duration)
    {
        stat.AddModifier(value);

        yield return new WaitForSeconds(duration);

        stat.RemoveModifier(value);
    }
    public Stat GetStatFromType(StatType statType)
    {
        switch (statType)
        {
            case StatType.strength:
                return strength;

            case StatType.agility:
                return agility;
            case StatType.intelligence:
                return intelligence;
            case StatType.vitality:
                return vitality;
            case StatType.damage:
                return damage;
            case StatType.critChance:
                return critChance;
            case StatType.critPower:
                return critPower;
            case StatType.health:
                return maxHealth;
            case StatType.armor:
                return armor;
            case StatType.evasion:
                return evasion;
            case StatType.magicResistance:
                return magicResistance;
            case StatType.fireDamage:
                return fireDamage;
            case StatType.iceDamage:
                return iceDamage;
            case StatType.lightingDamage:
                return lightingDamage;
            default:
                return null;
        }
    }

    // Health
    public virtual void IncreaseHealth(int value)
    {
        currentHealth += value;

        if (currentHealth > GetMaxHealthValue())
            currentHealth = GetMaxHealthValue();

        OnHealthChange?.Invoke(this, EventArgs.Empty);
    }
    protected virtual void DecreaseHealth(int damage)
    {
        if (IsDead) 
            return;
        
        if (IsVulnerable)
        {
            damage = Mathf.RoundToInt(damage * vulnerableModify);
        }

        currentHealth -= damage;

        OnHealthChange?.Invoke(this, EventArgs.Empty);
    }
    protected virtual void Die()
    {
        IsDead = true;
    }

    public void KillCharacter()
    {
        if (!IsDead)
            Die();
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
                target.SetupShockDamage(Mathf.RoundToInt(lightingDamage * shockDamageMultiplier));
                return;
            }
        }

        if (canApplyIgnite)
        {
            target.SetupIgniteDamage(Mathf.RoundToInt(fireDamage * igniteDamageMultiplier));
        }

        if (canApplyShock)
        {
            target.SetupShockDamage(Mathf.RoundToInt(lightingDamage * shockDamageMultiplier));
        }

        target.ApplyElements(canApplyIgnite, canApplyChill, canApplyShock);
    }

    public void SetupIgniteDamage(int value) => igniteDamage = value;
    public void SetupShockDamage(int value) => shockDamage = value;

    public void ApplyElements(bool isIgnite, bool isChill, bool isShock)
    {
        if (isElementalState)
        {
            if (isShock)
            {
                if (GetComponent<Player>() != null)
                    return;

                HitShockClosestEnemy();
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
            DecreaseHealth(igniteDamage);

            if (currentHealth <= 0 && !IsDead)
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
    private void HitShockClosestEnemy()
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
            GameObject shockStrike = Instantiate(shockStrikePrefab, transform.position + new Vector3(1, 0), Quaternion.identity);
            shockStrike.GetComponent<ShockStrikeContoller>().Setup(closestEnemy.GetComponent<CharacterStats>(), shockDamage);
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
            target.OnEvasion?.Invoke(this, new TransformTargetEventsArgs
            {
                target = transform
            });
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
    public int GetMaxHealthValue() => maxHealth.GetValue() + vitality.GetValue() * 5;
    #endregion
}

public class TransformTargetEventsArgs
{
    public Transform target;
}