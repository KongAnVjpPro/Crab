using UnityEngine;

[CreateAssetMenu(fileName = "New Boost Speed Item", menuName = "Inventory/Item/Boost Speed Item")]
public class BoostSpeedItemSO : ItemEffectSO
{
    // public int manaAmount = 10;
    public float scaledMove = 1.2f;
    public float scaledJump = 1.2f;
    public float durationBoost = 30f;
    public override IUsableItem GetIUsable()
    {
        return new BoostSpeedItem(scaledMove, scaledJump, durationBoost);
    }
}