using UnityEngine;
public class EnemyAnimator : EnemyComponent
{
    [SerializeField] Animator animator;
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
    public virtual void Attacking()
    {

    }
    public virtual void Chasing()
    {

    }
    public virtual void Patrolling()
    {

    }
}