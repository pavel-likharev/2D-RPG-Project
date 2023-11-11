using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_ItemSlot : MonoBehaviour, IPointerDownHandler, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] protected Image image;
    [SerializeField] protected TextMeshProUGUI itemText;
    [SerializeField] protected Color defaultColor;

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

            UI.Instance.MenuController.itemTooltip.HideTolltip();
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (item == null || item.itemData == null)
            return;

        if (item.itemData.itemType == ItemType.Equipment)
        {
            Vector2 mousePosition = Input.mousePosition;

            UI.Instance.MenuController.itemTooltip.transform.position = new Vector2(mousePosition.x - 200, mousePosition.y + 200);
            UI.Instance.MenuController.itemTooltip.ShowTooltip(item.itemData as ItemData_Equipment);
        }

        if (item.itemData.itemType == ItemType.Material)
        {
            Vector2 mousePosition = Input.mousePosition;

            UI.Instance.MenuController.materialTooltip.transform.position = new Vector2(mousePosition.x, mousePosition.y + 100);
            UI.Instance.MenuController.materialTooltip.ShowTooltip(item.itemData);
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (item == null || item.itemData == null)
            return;

        if (item.itemData.itemType == ItemType.Equipment)
            UI.Instance.MenuController.itemTooltip.HideTolltip();

        if (item.itemData.itemType == ItemType.Material)
            UI.Instance.MenuController.materialTooltip.HideTolltip();
    }

    private void SetTooltipPosition(Transform tooltip)
    {
        Vector2 mousePosition = Input.mousePosition;

        tooltip.position = new Vector2(mousePosition.x - 100, mousePosition.y + 100);
    }
}
