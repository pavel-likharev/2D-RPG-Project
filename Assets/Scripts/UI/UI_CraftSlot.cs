using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UI_CraftSlot : UI_ItemSlot
{
    public void SetupCraftSlot(ItemData_Equipment data)
    {
        if (data == null)
            return;

        item.itemData = data;

        image.sprite = data.icon;
        itemText.text = data.itemName;
    }

    public override void OnPointerDown(PointerEventData eventData)
    {
        ItemData_Equipment craftData = item.itemData as ItemData_Equipment;
        
        UI.Instance.MenuController.craftingWindow.SetupCraftWindow(craftData);
    }
}
