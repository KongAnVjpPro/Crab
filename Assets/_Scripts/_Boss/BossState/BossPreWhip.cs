using UnityEngine;
public class BossPreWhip : EnemyState
{
    [SerializeField] float minPrepareTime = 3f;
    [SerializeField] float maxprepareTime = 5f;
    [SerializeField] float timer = 0;
    [SerializeField] float currentPrepareTime;
    public override void Init(EnemyStateMachine machine)
    {
        base.Init(machine);
        stateID = EnemyStateID.SeaWeedPrepareWhip;
    }
    public override void Enter()
    {
        base.Enter();
        stateMachine.collie.enabled = false;
        SetTalkLayer();
        timer = 0;
        currentPrepareTime = Random.Range(minPrepareTime, maxprepareTime) / stateMachine.bossBoost;
    }

    public override void Do()
    {
        base.Do();
        timer += Time.deltaTime;
        stateMachine.Flip(PlayerEntity.Instance.transform.position.x > stateMachine.transform.position.x ? EnemyRotator.FlipDirection.Right : EnemyRotator.FlipDirection.Left);
        if (timer < currentPrepareTime)
        {
            stateMachine.impulseSource.GenerateImpulse();
            return;
        }
        timer = 0;
        isComplete = true;
    }


    public override void Exit()
    {
        base.Exit();
        timer = 0;
        SetAttackLayer();
        stateMachine.collie.enabled = true;
    }
    public override EnemyStateID? CheckNextState()
    {
        return EnemyStateID.SeaWeedWhip;
    }

}