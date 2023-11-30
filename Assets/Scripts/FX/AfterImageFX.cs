using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AfterImageFX : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    private float colorLooseRate;

    public void SetupFX(float looseRate, Sprite sprite)
    {
        spriteRenderer = GetComponent<SpriteRenderer>();

        spriteRenderer.sprite = sprite;
        colorLooseRate = looseRate;
    }

    private void Update()
    {
        float alpha = spriteRenderer.color.a - colorLooseRate * Time.deltaTime;
        spriteRenderer.color = new Color(spriteRenderer.color.r, spriteRenderer.color.g, spriteRenderer.color.b, alpha);

        if (spriteRenderer.color.a <= 0)
        {
            Destroy(gameObject);
        }
    }
}
