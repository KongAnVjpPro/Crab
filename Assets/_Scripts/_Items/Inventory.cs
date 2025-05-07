using System;
using System.Collections.Generic;
using UnityEngine;
[Serializable]
public class Inventory
{
    [SerializeField] private List<ItemData> itemList;
    public Inventory()
    {
        itemList = new List<ItemData>();
        // itemList.Add(new ItemD { itemType = ItemSO.ItemType.equipment, amount = 1, itemName = "Stone" }); // Add a default item to the inventory
        // itemList.Add(new ItemSO { itemType = ItemSO.ItemType.equipment, amount = 1, itemName = "Stone2" });
        // Debug.Log("Init");
    }
    public void AddItem(ItemData item)
    {
        ItemData newItem = FindItem(item.itemSO);
        if (newItem != null)
        {
            newItem.amount += item.amount;
            return;
        }
        itemList.Add(item);
    }
    public List<ItemData> GetItems()
    {
        return itemList;
    }
    ItemData FindItem(ItemSO itemSO)
    {
        foreach (var item in itemList)
        {
            if (item.itemSO == itemSO)
            {
                return item;
            }
        }
        return null;
    }
}