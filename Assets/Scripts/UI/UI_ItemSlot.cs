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
            SetTooltipPosition(UI.Instance.MenuController.itemTooltip.transform);
            UI.Instance.MenuController.itemTooltip.ShowTooltip(item.itemData as ItemData_Equipment);
        }

        if (item.itemData.itemType == ItemType.Material)
        {
            SetTooltipPosition(UI.Instance.MenuController.materialTooltip.transform);
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

        float xOffset = 0;
        float yOffset = 0;

        if (mousePosition.x > 600)
            xOffset = -200;
        else
            xOffset = 200;

        if (mousePosition.y > 300)
            yOffset = -50;
        else
            yOffset = 50;

        tooltip.position = new Vector2(mousePosition.x + xOffset, mousePosition.y + yOffset);
    }
}
