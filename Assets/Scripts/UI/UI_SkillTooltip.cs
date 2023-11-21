using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UI_SkillTooltip : UI_Tooltip
{
    [SerializeField] private TextMeshProUGUI skillName;
    [SerializeField] private TextMeshProUGUI description;
    [SerializeField] private TextMeshProUGUI price;

    private void Start()
    {
        gameObject.SetActive(false);
    }

    public void ShowTooltip(string nameText, string descriptionText, string priceText)
    {
        skillName.text = nameText;
        description.text = descriptionText;
        price.text = priceText;
        gameObject.SetActive(true);
    }

    public void HideTooltip()
    {
        gameObject.SetActive(false);
    }
}
