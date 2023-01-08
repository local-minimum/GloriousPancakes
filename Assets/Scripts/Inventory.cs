using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    InventorySlot[] slots;
    int nextSlot = 0;

    [SerializeField]
    Sprite harvestedItem;

    void Start()
    {
        slots = GetComponentsInChildren<InventorySlot>();
    }

    public void AddToInventory()
    {
        if (nextSlot >= slots.Length) return;

        slots[nextSlot].Slot(harvestedItem);

        nextSlot++;
    }
}
