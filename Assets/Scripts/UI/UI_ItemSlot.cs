using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_ItemSlot : MonoBehaviour, IPointerDownHandler, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private Image image;
    [SerializeField] private TextMeshProUGUI itemText;
    [SerializeField] private Color defaultColor;

    public InventoryItem item;

    public void UpdateSlot(InventoryItem newItem)
    {
        item = newItem;
        image.color = defaultColor;

        if (item != null)
        {
            image.color = Color.white;
            image.sprite = item.itemData.icon;
            itemText.text = item.stackSize > 1 ? item.stackSize.ToString() : "";
        }
    }

    public void CleanUp()
    {
        item = null; 
        image.sprite = null;
        image.color = defaultColor;


        itemText.text = "";
    }

    public virtual void OnPointerDown(PointerEventData eventData)
    {
        if (item != null)
        {
            if (Input.GetKey(KeyCode.LeftControl))
            {
                Inventory.Instance.RemoveItem(item.itemData);
                return;
            }

        
            if (item.itemData.itemType == ItemType.Equipment)
            {
                Inventory.Instance.EquipItem(item.itemData);
            }
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (item == null || item.itemData == null)
            return;

        UI.Instance.itemTooltip.ShowTooltip(item.itemData as ItemData_Equipment);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (item == null || item.itemData == null)
            return;

        UI.Instance.itemTooltip.HideTolltip();
    }
}
