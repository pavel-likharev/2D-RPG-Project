using System;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;

public class CharacterStats : MonoBehaviour
{
    public event EventHandler OnHealthChange;

    private CharacterFX fx;

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

    private float igniteEffect = 0.2f;
    private float chillEffect = 0.8f;
    private int shockEffect = 20;

    [SerializeField] private int igniteDamage;
    private float igniteDamageTimer;
    private float igniteDamageCooldown = 0.3f;


    protected virtual void Start()
    {
        fx = GetComponentInChildren<CharacterFX>();

        currentHealth = GetMaxHealtValue();
        critPower.SetDefaultValue(150);
    }

    protected virtual void Update()
    {
        ElementalEffect();
    }

    private void ElementalEffect()
    {
        if (isElementalState)
        {
            if (isIgniting)
            {
                if (igniteDamageTimer >= 0)
                {
                    igniteDamageTimer -= Time.deltaTime;
                }
                else
                {
                    DecreaseHealth(igniteDamage);

                    if (currentHealth <= 0)
                    {
                        Die();
                    }

                    igniteDamageTimer = igniteDamageCooldown;
                }


                if (igniteTimer >= 0)
                {
                    igniteTimer -= Time.deltaTime;
                }
                else
                {
                    isIgniting = false;
                    isElementalState = false;
                }
            }

            if (isChilling)
            {
                if (chillTimer >= 0)
                {
                    chillTimer -= Time.deltaTime;
                }
                else
                {
                    isChilling = false;
                    isElementalState = false;
                }
            }

            if (isShocking)
            {
                if (shockTimer >= 0)
                {
                    shockTimer -= Time.deltaTime;
                }
                else
                {
                    isShocking = false;
                    isElementalState = false;
                }
            }

        }
    }

    public virtual void DoDamage(CharacterStats target)
    {
        if (CanTargetAvoidAttack(target))
            return;

        int totalDamage = damage.GetValue() + strength.GetValue();

        if (CanCrit())
            totalDamage = CalculateCriticalDamage(totalDamage);       
        
        totalDamage = CheckTargetArmor(target, totalDamage);

        target.TakeDamage(totalDamage);
        DoMagicalDamage(target);
    }

    public void DoMagicalDamage(CharacterStats target)
    {
        int fireDamage = this.fireDamage.GetValue();
        int iceDamage = this.iceDamage.GetValue();
        int lightingDamage = this.lightingDamage.GetValue();

        int totalMagicalDamage = fireDamage + iceDamage + lightingDamage + intelligence.GetValue();
        totalMagicalDamage = CheckTargetResistance(target, totalMagicalDamage);

        target.TakeDamage(totalMagicalDamage);

        if (Mathf.Max(fireDamage, iceDamage, lightingDamage) <= 0)
            return;


        bool canApplyIgnite = fireDamage > iceDamage && fireDamage > lightingDamage;
        bool canApplyChill = iceDamage > fireDamage && iceDamage > lightingDamage;
        bool canApplyShock = lightingDamage > fireDamage && lightingDamage > iceDamage;

        while (!canApplyIgnite && !canApplyChill && !canApplyShock)
        {
            if (UnityEngine.Random.value < 0.5f && fireDamage > 0)
            {
                canApplyIgnite = true;
                target.ApplyElements(canApplyIgnite, canApplyChill, canApplyShock);
                target.SetupIgniteDamage(Mathf.RoundToInt(fireDamage * igniteEffect));
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
                return;
            }
        }

        if (canApplyIgnite)
        {
            target.SetupIgniteDamage(Mathf.RoundToInt(fireDamage * igniteEffect));
        }

        target.ApplyElements(canApplyIgnite, canApplyChill, canApplyShock);
    }

    public void SetupIgniteDamage(int value) => igniteDamage = value;

    public void ApplyElements(bool isIgnite, bool isChill, bool isShock)
    {
        if (isElementalState)
            return;

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

            fx.ChillFx(chillTimer);
        }

        if (isShock)
        {
            isElementalState = true;
            shockTimer = elementalTimer;
            isShocking = isShock;

            fx.ShockFx(shockTimer);
        }
    }

    public virtual void TakeDamage(int damage)
    {
        DecreaseHealth(damage);

        Debug.Log(damage);

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    protected virtual void DecreaseHealth(int damage)
    {
        currentHealth -= damage;

        OnHealthChange?.Invoke(this, EventArgs.Empty);
    }

    protected virtual void Die()
    {
        //throw new NotImplementedException();
    }
    private bool CanTargetAvoidAttack(CharacterStats target)
    {
        int totalEvasion = target.evasion.GetValue() + target.agility.GetValue();

        if (isShocking)
        {
            totalEvasion -= shockEffect;
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
            totalDamage -= Mathf.RoundToInt(target.armor.GetValue() * chillEffect);
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
}
