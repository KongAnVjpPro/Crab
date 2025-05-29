using UnityEngine;
public class BossStun : StunState
{
    [SerializeField]
    float phase2Rate = 0.5f;
    public override void Enter()
    {
        SetAttackLayer();
        // SetTalkLayer();
        stunTimer = 0f;
        isRecoiling = false;
        isComplete = false;
        ApplyRecoil();
    }
    public override void Exit()
    {

    }
    public override EnemyStateID? CheckNextState()
    {
        if (stateMachine.IsDead()) return EnemyStateID.SeaWeedPrepareDeath;
        float hpRate = stateMachine.enemyEntity.enemyStat.CurrentHealth / stateMachine.enemyEntity.enemyStat.TotalHealth;

        if (hpRate > phase2Rate) return EnemyStateID.Patrolling;
        if (hpRate <= phase2Rate && !stateMachine.isOnPhase2) return EnemyStateID.SeaWeedPhaseTwo;
        return EnemyStateID.Patrolling;
    }
}