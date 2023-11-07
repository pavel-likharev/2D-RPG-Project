using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UI_StatTooltip : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI description;

    public void ShowTooltip(string text)
    {
        description.text = text;

        gameObject.SetActive(true);
    }

    public void HideTolltip() => gameObject.SetActive(false);
}
