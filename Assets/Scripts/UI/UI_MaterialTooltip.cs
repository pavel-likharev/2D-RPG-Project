using TMPro;
using UnityEngine;

public class UI_MaterialTooltip : UI_Tooltip
{
    [SerializeField] private TextMeshProUGUI itemName;
    [SerializeField] private TextMeshProUGUI itemDescruiption;

    private void Start()
    {
        gameObject.SetActive(false);
    }

    public void ShowTooltip(ItemData itemData)
    {
        itemName.text = itemData.itemName;
        itemDescruiption.text = itemData.GetDescription();

        gameObject.SetActive(true);
    }

    public void HideTolltip() => gameObject.SetActive(false);
}