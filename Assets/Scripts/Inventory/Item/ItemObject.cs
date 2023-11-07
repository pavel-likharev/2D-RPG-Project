using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemObject : MonoBehaviour
{
    [SerializeField] private ItemData itemData;

    private Rigidbody2D rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        if (itemData == null)
            return;
        
        gameObject.name = "ItemObject_" + itemData.name;
        GetComponent<SpriteRenderer>().sprite = itemData.icon;
    }

    public void SetupItem(ItemData itemData, Vector2 velocity)
    {
        this.itemData = itemData;
        rb.velocity = velocity;
    }

    public void PickupItem()
    {
        if (!Inventory.Instance.CanAddItemToInventory() &&
            itemData.itemType == ItemType.Equipment &&
            !Inventory.Instance.inventoryDictionary.ContainsKey(itemData))
        {
            rb.velocity = new Vector2 (0, 5);
            return;
        }
        
        Inventory.Instance.AddItem(itemData);
        Destroy(gameObject);
        
    }
}
