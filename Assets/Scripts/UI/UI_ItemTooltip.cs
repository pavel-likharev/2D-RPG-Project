using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UI_ItemTooltip : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI itemName;
    [SerializeField] private TextMeshProUGUI itemType;
    [SerializeField] private TextMeshProUGUI itemDescruiption;

    private void Start()
    {
        gameObject.SetActive(false);
    }

    public void ShowTooltip(ItemData_Equipment itemData)
    {
        itemName.text = itemData.itemName;
        itemType.text = itemData.equipmentType.ToString();
        itemDescruiption.text = itemData.GetDescription();

        gameObject.SetActive(true);
    }

    public void HideTolltip() => gameObject.SetActive(false);
}
