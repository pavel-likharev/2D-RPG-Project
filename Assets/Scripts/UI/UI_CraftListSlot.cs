using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UIElements;

public class UI_CraftListSlot : MonoBehaviour, IPointerDownHandler
{
    [SerializeField] private Transform craftSlotContainer;
    [SerializeField] private GameObject craftSlotPrefab;

    [SerializeField] private List<ItemData_Equipment> craftEquipment;

    private void Start()
    {
        transform.parent.GetChild(0).GetComponent<UI_CraftListSlot>().SetupCraftList();
    }

    private void ClearCraftSlots()
    {
        for (int i = 0; i < craftSlotContainer.childCount; i++)
        {
            Destroy(craftSlotContainer.GetChild(i).gameObject);
        }
    }

    public void SetupCraftList()
    {
        ClearCraftSlots();

        for (int i = 0; i < craftEquipment.Count; i++)
        {
            GameObject newSlot = Instantiate(craftSlotPrefab, craftSlotContainer);
            newSlot.GetComponent<UI_CraftSlot>().SetupCraftSlot(craftEquipment[i]);
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        SetupCraftList();
    }
}
