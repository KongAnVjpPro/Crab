using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
public class BossWhip : EnemyState

{
    public List<RangeAttack> bulletPool;

    public RangeAttack shockWave;

    public float radiusFire = 3f;

    public override void Init(EnemyStateMachine machine)
    {
        base.Init(machine);
        stateID = EnemyStateID.SeaWeedWhip;
    }
    public override void Enter()
    {
        base.Enter();
        stateMachine.collie.enabled = false;
        float rate = Random.Range(0f, 1f);
        if (rate >= 0.5f)
        {
            ShockWave();
        }
        else
        {
            SpikeRain();

        }
    }
    public void ShockWave()
    {
        shockWave.gameObject.SetActive(false);
        Vector2 dir = PlayerEntity.Instance.transform.position - stateMachine.transform.position;
        shockWave.gameObject.SetActive(true);
        shockWave.Fire(stateMachine.transform.position, dir.normalized);
    }
    public void SpikeRain()
    {
        foreach (var spike in bulletPool)
        {
            spike.gameObject.SetActive(false);
            // float x = Random.Range(-1 * radiusFire, radiusFire);
            // float y = Mathf.Sqrt(Mathf.Pow(radiusFire, 2) - Mathf.Pow(x, 2));
            Vector2 offset = Random.insideUnitCircle * radiusFire;
            // float x = offset.x;
            spike.transform.position = stateMachine.transform.position + new Vector3(offset.x, offset.y, 0);

            spike.gameObject.SetActive(true);
            spike.Fire(spike.transform.position, offset);


        }
    }


    public override void Do()
    {
        base.Do();
        isComplete = true;
    }
    public override void Exit()
    {
        base.Exit();
        stateMachine.collie.enabled = true;
    }

    public override EnemyStateID? CheckNextState()
    {
        if (stateMachine.IsDead())
        {
            return EnemyStateID.SeaWeedPrepareDeath;
        }
        return EnemyStateID.Patrolling;
    }
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(stateMachine.transform.position, radiusFire);
    }
}