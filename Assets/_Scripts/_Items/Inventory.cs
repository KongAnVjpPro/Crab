using System;
using System.Collections.Generic;
using UnityEngine;
[Serializable]
public class Inventory
{
    [SerializeField] private List<Item> itemList;
    public Inventory()
    {
        itemList = new List<Item>();
        itemList.Add(new Item { itemType = Item.ItemType.equipment, amount = 1, itemName = "Stone" }); // Add a default item to the inventory
        itemList.Add(new Item { itemType = Item.ItemType.equipment, amount = 1, itemName = "Stone2" });
        // Debug.Log("Init");
    }
    public void AddItem(Item item)
    {
        itemList.Add(item);
    }
    public List<Item> GetItems()
    {
        return itemList;
    }
}