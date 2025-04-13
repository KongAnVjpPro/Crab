using UnityEngine;
public class EnemyEntity : EntityController
{
    public StatComponent enemyStat;
    protected override void LoadComponents()
    {
        base.LoadComponents();
        this.LoadStat();
    }
    protected virtual void LoadStat()
    {
        if (this.enemyStat != null) return;
        this.enemyStat = GetComponent<StatComponent>();
    }
}