using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillClone : Skill
{
    [SerializeField] private GameObject clonePrefab;
    [SerializeField] private float cloneDuration;
    [SerializeField] private bool isAttack;

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

    public void CreateClone(Transform clonePosition, Vector2 offset)
    {
        GameObject newClone = Instantiate(clonePrefab);

        newClone.GetComponent<SkillCloneController>().SetupClone(clonePosition, cloneDuration, isAttack, offset);
    }
}
