using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UI_EquipmentSlot : UI_ItemSlot
{
    public EquipmentType slotType;

    private void OnValidate()
    {
        gameObject.name = "Equipment Slot " + slotType; 
    }

    public override void OnPointerDown(PointerEventData eventData)
    {
        Inventory.Instance.AddItem(item.itemData);
        Inventory.Instance.UnequipItem(item.itemData as ItemData_Equipment);
        CleanUp();

    }
}
