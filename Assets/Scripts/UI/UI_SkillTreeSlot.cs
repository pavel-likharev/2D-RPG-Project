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

    public void OnPointerEnter(PointerEventData eventData)
    {
        Vector2 mousePosition = Input.mousePosition;

        float xOffset;
        float yOffset;

        if (mousePosition.x > 600)
            xOffset = -150;
        else
            xOffset = 150;

        if (mousePosition.y > 300)
            yOffset = -150;
        else
            yOffset = 150;

        UI.Instance.MenuController.skillTooltip.transform.position = new Vector2(mousePosition.x + xOffset, mousePosition.y + yOffset);

        UI.Instance.MenuController.skillTooltip.ShowTooltip(skillName, skillDescription);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        UI.Instance.MenuController.skillTooltip.HideTooltip();
    }
}
