using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_ItemSlot : MonoBehaviour, IPointerDownHandler
{
    [SerializeField] private Image image;
    [SerializeField] private TextMeshProUGUI itemText;

    public InventoryItem item;

    public void UpdateSlot(InventoryItem newItem)
    {
        item = newItem;
        image.color = Color.white;

        if (item != null)
        {
            image.sprite = item.itemData.icon;
            itemText.text = item.stackSize > 1 ? item.stackSize.ToString() : "";
        }
    }

    public void CleanUp()
    {
        item = null; 
        image.sprite = null;
        image.color = Color.clear;

        itemText.text = "";
    }

    public virtual void OnPointerDown(PointerEventData eventData)
    {
        if (Input.GetKey(KeyCode.LeftControl))
        {
            Inventory.Instance.RemoveItem(item.itemData);
            return;
        }

            if (item.itemData != null)
            {
                if (item.itemData.itemType == ItemType.Equipment)
                {
                    Inventory.Instance.EquipItem(item.itemData);
                }
            }
        
    }
}
