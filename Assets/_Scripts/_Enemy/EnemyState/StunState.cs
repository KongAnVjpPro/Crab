using UnityEngine;

public class StunState : EnemyState
{
    [SerializeField] private float stunDuration = 0.1f;
    [SerializeField] private float recoilForce = 5f;

    private float stunTimer = 0f;
    private bool isRecoiling = false;
    public override void Init(EnemyStateMachine machine)
    {
        base.Init(machine);
        stateID = EnemyStateID.Stunned;
    }

    public override void Enter()
    {
        base.Enter();
        stunTimer = 0f;
        isRecoiling = false;
        isComplete = false;
        ApplyRecoil();
    }

    public override void Do()
    {
        // Debug.Log("d");
        stunTimer += Time.deltaTime;

        if (stunTimer >= stunDuration)
        {
            // stateMachine.hitByAttack = false;
            isComplete = true;
        }
    }

    private void ApplyRecoil()
    {

        Vector2 recoilDirection = stateMachine.rb.velocity.normalized;
        stateMachine.rb.velocity = new Vector2(RecoilDir() * recoilForce, 0);


    }
    float RecoilDir()
    {
        return stateMachine.transform.position.x >= stateMachine.player.position.x ? 1 : -1;
    }

    public override void Exit()
    {
        base.Exit();
        stateMachine.rb.velocity = Vector2.zero;
        // stateMachine.hitByAttack = false;
    }
    public override EnemyStateID? CheckNextState()
    {
        float dist = Vector2.Distance(transform.position, stateMachine.player.position);
        if (dist <= 2.5f) return EnemyStateID.Attacking;
        if (dist <= 6f) return EnemyStateID.Chasing;
        return EnemyStateID.Patrolling;
    }

}
