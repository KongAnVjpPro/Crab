using UnityEngine;

[CreateAssetMenu(fileName = "New Heal Item", menuName = "Inventory/Item/Heal Item")]
public class HealItemSO : ItemEffectSO
{
    public int healAmount = 10;
    public override IUsableItem GetIUsable()
    {
        return new HealthItem(healAmount);
    }
}