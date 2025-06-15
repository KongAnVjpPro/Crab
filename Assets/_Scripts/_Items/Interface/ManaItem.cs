public class ManaItem : IUsableItem
{
    private int recoverAmount;
    public ManaItem(int recoverAmount)
    {
        this.recoverAmount = recoverAmount;
    }
    public void SetRecoverAmount(int recoverAmount)
    {
        this.recoverAmount = recoverAmount;
    }
    public void Use(EntityController targetUser, int amount = 1)
    {
        StatComponent stat = targetUser.GetComponent<StatComponent>();
        if (stat == null) return;
        if (stat.IsDead()) return;

        stat.RecoverMana(recoverAmount * amount);
    }
}