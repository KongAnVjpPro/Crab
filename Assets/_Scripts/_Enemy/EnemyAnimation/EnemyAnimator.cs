using System.Diagnostics;
using UnityEngine;
public class EnemyAnimator : EnemyComponent
{
    [SerializeField] protected Animator animator;
    protected virtual void OnEnable()
    {
        enemyController.state.OnStateChanged += HandleStateChange;
    }
    protected virtual void OnDisable()
    {
        enemyController.state.OnStateChanged -= HandleStateChange;
    }
    protected override void LoadComponents()
    {
        base.LoadComponents();
        this.LoadAnimator();
    }
    protected virtual void LoadAnimator()
    {
        if (this.animator != null) return;
        this.animator = GetComponent<Animator>();
    }
    protected virtual void HandleStateChange(EnemyStateID stateID)
    {
        ResetTrigger();
        switch (stateID)
        {
            case EnemyStateID.Patrolling:
                Patrolling();
                break;
            case EnemyStateID.Chasing:
                Chasing();
                break;
            case EnemyStateID.Attacking:
                Attacking();
                break;
            case EnemyStateID.Stunned:
                Stunned();
                break;

            case EnemyStateID.Dead:
                Death();
                break;

        }
    }
    public virtual void ResetTrigger()
    {

    }
    public virtual void Attacking()
    {

    }
    public virtual void Chasing()
    {

    }
    public virtual void Patrolling()
    {

    }
    public virtual void Stunned()
    {

    }
    public virtual void Death()
    {

    }
}