using UnityEngine;
public class Item
{


    public enum ItemType
    {
        equipment,
        consumable,
        questItem,
    }
    public ItemType itemType;
    public int amount;
    public string itemName;
}