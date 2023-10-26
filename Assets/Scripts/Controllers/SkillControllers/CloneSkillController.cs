using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloneSkillController : SkillController
{
    [Header("Clone info")]
    [SerializeField] private GameObject clonePrefab;
    [SerializeField] private float cloneDuration;
    [SerializeField] private bool isAttack;

    [Header("Spawn info")]
    [SerializeField] private bool isCloneOnDashStart;
    [SerializeField] private bool isCloneOnDashEnd;
    [SerializeField] private bool canCreateCloneOnCounterAttack;

    [Header("Diplicate info")]
    [SerializeField] private bool canDuplicate;
    [SerializeField] private float chanceDuplicate;

    [Header("Clone from crystal")]
    public bool canCrystalFromClone;

    private float xOffset = 2f;
    private float createDelay = 0.4f;

    protected override void Awake()
    {
        base.Awake();
    }

    protected override void Start()
    {
        base.Start();
    }

    protected override void Update()
    {
        base.Update();
    }

    protected override void UseSkill()
    {
        base.UseSkill();
    }

    public void CreateClone(Transform clonePosition, Vector3 offset)
    {
        if (canCrystalFromClone)
        {
            SkillManager.Instance.CrystalSkillController.CreateCrystal();
            return;
        }

        GameObject newClone = Instantiate(clonePrefab, clonePosition.position + offset, Quaternion.identity);

        newClone.GetComponent<CloneSkill>().SetupClone(cloneDuration, isAttack, FindClosestEnemy(newClone.transform), canDuplicate, chanceDuplicate);
    }

    public void CreateCloneOnDashStart()
    {
        if (isCloneOnDashStart)
        {
            CreateClone(player.transform, Vector2.zero);
        }
    }

    public void CreateCloneOnDashEnd()
    {
        if (isCloneOnDashEnd)
        {
            CreateClone(player.transform, Vector2.zero);
        }
    }

    public void CreateCloneOnCounterAttack(Transform enemy)
    {
        if (canCreateCloneOnCounterAttack)
        {
            StartCoroutine(CreateCloneWithDelay(enemy, new Vector3(xOffset * player.MoveDir, 0)));
        }
    }

    private IEnumerator CreateCloneWithDelay(Transform enemy, Vector3 offset)
    {
        yield return new WaitForSeconds(createDelay);
            CreateClone(enemy.transform, offset);
    }
}
