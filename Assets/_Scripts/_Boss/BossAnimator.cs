using System.Collections;
using Ink.Parsed;
using UnityEngine;
public class BossAnimator : EnemyAnimator
{
    int hurtID = Animator.StringToHash("Hurt");
    int deathID = Animator.StringToHash("Death");
    int preDeath = Animator.StringToHash("PreDeath");
    int talk = Animator.StringToHash("Talk");
    int whipVine = Animator.StringToHash("WhipVine");
    int preWhip = Animator.StringToHash("Pre_WhipVine");
    int tieUp = Animator.StringToHash("TieUp");
    int sumMinion = Animator.StringToHash("SummonWeed");
    int phase2 = Animator.StringToHash("Phase2");
    int weedStab = Animator.StringToHash("WeedStab");
    int move = Animator.StringToHash("Move");
    /*
    flow: talk => weeed stab or whip vine, weed stab co 2 loai la ban dan hoac summon gai
    phase 2 ; weed stab, prewhipvine->whipvine,summon weed, tie up



    */
    protected override void OnEnable()
    {
        // base.OnEnable();
        StartCoroutine(WaitForController());
    }
    IEnumerator WaitForController()
    {
        while (enemyController == null)
        {
            yield return null;
        }
        while (enemyController.state == null)
        {
            yield return null;
        }
        enemyController.state.OnStateChanged += HandleStateChange;
        Debug.Log("exist");
    }
    protected override void HandleStateChange(EnemyStateID stateID)
    {
        // base.HandleStateChange(stateID);
        switch (stateID)
        {
            case EnemyStateID.Patrolling:
                ResetAll();
                break;
            case EnemyStateID.Stunned:
                Stunned();
                break;
            // case EnemyStateID.KnockBacked
            case EnemyStateID.SeaWeedPrepareDeath:
                ResetAll();
                PreDeath(true);
                break;
            case EnemyStateID.SeaWeedPhaseTwo:
                PhaseTwo(true);
                break;
            case EnemyStateID.SeaWeedPrepareWhip:
                PrepareWhip(true);
                break;
            case EnemyStateID.SeaWeedStab:
                WeedStab(true);
                break;
            case EnemyStateID.SeaWeedWhip:
                WhipVine();
                break;
            case EnemyStateID.SeaWeedTieUp:
                TieUp(true);
                break;
            case EnemyStateID.SeaWeedSummon:
                SummonMinion(true);
                break;
            case EnemyStateID.SeaWeedTalk:
                Talk(true);
                break;
            case EnemyStateID.Dead:
                Death();
                break;
        }
    }
    void ResetAll()
    {
        WeedStab(false);
        SummonMinion(false);
        TieUp(false);
        Move(false);
    }
    public void Move(bool val)
    {
        animator.SetBool(move, val);
    }
    public override void Stunned()
    {
        base.Stunned();
        animator.SetTrigger(hurtID);
    }
    public override void Death()
    {
        base.Death();
        animator.SetTrigger(deathID);
    }
    public void PreDeath(bool val)
    {
        animator.SetBool(preDeath, val);
    }
    public void Talk(bool val)
    {
        animator.SetBool(talk, val);
    }
    public void WhipVine()
    {
        PrepareWhip(false);
        animator.SetTrigger(whipVine);
    }
    public void PrepareWhip(bool val)
    {
        animator.SetBool(preWhip, val);
    }
    public void TieUp(bool val)
    {
        animator.SetBool(tieUp, val);
    }
    public void SummonMinion(bool val)
    {
        animator.SetBool(sumMinion, val);
    }
    public void PhaseTwo(bool val)
    {
        animator.SetBool(phase2, val);
    }
    public void WeedStab(bool val)
    {
        animator.SetBool(weedStab, val);
    }
}