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
    }
    public void AddItem(Item item)
    {
        itemList.Add(item);
    }
}