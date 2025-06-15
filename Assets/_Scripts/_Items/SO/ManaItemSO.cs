using UnityEngine;

[CreateAssetMenu(fileName = "New Mana Item", menuName = "Inventory/Item/Mana Item")]
public class ManaItemSO : ItemEffectSO
{
    public int manaAmount = 10;
    public override IUsableItem GetIUsable()
    {
        return new ManaItem(manaAmount);
    }
}