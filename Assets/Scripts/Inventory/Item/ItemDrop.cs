using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemDrop : MonoBehaviour
{
    [SerializeField] private GameObject dropPrefab;
    [SerializeField] private ItemData[] possibleDrops;
    [SerializeField] private int amountItems;

    private List<ItemData> dropList;

    private void Start()
    {
        dropList = new List<ItemData>();
    }

    public virtual void GenerateDrop()
    {
        for (int i = 0; i < possibleDrops.Length; i++)
        {
            if (Random.Range(0, 100) <= possibleDrops[i].dropChance)
            {
                dropList.Add(possibleDrops[i]);
            }
        }

        if (dropList.Count > 0)
        {
            for (int i = 0; i < amountItems; i++)
            {
                ItemData randomItem = dropList[Random.Range(0, dropList.Count - 1)];

                dropList.Remove(randomItem);
                DropItem(randomItem);

                if (dropList.Count <= 0)
                    break;
            }
        }
    }

    protected void DropItem(ItemData item)
    {
        
        GameObject newDrop = Instantiate(dropPrefab, transform.position, Quaternion.identity);

        Vector2 randomVelocity = new Vector2(Random.Range(-5, 5), Random.Range(15, 20));

        newDrop.GetComponent<ItemObject>().SetupItem(item, randomVelocity);
    }
}
