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
    public ItemEffectSO itemEffectSO;
    public string itemName;

    public Sprite itemSprite;
    public string itemDescription;
    public int maxStack = 10;

}
