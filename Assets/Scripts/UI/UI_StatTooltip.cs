using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UI_StatTooltip : UI_Tooltip
{
    [SerializeField] private TextMeshProUGUI description;

    private void Start()
    {
        gameObject.SetActive(false);
    }

    public void ShowTooltip(string text)
    {
        description.text = text;

        gameObject.SetActive(true);
    }

    public void HideTolltip() => gameObject.SetActive(false);
}
