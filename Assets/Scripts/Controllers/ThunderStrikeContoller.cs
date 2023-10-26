using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThunderStrikeContoller : MonoBehaviour
{
    [SerializeField] private CharacterStats target;
    private float speed = 25;
    private int damage;

    private Animator animator;
    private bool isTriggered;

    private void Start()
    {
        animator = GetComponentInChildren<Animator>();
    }

    public void Setup(CharacterStats target, int damage)
    {
        this.target = target;
        this.damage = damage;
    }

    private void Update()
    {
        if (!target)
            return;
        

        if (isTriggered)
            return;

        transform.position = Vector2.MoveTowards(transform.position, target.transform.position, speed * Time.deltaTime);
        transform.right = transform.position - target.transform.position;

        if (Vector2.Distance(transform.position, target.transform.position) < 0.1f)
        {
            animator.transform.localPosition = new Vector3(0, 0.5f);
            animator.transform.localRotation = Quaternion.identity;

            transform.rotation = Quaternion.identity;
            transform.localScale = new Vector3(3, 3);

            Invoke("DamageAndSelfDestroy", 0.2f);
            isTriggered = true;
            animator.SetTrigger("Hit");
        }
    }

    private void DamageAndSelfDestroy()
    {
        target.ApplyShockEffect(true);
        target.TakeDamage(damage, 0);
        Destroy(gameObject, 0.4f);
    }
}
