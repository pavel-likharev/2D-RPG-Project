using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar_UI : MonoBehaviour
{
    private Character character;
    private CharacterStats stats;

    private RectTransform myTransform;
    private Slider slider;

    private void Awake()
    {
        myTransform = GetComponent<RectTransform>();
        character = GetComponentInParent<Character>();
        stats = GetComponentInParent<CharacterStats>();
        slider = GetComponentInChildren<Slider>();
    }

    private void Start()
    {
        character.OnFlipped += Character_OnFlipped;
        stats.OnHealthChange += Stats_OnHealthChange;

        UpdateHealthUI();

        Debug.Log("UI called");
    }

    private void Stats_OnHealthChange(object sender, System.EventArgs e)
    {
        UpdateHealthUI();
    }

    private void Character_OnFlipped(object sender, System.EventArgs e)
    {
        FlipBar();
    }

    private void UpdateHealthUI()
    {
        slider.maxValue = stats.GetMaxHealtValue();
        slider.value = stats.currentHealth;
    }

    private void FlipBar()
    {
        myTransform.localScale = -myTransform.localScale;
    }

    private void OnDisable()
    {
        character.OnFlipped -= Character_OnFlipped;
        stats.OnHealthChange -= Stats_OnHealthChange;
    }
    
}
