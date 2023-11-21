using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_CraftingWindow : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI itemName;
    [SerializeField] private TextMeshProUGUI itemDescription;
    [SerializeField] private Image itemImage;
    [SerializeField] private Button craftButton;

    [SerializeField] private Image[] materialImages;
    [SerializeField] private Color defaultColor;

    public void SetupCraftWindow(ItemData_Equipment data)
    {
        craftButton.onClick.RemoveAllListeners();

        ClearCraftWindow();

        for (int i = 0; i < data.craftingMaterials.Count; i++)
        {
            if (data.craftingMaterials.Count > materialImages.Length)
                Debug.LogWarning("Crafting materials more tnah maximum slots in crafting window. Check correct amount crafting materials.");

            materialImages[i].sprite = data.craftingMaterials[i].itemData.icon;
            materialImages[i].color = Color.white;

            TextMeshProUGUI amountText = materialImages[i].GetComponentInChildren<TextMeshProUGUI>();

            amountText.text = data.craftingMaterials[i].stackSize.ToString();
        }

        itemImage.sprite = data.icon;
        itemImage.color = Color.white;
        itemName.text = data.itemName;
        itemDescription.text = data.GetDescription();

        craftButton.onClick.AddListener(() => Inventory.Instance.CanCraft(data, data.craftingMaterials));
    }

    public void ClearCraftWindow()
    {
        foreach (var image in materialImages)
        {
            image.sprite = null;
            image.color = defaultColor;
            image.GetComponentInChildren<TextMeshProUGUI>().text = "";
        }

        itemImage.sprite = null;
        itemName.text = "";
        itemDescription.text = "";
    }
}
