using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UI_ItemTooltip : UI_Tooltip
{
    [SerializeField] private TextMeshProUGUI itemName;
    [SerializeField] private TextMeshProUGUI itemType;
    [SerializeField] private TextMeshProUGUI itemDescruiption;
    [SerializeField] private TextMeshProUGUI itemUniqe;

    private void Start()
    {
        gameObject.SetActive(false);
    }

    public void ShowTooltip(ItemData_Equipment itemData)
    {
        itemName.text = itemData.itemName;
        itemType.text = itemData.equipmentType.ToString();
        itemDescruiption.text = itemData.GetDescription();

        if (itemData.effectDescriptionText != null && itemData.effectDescriptionText.Length > 0)
            itemUniqe.text = "Uniqe effect: " + itemData.effectDescriptionText;

        gameObject.SetActive(true);
    }

    public void HideTolltip() => gameObject.SetActive(false);
}
