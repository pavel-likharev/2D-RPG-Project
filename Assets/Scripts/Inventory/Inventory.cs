using System.Collections;
using System.Collections.Generic;
using UnityEditorInternal.Profiling.Memory.Experimental;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public static Inventory Instance { get; private set; }

    [SerializeField] private List<ItemData> startingItems;

    public List<InventoryItem> inventory;
    public Dictionary<ItemData, InventoryItem> inventoryDictionary;
    
    public List<InventoryItem> stash;
    public Dictionary<ItemData, InventoryItem> stashDictionary;

    public List<InventoryItem> equipment;
    public Dictionary<ItemData_Equipment, InventoryItem> equipmentDictionary;

    [Header("Inventory UI")]
    [SerializeField] private Transform inventoryContainer;
    [SerializeField] private Transform stashContainer;
    [SerializeField] private Transform equipmentContainer;

    private UI_ItemSlot[] inventoryItemSlots;
    private UI_ItemSlot[] stashItemSlots;
    private UI_EquipmentSlot[] equipmentSlots;

    [Header("Cooldown")]
    private float lastTimeUsedFlask;
    public float lastTimeUsedArmor;

    private float flaskCooldown;
    private float armorCooldown;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    private void Start()
    {
        inventory = new List<InventoryItem>();
        inventoryDictionary = new Dictionary<ItemData, InventoryItem>();

        stash = new List<InventoryItem>();
        stashDictionary = new Dictionary<ItemData, InventoryItem>();

        equipment = new List<InventoryItem>();
        equipmentDictionary = new Dictionary<ItemData_Equipment, InventoryItem>();

        inventoryItemSlots = inventoryContainer.GetComponentsInChildren<UI_ItemSlot>();
        stashItemSlots = stashContainer.GetComponentsInChildren<UI_ItemSlot>();
        equipmentSlots = equipmentContainer.GetComponentsInChildren<UI_EquipmentSlot>();

        AddStartingInventory();
    }

    private void AddStartingInventory()
    {
        for (int i = 0; i < startingItems.Count; i++)
        {
            AddItem(startingItems[i]);
        }
    }

    public void EquipItem(ItemData item)
    {
        ItemData_Equipment newEquipment = item as ItemData_Equipment;
        InventoryItem newItem = new InventoryItem(newEquipment);
        ItemData_Equipment oldEquipment = null;

        foreach (KeyValuePair<ItemData_Equipment, InventoryItem> currentItem in equipmentDictionary)
        {
            if (currentItem.Key.equipmentType == newEquipment.equipmentType)
            {
                oldEquipment = currentItem.Key;
                break;
            }
        }

        if (oldEquipment != null)
        {
            AddToInventory(oldEquipment);
            UnequipItem(oldEquipment);
        }
        

        equipment.Add(newItem);
        equipmentDictionary.Add(newEquipment, newItem);

        newEquipment.AddModifiers();

        RemoveItemFromInventory(newEquipment);

        UpdateEquipmentSlotUI();
    }

    public void UnequipItem(ItemData_Equipment oldEquipment)
    {
        if (equipmentDictionary.TryGetValue(oldEquipment, out InventoryItem value))
        {
            equipment.Remove(value);
            equipmentDictionary.Remove(oldEquipment);
            oldEquipment.RemoveModifiers();
        }
    }

    public void AddItem(ItemData item)
    {
        switch (item.itemType)  
        {
            case ItemType.Material:
                AddToStash(item);
                break;
            case ItemType.Equipment:
                AddToInventory(item);
                break;
            default:
                break;
        } 
    }

    private void AddToInventory(ItemData item)
    {
        if (inventoryDictionary.TryGetValue(item, out InventoryItem value))
        {
            value.AddStack();
        }
        else
        {
            InventoryItem newItem = new InventoryItem(item);
            inventory.Add(newItem);
            inventoryDictionary.Add(item, newItem);
        }

        UpdateInventorySlotUI();
    }

    private void AddToStash(ItemData item)
    {
        if (stashDictionary.TryGetValue(item, out InventoryItem value))
        {
            value.AddStack();
        }
        else
        {
            InventoryItem newItem = new InventoryItem(item);
            stash.Add(newItem);
            stashDictionary.Add(item, newItem);
        }

        UpdateStashSlotUI();
    }

    public void RemoveItem(ItemData item)
    {
        switch (item.itemType)
        {
            case ItemType.Material:
                RemoveItemFromStash(item);
                break;
            case ItemType.Equipment:
                RemoveItemFromInventory(item);
                break;
            default:
                break;
        }
    }

    public void RemoveItemFromInventory(ItemData item)
    {
        if (inventoryDictionary.TryGetValue(item, out InventoryItem value))
        {
            if (value.stackSize <= 1)
            {
                inventory.Remove(value);
                inventoryDictionary.Remove(item);
            }
            else
            {
                value.RemoveStack();
            }

        }

        UpdateInventorySlotUI();
    }

    public void RemoveItemFromStash(ItemData item)
    {
        if (stashDictionary.TryGetValue(item, out InventoryItem value))
        {
            if (value.stackSize <= 1)
            {
                stash.Remove(value);
                stashDictionary.Remove(item);
            }
            else
            {
                value.RemoveStack();
            }

        }

        UpdateStashSlotUI();
    }

    public bool CanCraft(ItemData_Equipment itemToCraft, List<InventoryItem> requiredMaterials)
    {
        List<InventoryItem> materialsToRemove = new List<InventoryItem>();

        for (int i = 0; i < requiredMaterials.Count; i++)
        {
            if (stashDictionary.TryGetValue(requiredMaterials[i].itemData, out InventoryItem stashValue))
            {
                if (stashValue.stackSize < requiredMaterials[i].stackSize)
                {
                    Debug.Log("Not enough materials");
                    return false;
                }
                else
                {
                    materialsToRemove.Add(stashValue);
                }
            }
            else
            {
                Debug.Log("Not enough materials");
                return false;
            }
        }

        foreach (InventoryItem item in materialsToRemove)
        {
            RemoveItemFromStash(item.itemData);
        }

        AddItem(itemToCraft);
        Debug.Log("Craft " + itemToCraft + " success");
        return true;
    }

    public List<InventoryItem> GetEquipmentList() => equipment;

    public List<InventoryItem> GetStashList() => stash;

    public ItemData_Equipment GetEquipment(EquipmentType type)
    {
        ItemData_Equipment equipedItem = null;

        foreach (KeyValuePair<ItemData_Equipment, InventoryItem> currentItem in equipmentDictionary)
        {
            if (currentItem.Key.equipmentType == type)
            {
                equipedItem = currentItem.Key;
                break;
            }
        }

        return equipedItem;
    }

    public void UseFlask()
    {
        ItemData_Equipment currentFlask = GetEquipment(EquipmentType.Flask);

        if (currentFlask)
        {
            bool canUseFlask = Time.time > lastTimeUsedFlask + flaskCooldown;

            if (canUseFlask)
            {
                flaskCooldown = currentFlask.itemCooldown;
                currentFlask.ApplyEffect(null);
                lastTimeUsedFlask = Time.time;
            }
            else
            {
                Debug.Log("flask on cooldown");
            }
        }
    }

    public void UseArmor()
    {
        ItemData_Equipment armor = GetEquipment(EquipmentType.Armor);

        if (armor)
        {
            Debug.Log("is armor");
            bool canUseArmor = Time.time > lastTimeUsedArmor + armorCooldown;

            if (canUseArmor)
            {
                armorCooldown = armor.itemCooldown;
                armor.ApplyEffect(PlayerManager.Instance.Player.transform);
            }
            else
            {
                Debug.Log("armor on cooldown");
            }
        }
    }

    #region UpdateUI
    public void UpdateInventorySlotUI()
    {
        for (int i = 0; i < inventoryItemSlots.Length; i++)
        {
            inventoryItemSlots[i].CleanUp();
        }

        for (int i = 0; i < inventory.Count; i++)
        {
            inventoryItemSlots[i].UpdateSlot(inventory[i]);
        }
    }

    public void UpdateStashSlotUI()
    {
        for (int i = 0; i < stashItemSlots.Length; i++)
        {
            stashItemSlots[i].CleanUp();
        }

        for (int i = 0; i < stash.Count; i++)
        {
            stashItemSlots[i].UpdateSlot(stash[i]);
        }
    }

    public void UpdateEquipmentSlotUI()
    {
        for (int i = 0; i < equipmentSlots.Length; i++)
        {
            equipmentSlots[i].CleanUp();
        }

        for (int i = 0; i < equipmentSlots.Length; i++)
        {
            foreach (KeyValuePair<ItemData_Equipment, InventoryItem> currentItem in equipmentDictionary)
            {
                if (currentItem.Key.equipmentType == equipmentSlots[i].slotType)
                {
                    equipmentSlots[i].UpdateSlot(currentItem.Value);
                    break;
                }
            }
        }
    }
    #endregion
}
