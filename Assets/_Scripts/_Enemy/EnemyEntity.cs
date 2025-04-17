using UnityEngine;
public class EnemyEntity : EntityController
{
    public StatComponent enemyStat;
    public GroundCheck groundCheck;
    public WallCheck wallCheck;
    public EnemyState state;
    protected override void LoadComponents()
    {
        base.LoadComponents();
        this.LoadStat();
        this.LoadState();
    }
    protected virtual void LoadStat()
    {
        if (this.enemyStat != null) return;
        this.enemyStat = GetComponent<StatComponent>();
    }
    protected virtual void LoadState()
    {
        if (this.state != null) return;
        this.state = GetComponent<EnemyState>();
    }
}