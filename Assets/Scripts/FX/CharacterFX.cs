using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CharacterFX : MonoBehaviour
{
    protected SpriteRenderer spriteRenderer;

    [Header("TextPopup")]
    [SerializeField] private GameObject textPopupPrefab;

    [Header("Flash FX")]
    [SerializeField] private Material hitMaterial;
    [SerializeField] private float flashDuration = 0.2f;
    private Material originalMaterial;

    [Header("Ailment colors")]
    [SerializeField] private Color[] chillColor;
    [SerializeField] private Color[] igniteColor;
    [SerializeField] private Color[] shockColor;

    [SerializeField] private ParticleSystem chillFX;
    [SerializeField] private ParticleSystem igniteFX;
    [SerializeField] private ParticleSystem shockFX;

    [SerializeField] private GameObject hitFx;
    [SerializeField] private GameObject criticalHitFx;

    protected virtual void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    protected virtual void Start()
    {
        originalMaterial = spriteRenderer.material;

    }

    protected virtual void Update()
    {
    }

    public void CreatePopupText(string text)
    {
        float randomX = Random.Range(-1, 1);
        float randomY = Random.Range(1, 3);

        Vector3 positionOffset = new Vector3(randomX, randomY, 0);

        GameObject newTextPopup = Instantiate(textPopupPrefab, transform.position + positionOffset, Quaternion.identity);

        newTextPopup.GetComponent<TextMeshPro>().text = text;
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

    private IEnumerator HitColorFX()
    {
        spriteRenderer.material = hitMaterial;
        Color currentColor = spriteRenderer.color;
        spriteRenderer.color = Color.white;

        yield return new WaitForSeconds(flashDuration);

        spriteRenderer.color = currentColor;
        spriteRenderer.material = originalMaterial;
    }

    public void CreateHitFX(Transform target, bool critical)
    {
        float zRotation = Random.Range(-45, 45);

        float xPosition = Random.Range(-0.5f, 0.5f);
        float yPosition = Random.Range(-0.5f, 0.5f);

        Vector3 hitFxPosition = new Vector3(xPosition, yPosition);
        Vector3 hitFXRotation = new Vector3(0, 0, zRotation);

        GameObject hitPrefab;

        if (critical)
        {
            hitPrefab = criticalHitFx;
        }
        else
        {
            hitPrefab = hitFx;
        }

        GameObject newHitFx = Instantiate(hitPrefab, target.position + hitFxPosition, Quaternion.identity);
        newHitFx.transform.Rotate(hitFXRotation);
        newHitFx.transform.localScale = new Vector3(GetComponentInParent<Character>().MoveDir, newHitFx.transform.localScale.y, newHitFx.transform.localScale.z);

        Destroy(newHitFx, 0.5f);
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

        igniteFX.Stop();
        chillFX.Stop();
        shockFX.Stop();
    }

    public void IgniteFx(float seconds)
    {
        InvokeRepeating("IgniteColorFx", 0, flashDuration);
        Invoke("CancelColor", seconds);

        igniteFX.Play();
    }

    public void ChillFx(float seconds)
    {
        InvokeRepeating("ChillColorFx", 0, flashDuration);
        Invoke("CancelColor", seconds);

        chillFX.Play();
    }

    public void ShockFx(float seconds)
    {
        InvokeRepeating("ShockColorFx", 0, flashDuration);
        Invoke("CancelColor", seconds);

        shockFX.Play();
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
