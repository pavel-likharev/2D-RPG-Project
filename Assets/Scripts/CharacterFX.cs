using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterFX : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;

    [Header("Flash FX")]
    [SerializeField] private Material hitMaterial;
    [SerializeField] private float flashDuration = 0.2f;
    private Material originalMaterial;

    [Header("Ailment colors")]
    [SerializeField] private Color[] chillColor;
    [SerializeField] private Color[] igniteColor;
    [SerializeField] private Color[] shockColor;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Start()
    {
        originalMaterial = spriteRenderer.material;
    }

    public void MakeTransparent(bool isTransparent)
    {
        if (isTransparent)
        {
            spriteRenderer.color = Color.clear;
        }
        else
        {
            spriteRenderer.color = Color.white;
        }
    }

    private IEnumerator HitFX()
    {
        spriteRenderer.material = hitMaterial;
        Color currentColor = spriteRenderer.color;
        spriteRenderer.color = Color.white;

        yield return new WaitForSeconds(flashDuration);

        spriteRenderer.color = currentColor;
        spriteRenderer.material = originalMaterial;
    }

    private void RedColorBlink()
    {
        
        if (spriteRenderer.color != Color.white)
        {
            spriteRenderer.color = Color.white;
        }
        else
        {
            spriteRenderer.color = Color.red;
        }
    }

    private void CancelColor()
    {
        CancelInvoke();
        spriteRenderer.color = Color.white;
    }

    public void IgniteFx(float seconds)
    {
        InvokeRepeating("IgniteColorFx", 0, flashDuration);
        Invoke("CancelColor", seconds);
    }

    public void ChillFx(float seconds)
    {
        InvokeRepeating("ChillColorFx", 0, flashDuration);
        Invoke("CancelColor", seconds);
    }

    public void ShockFx(float seconds)
    {
        InvokeRepeating("ShockColorFx", 0, flashDuration);
        Invoke("CancelColor", seconds);
    }

    private void IgniteColorFx()
    {
        if (spriteRenderer.color != igniteColor[0])
            spriteRenderer.color = igniteColor[0];
        else
            spriteRenderer.color = igniteColor[1];
    }

    public void ChillColorFx()
    {
        if (spriteRenderer.color != chillColor[0])
            spriteRenderer.color = chillColor[0];
        else
            spriteRenderer.color = chillColor[1];
    }

    public void ShockColorFx()
    {
        if (spriteRenderer.color != shockColor[0])
            spriteRenderer.color = shockColor[0];
        else
            spriteRenderer.color = shockColor[1];
    }
}
