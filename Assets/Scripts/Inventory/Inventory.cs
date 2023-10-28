using System.Collections;
using System.Collections.Generic;
using UnityEditorInternal.Profiling.Memory.Experimental;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public static Inventory Instance { get; private set; }

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

    private void UnequipItem(ItemData_Equipment oldEquipment)
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

    private void UpdateInventorySlotUI()
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

    private void UpdateStashSlotUI()
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

    private void UpdateEquipmentSlotUI()
    {
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
}
