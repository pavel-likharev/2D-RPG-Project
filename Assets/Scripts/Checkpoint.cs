using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    private const string IS_ACTIVE = "IsActive";

    private Animator animator;

    public string id;
    public bool Activated { get; private set; }

    [ContextMenu("Generate ID")]
    private void GenerateId()
    {
        id = System.Guid.NewGuid().ToString();
    }

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (Activated)
            return;
        
        if (collision.GetComponent<Player>() != null)
        {
            ActivateCheckpoint();
        }
    }

    public void ActivateCheckpoint()
    {
        AudioManager.Instance.PlaySFX(5, transform);

        Activated = true;
        animator.SetBool(IS_ACTIVE, true);
    }
}
