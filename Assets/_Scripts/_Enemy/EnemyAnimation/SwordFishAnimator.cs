using UnityEngine;
public class SwordFishAnimator : EnemyAnimator
{
    public override void ResetTrigger()
    {
        base.ResetTrigger();
        animator.ResetTrigger("Attacking");
        animator.ResetTrigger("Patrolling");
        animator.ResetTrigger("Chasing");
    }
    public override void Attacking()
    {
        base.Attacking();
        animator.SetTrigger("Attacking");
    }
    public override void Patrolling()
    {
        base.Patrolling();
        animator.SetTrigger("Patrolling");
    }
    public override void Chasing()
    {
        base.Chasing();
        animator.SetTrigger("Chasing");
    }
}