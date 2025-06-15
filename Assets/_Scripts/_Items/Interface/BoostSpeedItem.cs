public class BoostSpeedItem : IUsableItem
{
    // private int recoverAmount;
    private float moveBoost;
    private float jumpBoost;
    private float durationBoost;
    public BoostSpeedItem(float moveBoost, float jumpBoost, float durationBoost)
    {
        this.moveBoost = moveBoost;
        this.jumpBoost = jumpBoost;
        this.durationBoost = durationBoost;
    }
    // public void SetRecoverAmount(int recoverAmount)
    // {
    //     this.recoverAmount = recoverAmount;
    // }
    public void Use(EntityController targetUser, int amount = 1)
    {
        StatComponent target = targetUser.GetComponent<StatComponent>();
        PlayerMovement stat = targetUser.GetComponent<PlayerMovement>();
        if (stat == null || target == null) return;
        if (target.IsDead()) return;
        for (int i = 0; i < amount; i++)
        {
            stat.BoostInTime(moveBoost, jumpBoost, durationBoost);
        }

        // stat.RecoverMana(recoverAmount * amount);
    }
}