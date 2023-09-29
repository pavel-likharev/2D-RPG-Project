using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterFX : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;

    [SerializeField] private Material hitMaterial;
    SerializeField] private float flashDuration = 0.2f;
    private Material originalMaterial;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Start()
    {
        originalMaterial = spriteRenderer.material;
    }

    private IEnumerator HitFX()
    {
        spriteRenderer.material = hitMaterial;

        yield return new WaitForSeconds(flashDuration);

        spriteRenderer.material = originalMaterial;
    }

}