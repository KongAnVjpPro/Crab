using UnityEngine;
public class EnemyEntity : EntityController
{
    public StatComponent enemyStat;
    public GroundCheck groundCheck;
    public WallCheck wallCheck;
    public EnemyStateMachine state;
    public EnemyAnimator enemyAnimator;
    public MovementComponent enemyMove;
    public EnemyRotator enemyRotator;
    public Recoil enemyRecoil;
    public EnemyDrop enemyDrop;
    public EnemyHealthBar enemyHealthBar;
    protected override void LoadComponents()
    {
        base.LoadComponents();
        this.LoadStat();
        this.LoadState();
        this.LoadWallCheck();
        this.LoadGroundCheck();
        this.LoadAnimator();
        this.LoadMovement();
        this.LoadRotator();
        this.LoadRecoil();
        this.LoadDrop();
        this.LoadHealthBar();
    }
    protected virtual void LoadStat()
    {
        if (this.enemyStat != null) return;
        this.enemyStat = GetComponent<StatComponent>();
    }
    protected virtual void LoadState()
    {
        if (this.state != null) return;
        this.state = GetComponent<EnemyStateMachine>();
    }
    protected virtual void LoadGroundCheck()
    {
        if (this.groundCheck != null) return;
        this.groundCheck = GetComponent<GroundCheck>();
    }
    protected virtual void LoadWallCheck()
    {
        if (this.wallCheck != null) return;
        this.wallCheck = GetComponent<WallCheck>();
    }
    protected virtual void LoadAnimator()
    {
        if (this.enemyAnimator != null) return;
        this.enemyAnimator = GetComponent<EnemyAnimator>();
    }
    protected virtual void LoadMovement()
    {
        if (this.enemyMove != null) return;
        this.enemyMove = GetComponent<MovementComponent>();
    }
    protected virtual void LoadRotator()
    {
        if (this.enemyRotator != null) return;
        this.enemyRotator = GetComponent<EnemyRotator>();
    }
    protected virtual void LoadRecoil()
    {
        if (this.enemyRecoil != null) return;
        this.enemyRecoil = GetComponent<Recoil>();
    }
    protected virtual void LoadDrop()
    {
        if (this.enemyDrop != null) return;
        this.enemyDrop = GetComponent<EnemyDrop>();
    }
    protected virtual void LoadHealthBar()
    {
        if (this.enemyHealthBar != null) return;
        this.enemyHealthBar = GetComponent<EnemyHealthBar>();
    }
}