using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_SkillTreeSlot : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private string skillName;
    [TextArea][SerializeField] private string skillDescription;

    [SerializeField] private Color lockedColor;

    private Image skillImage;
    private string skillPriceText;
    
    private void OnValidate()
    {
        gameObject.name = "SkillTreeSlot_" + skillName;
    }

    private void Start()
    {
        skillImage = GetComponent<Image>();
        skillImage.color = lockedColor;
    }

    public void UnlockSlot()
    {
        skillImage.color = Color.white;
    }

    public void SetPriceText(int price) => skillPriceText = "Price: " + price.ToString();

    public void OnPointerEnter(PointerEventData eventData)
    {

        UI.Instance.MenuController.skillTooltip.SetTooltipPosition();
        UI.Instance.MenuController.skillTooltip.ShowTooltip(skillName, skillDescription, skillPriceText);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        UI.Instance.MenuController.skillTooltip.HideTooltip();
    }
}
