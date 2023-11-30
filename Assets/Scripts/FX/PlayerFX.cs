using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerFX : CharacterFX
{


    [Header("Screen shake")]
    private CinemachineImpulseSource screenShake;
    [SerializeField] private float shakeMultiplier;
    private Vector3 shakePower;
    [SerializeField] private Vector3 shakePowerOnCatchSword;
    [SerializeField] private Vector3 shakePowerOnHugeDamage;

    [Header("AfterImage")]
    [SerializeField] private GameObject afterPrefab;
    [SerializeField] private float afterImageLooseRate;
    [SerializeField] private float afterImageCooldown;
    private float afterImageTimer;

    [SerializeField] private ParticleSystem dustFx;

    protected override void Awake()
    {
        base.Awake();

        screenShake = GetComponent<CinemachineImpulseSource>();
    }

    protected override void Update()
    {
        base.Update();

        if (afterImageTimer >= 0)
        {
            afterImageTimer -= Time.deltaTime;
        }
    }



    private void ScreenShake(Vector3 shakePower)
    {
        screenShake.m_DefaultVelocity = new Vector3(shakePower.x * PlayerManager.Instance.Player.MoveDir, shakePower.y) * shakeMultiplier;
        screenShake.GenerateImpulse();
    }

    public void ScreenShakeOnCatchSword() => ScreenShake(shakePowerOnCatchSword);
    public void ScreenShakeOnHugeDamage() => ScreenShake(shakePowerOnHugeDamage);

    public void CreateAfterImage()
    {
        if (afterImageTimer <= 0)
        {
            afterImageTimer = afterImageCooldown;
            GameObject newAfterDashImage = Instantiate(afterPrefab, transform.position, Quaternion.identity);
            if (GetComponentInParent<Player>().MoveDir == -1)
            {
                newAfterDashImage.GetComponent<SpriteRenderer>().flipX = true;
            }

            newAfterDashImage.GetComponent<AfterImageFX>().SetupFX(afterImageLooseRate, spriteRenderer.sprite);
        }
    }

    public void PlayDustFX()
    {
        dustFx?.Play();
    }
}
