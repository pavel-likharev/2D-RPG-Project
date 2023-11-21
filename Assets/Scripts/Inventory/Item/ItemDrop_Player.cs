using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemDrop_Player : ItemDrop
{
    [Header("Player's drops")]
    [Range(0, 100)][SerializeField] private float chanceToLooseEquipment;
    [Range(0, 100)][SerializeField] private float chanceToLooseMaterials;

    public override void GenerateDrop()
    {
        Inventory inventory = Inventory.Instance;

        GenerateDropEquipment(inventory);
        GenerateDropStash(inventory);
    }

    private void GenerateDropEquipment(Inventory inventory)
    {
        List<InventoryItem> currentEquipment = inventory.GetEquipmentList();
        List<InventoryItem> itemsToUnequip = new List<InventoryItem>();

        foreach (InventoryItem item in currentEquipment)
        {
            if (Random.Range(0, 100) <= chanceToLooseEquipment)
            {
                DropItem(item.itemData);
                itemsToUnequip.Add(item);
            }
        }
        foreach (InventoryItem item in itemsToUnequip)
        {
            inventory.UnequipItem(item.itemData as ItemData_Equipment);
        }

        inventory.UpdateEquipmentSlotsUI();
    }

    private void GenerateDropStash(Inventory inventory)
    {
        List<InventoryItem> currentStash = inventory.GetStashList();
        List<InventoryItem> itemsToDrop = new List<InventoryItem>();

        foreach (InventoryItem item in currentStash)
        {
            if (Random.Range(0, 100) <= chanceToLooseMaterials)
            {
                DropItem(item.itemData);
                itemsToDrop.Add(item);
            }
        }
        foreach (InventoryItem item in itemsToDrop)
        {
            inventory.RemoveItemFromStash(item.itemData);
        }

        inventory.UpdateStashSlotsUI();
    }
}
