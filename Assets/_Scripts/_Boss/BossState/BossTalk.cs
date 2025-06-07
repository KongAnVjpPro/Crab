using UnityEngine;
public class BossTalk : EnemyState
{


    public override void Init(EnemyStateMachine machine)
    {
        base.Init(machine);
        stateID = EnemyStateID.SeaWeedTalk;
    }
    public override void Enter()
    {
        base.Enter();

        SetTalkLayer();
    }
    public override void Exit()
    {
        base.Exit();

        if (stateMachine.IsDead())
        {
            isComplete = true;
            return;
        }
        isComplete = true;
        SetAttackLayer();
    }

    public override EnemyStateID? CheckNextState()
    {
        // return base.CheckNextState();
        if (stateMachine.IsDead()) return EnemyStateID.Dead;
        return EnemyStateID.Patrolling;
    }
}