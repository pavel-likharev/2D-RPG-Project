using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UI_SkillTooltip : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI skillName;
    [SerializeField] private TextMeshProUGUI description;

    private void Start()
    {
        gameObject.SetActive(false);
    }

    public void ShowTooltip(string nameText, string descriptionText)
    {
        skillName.text = nameText;
        description.text = descriptionText; 
        gameObject.SetActive(true);
    }

    public void HideTooltip()
    {
        gameObject.SetActive(false);
    }
}
