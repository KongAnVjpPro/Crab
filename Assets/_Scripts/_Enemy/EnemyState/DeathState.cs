using System.Collections;
using UnityEngine;

public class DeadState : EnemyState
{
    [SerializeField] bool canRevive;
    [SerializeField] float reviveTime = 5f;
    [SerializeField] float deathTime = 2f;
    public override void Init(EnemyStateMachine machine)
    {
        base.Init(machine);
        stateID = EnemyStateID.Dead;
    }

    public override void Enter()
    {
        base.Enter();
        if (!canRevive)
        {
            stateMachine.GetComponent<Collider2D>().enabled = false;
            stateMachine.rb.gravityScale = 0;


            //spawn something

            //destroy
        }
        stateMachine.HPBarFadeOut();
        isComplete = false;
        stateMachine.Drop();
        // if (canRevive) return;


    }

    public override void Do()
    {
        if (!canRevive)
        {
            StartCoroutine(WaitForDeathTime(deathTime));
        }
        if (isComplete) return;
        StartCoroutine(WaitForReviveTime());

    }
    IEnumerator WaitForReviveTime()
    {
        yield return new WaitForSeconds(reviveTime);
        isComplete = true;
    }
    IEnumerator WaitForDeathTime(float timer)
    {
        yield return new WaitForSeconds(timer);
        stateMachine.gameObject.SetActive(false);
    }

    public override void Exit()
    {
        base.Exit();

    }
    public override EnemyStateID? CheckNextState()
    {
        if (canRevive)
        {
            return EnemyStateID.Patrolling;
        }
        // return EnemyStateID.Patrolling;
        return null;
    }

}
