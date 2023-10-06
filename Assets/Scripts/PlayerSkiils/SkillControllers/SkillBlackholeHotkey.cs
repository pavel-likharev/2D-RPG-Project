using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SkillBlackholeHotkey : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    private KeyCode hotKey;
    private TextMeshProUGUI hotKeyText;

    private Transform enemy;
    private SkillBlackholeController blackhole;

    public void SetupHotKey(KeyCode hotKey, Transform enemy, SkillBlackholeController blackhole)
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        hotKeyText = GetComponentInChildren<TextMeshProUGUI>();

        this.enemy = enemy;
        this.blackhole = blackhole;

        this.hotKey = hotKey;
        hotKeyText.text = hotKey.ToString();
    }

    private void Update()
    {
        if (Input.GetKeyDown(hotKey))
        {
            blackhole.AddEnemyToList(enemy);

            hotKeyText.color = Color.clear;
            spriteRenderer.color = Color.clear;
        }
    }
}
