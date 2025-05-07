using UnityEngine;


[CreateAssetMenu(fileName = "New Item", menuName = "Inventory/Item")]
public class ItemSO : ScriptableObject
{

    public enum ItemType
    {
        equipment,
        consumable,
        questItem,
    }
    public ItemType itemType;
    public ItemEffectType itemEffectType = ItemEffectType.none;
    // public int amount;
    public string itemName;

    public Sprite itemSprite;
    public string itemDescription;
    public int maxStack = 10;

}
public enum ItemEffectType
{
    none = 0,
    heal = 1,
    damage = 2,
    buff = 3,
    debuff = 4,
    teleport = 5,
    spawn = 6,


}