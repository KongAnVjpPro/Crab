using UnityEngine;
public class BossTieUp : EnemyState
{
    [SerializeField] float timer = 0;
    [SerializeField] float time = 3f;
    [SerializeField] SeaWeedTieUp tieUp;

    public override void Init(EnemyStateMachine machine)
    {
        base.Init(machine);
        stateID = EnemyStateID.SeaWeedTieUp;
    }
    public override void Enter()
    {
        base.Enter();
        SetTalkLayer();
        timer = 0;
        stateMachine.collie.enabled = false;
    }
    public override void Do()
    {
        base.Do();
        timer += Time.deltaTime;
        if (timer < time / stateMachine.bossBoost)
        {

            return;
        }
        tieUp.transform.position = new Vector2(PlayerEntity.Instance.transform.position.x, tieUp.transform.position.y);
        tieUp.gameObject.SetActive(true);
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
        // return base.CheckNextState();
        if (stateMachine.IsDead())
        {
            return EnemyStateID.SeaWeedPrepareDeath;
        }
        return EnemyStateID.Patrolling;
    }
}