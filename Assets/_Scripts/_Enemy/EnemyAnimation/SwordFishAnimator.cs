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
    public override void Death()
    {
        animator.SetTrigger("Death");
    }
    public override void Stunned()
    {
        animator.SetTrigger("Stunned");
    }
    //for rangeAttack crea
    public override void PrepareRangeAttack()
    {
        base.PrepareRangeAttack();
        animator.SetTrigger("RangeAttack");
    }
    public override void Block()
    {
        base.Block();
        animator.SetTrigger("Block");
    }

}