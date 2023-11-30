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

    private Image skillImage => GetComponent<Image>();
    private string skillPriceText;

    public bool Unlocked { get; private set; }

    private void Awake()
    {
        if (!Unlocked)
        {
            skillImage.color = lockedColor;
        }
    }

    private void OnValidate()
    {
        gameObject.name = "SkillTreeSlot_" + skillName;

    }

    private void Start()
    {

    }

    public void UnlockSlot()
    {
        skillImage.color = Color.white;
        Unlocked = true;

        GetComponent<Button>().enabled = false;
    }

    public string GetName() => skillName;

    public void LockSlot()
    {
        GetComponent<Button>().enabled = true;
        Unlocked = false;
        skillImage.color = lockedColor;
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
