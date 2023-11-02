using System.Collections.Generic;
using UnityEngine;

public class BlackholeSkill : MonoBehaviour
{
    public bool CanExitState { get; private set; }

    [SerializeField] private GameObject hotKeyPrefab;
    [SerializeField] private List<KeyCode> keyCodeList;

    // Blackhole properties
    private float maxSize = 10;
    private float growSpeed = 0.2f;
    private bool canGrow;
    private float blackholeTimer;
    private float shrinkSpeed;
    private bool canShrink;

    // Clone properties
    private bool canCloneAttack;
    private int attackAmount;
    private float cloneAttackCooldown;
    private float cloneAttackTimer;

    private List<Transform> targets = new List<Transform>();
    private List<GameObject> createdHotKeys = new List<GameObject>();
    private bool canCreateHotKey = true;
    private bool isSkillActive;

    private void Update()
    {
        BlackholeTimer();

        CloneAttackLogic();

        BlackholeBehavior();

        if (Input.GetKeyDown(KeyCode.R) && !isSkillActive && targets.Count > 0)
        {
            isSkillActive = true;
            ReleaseCloneAttack();
        }
    }

    private void BlackholeBehavior()
    {
        if (canGrow && !canShrink)
        {
            transform.localScale = Vector2.Lerp(transform.localScale, new Vector2(maxSize, maxSize), growSpeed);
        }

        if (canShrink)
        {
            transform.localScale = Vector2.Lerp(transform.localScale, new Vector2(-1, -1), shrinkSpeed * Time.deltaTime);

            if (transform.localScale.x < 0)
            {
                Destroy(gameObject);
            }
        }
    }

    private void BlackholeTimer()
    {
        if (blackholeTimer >= 0)
            blackholeTimer -= Time.deltaTime;


        if (blackholeTimer <= 0 && !isSkillActive)
        {
            isSkillActive = true;

            if (targets.Count > 0)
                ReleaseCloneAttack();
            else
                FinishedSkill();
        }
    }

    private void ReleaseCloneAttack()
    {
        DestroyHotKeys();
        canCloneAttack = true;
        canCreateHotKey = false;

        if (!SkillManager.Instance.CloneSkillController.canCrystalFromClone)
            PlayerManager.Instance.Player.CharacterFX.MakeTransparent(true);
    }

    private void CloneAttackLogic()
    {
        cloneAttackTimer -= Time.deltaTime;

        if (canCloneAttack && cloneAttackTimer <= 0 && attackAmount > 0)
        {
            cloneAttackTimer = cloneAttackCooldown;

            int randomIndex = Random.Range(0, targets.Count);

            float xOffset;

            if (Random.Range(0, 100) > 50)
            {
                xOffset = 1;
            }
            else
            {
                xOffset = -1;
            }

            if (SkillManager.Instance.CloneSkillController.canCrystalFromClone)
            {
                SkillManager.Instance.CrystalSkillController.CreateCrystal();
                SkillManager.Instance.CrystalSkillController.CurrentCrystalChooseRandomTarget();
            }
            else
            {
                SkillManager.Instance.CloneSkillController.CreateClone(targets[randomIndex], new Vector2(xOffset, 0));
            }

            attackAmount--;

            if (attackAmount == 0)
            {
                Invoke("FinishedSkill", 0.75f);
            }
        }
    }

    private void FinishedSkill()
    {
        DestroyHotKeys();
        canCloneAttack = false;
        canShrink = true;
        Player player = PlayerManager.Instance.Player;

        CanExitState = true;
    }

    private void DestroyHotKeys()
    {
        foreach (GameObject hotKey in createdHotKeys)
        {
            Destroy(hotKey);
        }
    }

    public void SetupBlackhole(float maxSize, float growSpeed, float shrinkSpeed, int attackAmount, float cloneAttackCooldown, float blackholeDuration)
    {
        this.maxSize = maxSize;
        this.growSpeed = growSpeed;
        this.shrinkSpeed = shrinkSpeed;
        this.attackAmount = attackAmount;
        this.cloneAttackCooldown = cloneAttackCooldown;
        this.blackholeTimer = blackholeDuration;

        canGrow = true;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<Enemy>() != null)
        {
            collision.GetComponent<Enemy>().FreezeTime(true);

            CteateHotKey(collision);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.GetComponent<Enemy>() != null)
        {
            collision.GetComponent<Enemy>().FreezeTime(false);
        }
    }

    private void CteateHotKey(Collider2D collision)
    {
        if (keyCodeList.Count <= 0)
        {
            Debug.LogWarning("Not enoght hot keys in a key code list");
            return;
        }

        if (!canCreateHotKey)
        {
            return;
        }

        GameObject newHotKey = Instantiate(hotKeyPrefab, collision.transform.position + new Vector3(0, 2), Quaternion.identity);
        createdHotKeys.Add(newHotKey);


        KeyCode choosenKey = keyCodeList[Random.Range(0, keyCodeList.Count)];
        keyCodeList.Remove(choosenKey);

        BlackholeSkillHotkey blackholeHotkeyScript = newHotKey.GetComponent<BlackholeSkillHotkey>();
        blackholeHotkeyScript.SetupHotKey(choosenKey, collision.transform, this);
    }

    public void AddEnemyToList(Transform enemy) => targets.Add(enemy);
}
