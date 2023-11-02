using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemObject_Trigger : MonoBehaviour
{
    private ItemObject myItemObject;

    private void Start()
    {
        myItemObject = GetComponentInParent<ItemObject>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<Player>() != null)
        {
            if (!collision.GetComponent<CharacterStats>().IsDead)
                myItemObject.PickupItem();
        }
    }
}
