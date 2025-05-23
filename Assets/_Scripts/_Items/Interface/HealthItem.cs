public class HealthItem : IUsableItem
{
    private int healAmount;
    public HealthItem(int healAmount)
    {
        this.healAmount = healAmount;
    }
    public void SetHealAmount(int healAmount)
    {
        this.healAmount = healAmount;
    }
    public void Use(EntityController targetUser, int amount = 1)
    {
        StatComponent stat = targetUser.GetComponent<StatComponent>();
        if (stat == null) return;
        if (stat.IsDead()) return;

        stat.Heal(healAmount * amount);
    }
}