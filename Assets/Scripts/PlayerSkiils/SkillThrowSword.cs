using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillThrowSword : Skill
{
    [SerializeField] private GameObject swordPrefab;
    [SerializeField] private Vector2 swordLaunchForce;
    [SerializeField] private float swordGravity;

    private Vector2 finalDir;
    
    [Header("Aim dots")]
    [SerializeField] private GameObject dotPrefab;
    [SerializeField] private Transform dotsParent;
    [SerializeField] private int numberOfDots;
    [SerializeField] private float spaceBetweenDots;

    private GameObject[] dots;

    protected override void Start()
    {
        base.Start();

        GenerateDots();
    }

    protected override void Update()
    {
        base.Update();

        if (Input.GetKeyUp(KeyCode.Mouse1))
        {
            finalDir = new Vector2(AimDirection().normalized.x * swordLaunchForce.x, AimDirection().normalized.y * swordLaunchForce.y);
        }

        if (Input.GetKey(KeyCode.Mouse1))
        {
            for (int i = 0; i < numberOfDots; i++)
            {
                dots[i].transform.position = DotsPosition(i * spaceBetweenDots);
            }
        }
    }

    public void CreateSword()
    {
        GameObject newSword = Instantiate(swordPrefab, player.transform.position, transform.rotation);
        SkillThrowSwordController skillController = newSword.GetComponent<SkillThrowSwordController>();

        skillController.SetupSword(finalDir, swordGravity, player);
        player.AssignNewSword(newSword);

        DotsActive(false);
    }

    public Vector2 AimDirection()
    {
        Vector2 playerPosition = player.transform.position;
        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 direction = mousePosition - playerPosition;

        return direction;
    }

    public void DotsActive(bool isActive)
    {
        foreach (GameObject dot in dots)
        {
            dot.SetActive(isActive);
        }
    }

    private void GenerateDots()
    {
        dots = new GameObject[numberOfDots];

        for (int i = 0; i < numberOfDots; i++)
        {
            dots[i] = Instantiate(dotPrefab, player.transform.position, Quaternion.identity, dotsParent);
            dots[i].SetActive(false);
        }
    }

    private Vector2 DotsPosition(float t)
    {
        Vector2 position = (Vector2)player.transform.position + new Vector2(
            AimDirection().normalized.x * swordLaunchForce.x, AimDirection().normalized.y * swordLaunchForce.y) *
            t + 0.5f * (Physics2D.gravity * swordGravity) * (t * t);

        return position;
    }
}
