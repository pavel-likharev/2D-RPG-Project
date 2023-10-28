using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemObject : MonoBehaviour
{
    [SerializeField] private ItemData item;

    private void OnValidate()
    {
        gameObject.name = "Item object " + item.name;
        GetComponent<SpriteRenderer>().sprite = item.icon;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<Player>() != null)
        {
            Inventory.Instance.AddItem(item);
            Destroy(gameObject);
        }
    }
}
