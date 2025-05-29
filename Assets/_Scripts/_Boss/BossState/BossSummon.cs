using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
public class BossSummon : EnemyState
{
    // [SerializeField] List<EnemyEntity> minions;
    [SerializeField] List<ParticleSystem> minionsAppearFX;
    [SerializeField] float summonTimer = 0;
    [SerializeField] float summonLasting = 5;
    public override void Init(EnemyStateMachine machine)
    {
        base.Init(machine);
        stateID = EnemyStateID.SeaWeedSummon;
    }
    public override void Enter()
    {
        // SetAttackLayer();
        SetTalkLayer();
        stateMachine.collie.enabled = false;
        base.Enter();
        foreach (var particle in minionsAppearFX)
        {
            particle.Play();
        }
    }
    public override void Do()
    {
        base.Do();
        summonTimer += Time.deltaTime;
        if (summonTimer <= summonLasting)
        {
            stateMachine.impulseSource.GenerateImpulse();
            return;
        }
        isComplete = true;
        foreach (var minion in stateMachine.minions)
        {
            minion.gameObject.SetActive(true);
        }
        foreach (var fx in minionsAppearFX)
        {
            fx.Stop();
        }
        summonTimer = 0;
    }
    public override void Exit()
    {
        base.Exit();
        SetAttackLayer();
        stateMachine.collie.enabled = true;
    }
    public override EnemyStateID? CheckNextState()
    {
        return EnemyStateID.Patrolling;
    }

}