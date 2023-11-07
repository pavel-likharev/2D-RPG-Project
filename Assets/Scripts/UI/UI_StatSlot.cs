using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Windows;

public class UI_StatSlot : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private StatType statType;
    [SerializeField] private string statName;
    [SerializeField] private TextMeshProUGUI statValueText;
    [SerializeField] private TextMeshProUGUI statNameText;

    [TextArea]
    [SerializeField] private string description;

    private void OnValidate()
    {
        gameObject.name = "Stat_" + statType;

        if (statName != null)
            statNameText.text = statName;
    }

    private void Start()
    {
        UpdateStatValueUI();
    }

    public void UpdateStatValueUI()
    {
        PlayerStats playerStats = PlayerManager.Instance.Player.GetComponent<PlayerStats>();

        if (playerStats)
        {
            switch (statType)
            {
                case StatType.damage:
                    statValueText.text = (playerStats.damage.GetValue() + playerStats.strength.GetValue()).ToString();
                    break;
                case StatType.critChance:
                    statValueText.text = (playerStats.critChance.GetValue() + playerStats.agility.GetValue()).ToString();
                    break;
                case StatType.critPower:
                    statValueText.text = (playerStats.critPower.GetValue() + playerStats.strength.GetValue()).ToString();
                    break;
                case StatType.health:
                    statValueText.text = playerStats.GetMaxHealtValue().ToString();
                    Debug.Log(playerStats.GetMaxHealtValue().ToString());
                    break;
                case StatType.evasion:
                    statValueText.text = (playerStats.evasion.GetValue() + playerStats.agility.GetValue()).ToString();
                    break;
                case StatType.magicResistance:
                    statValueText.text = (playerStats.magicResistance.GetValue() + playerStats.intelligence.GetValue() * 3).ToString();
                    break;

                default:
                    statValueText.text = playerStats.GetStatFromType(statType).GetValue().ToString();
                    break;
            }
        }

    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        UI.Instance.statTooltip.ShowTooltip(description);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        UI.Instance.statTooltip.HideTolltip();
    }
}