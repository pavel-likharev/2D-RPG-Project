using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EquipmentType
{
    Weapon,
    Armor,
    Amulet,
    Flask
}

[CreateAssetMenu(fileName = "NewItemData", menuName = "Data/Equipment")]
public class ItemData_Equipment : ItemData
{
    public EquipmentType equipmentType;

    [Header("Unique effect")]
    public List<Effect> effects;
    public float itemCooldown;
    [TextArea] public string effectDescriptionText;

    [Header("Major stats")]
    public int strength; // 1 point increase damage by 1 and crit.power by 1%
    public int agility; // 1 point increase evasion by 1 and crit.chance by 1%
    public int intelligence; // 1 point increase magic damage by 1 and resistance by 3%
    public int vitality; // // 1 point increase health by 3 or 5 point

    [Header("Offensive stats")]
    public int damage;
    public int critChance;
    public int critPower; // default value 150%

    [Header("Defence stats")]
    public int health;
    public int armor;
    public int evasion;
    public int magicResistance;

    [Header("Elemental modifies")]
    public int fireDamage;
    public int iceDamage;
    public int lightingDamage;

    [Header("Craft requirements")]
    public List<InventoryItem> craftingMaterials; // max 4

    public void ApplyEffect(Transform target)
    {
        foreach (var effect in effects)
        {
            effect.ExecuteEffect(target);
        }

        Inventory.Instance.UpdateStatSlotsUI();
    }

    public void AddModifiers()
    {
        PlayerStats playerStats = PlayerManager.Instance.Player.GetComponent<PlayerStats>();

        playerStats.strength.AddModifier(strength);
        playerStats.agility.AddModifier(agility);
        playerStats.intelligence.AddModifier(intelligence);
        playerStats.vitality.AddModifier(vitality);

        playerStats.damage.AddModifier(damage);
        playerStats.critChance.AddModifier(critChance);
        playerStats.critPower.AddModifier(critPower);

        playerStats.maxHealth.AddModifier(health);
        playerStats.armor.AddModifier(armor);
        playerStats.evasion.AddModifier(evasion);
        playerStats.magicResistance.AddModifier(magicResistance);

        playerStats.fireDamage.AddModifier(fireDamage);
        playerStats.iceDamage.AddModifier(iceDamage);
        playerStats.lightingDamage.AddModifier(lightingDamage);
    }

    public void RemoveModifiers()
    {
        PlayerStats playerStats = PlayerManager.Instance.Player.GetComponent<PlayerStats>();

        playerStats.strength.RemoveModifier(strength);
        playerStats.agility.RemoveModifier(agility);
        playerStats.intelligence.RemoveModifier(intelligence);
        playerStats.vitality.RemoveModifier(vitality);

        playerStats.damage.RemoveModifier(damage);
        playerStats.critChance.RemoveModifier(critChance);
        playerStats.critPower.RemoveModifier(critPower);

        playerStats.maxHealth.RemoveModifier(health);
        playerStats.armor.RemoveModifier(armor);
        playerStats.evasion.RemoveModifier(evasion);
        playerStats.magicResistance.RemoveModifier(magicResistance);

        playerStats.fireDamage.RemoveModifier(fireDamage);
        playerStats.iceDamage.RemoveModifier(iceDamage);
        playerStats.lightingDamage.RemoveModifier(lightingDamage);
    }

    public override string GetDescription()
    {
        description.Length = 0;

        AddItemDescription(strength, "Strenght");
        AddItemDescription(agility, "Agility");
        AddItemDescription(intelligence, "Intelligence");
        AddItemDescription(vitality, "Vitality");

        AddItemDescription(damage, "Damage");
        AddItemDescription(critChance, "Crit.chance");
        AddItemDescription(critPower, "Crit.power");

        AddItemDescription(health, "Health");
        AddItemDescription(evasion, "Evasion");
        AddItemDescription(armor, "Armor");
        AddItemDescription(magicResistance, "Magic Resist.");

        AddItemDescription(fireDamage, "Fire Damage");
        AddItemDescription(iceDamage, "Ice Damage");
        AddItemDescription(lightingDamage, "Lighting Damage");

        if (effectDescriptionText.Length > 0 && effectDescriptionText != null)
        {
            description.AppendLine();
            description.Append(effectDescriptionText);
        }

        return description.ToString();
    }

    private void AddItemDescription(int value, string name)
    {
        if (value != 0)
        {
            if (description.Length > 0) 
                description.AppendLine();

            if (value > 0)
                description.Append(name + ": " + value);
        }
    }
}
