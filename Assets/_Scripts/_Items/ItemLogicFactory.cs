public static class ItemLogicFactory
{
    public static IUsableItem CreateUsableItem(ItemSO itemSO)
    {
        switch (itemSO.itemEffectType)
        {
            case ItemEffectType.none:
                return null;
            case ItemEffectType.heal:
                return new HealthItem(1);
            case ItemEffectType.damage:
            // return new DamageItem(itemSO.itemEffectType);
            case ItemEffectType.buff:
            // return new BuffItem(itemSO.itemEffectType);
            case ItemEffectType.debuff:
            // return new DebuffItem(itemSO.itemEffectType);
            case ItemEffectType.teleport:
            // return new TeleportItem(itemSO.itemEffectType);
            case ItemEffectType.spawn:
            // return new SpawnItem(itemSO.itemEffectType);
            default:
                return null;
        }
    }
}