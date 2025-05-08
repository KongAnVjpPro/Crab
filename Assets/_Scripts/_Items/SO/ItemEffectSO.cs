using UnityEngine;

// [CreateAssetMenu(fileName = "New Item Effect", menuName = "Inventory/Item Effect")]
public abstract class ItemEffectSO : ScriptableObject
{
    public abstract IUsableItem GetIUsable();
}