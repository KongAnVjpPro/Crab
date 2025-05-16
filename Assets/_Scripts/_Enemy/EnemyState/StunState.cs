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
        stateMachine.HPBarFadeIn();
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

        // Vector2 recoilDirection = stateMachine.rb.velocity.normalized;
        Vector2 recoilDirection = new Vector2(RecoilDirX(), RecoilDirY()) * recoilForce;
        // stateMachine.rb.velocity = ;
        if (stateMachine.isSwimming)
        {
            stateMachine.rb.velocity = recoilDirection;
        }
        else
        {
            stateMachine.rb.velocity = new Vector2(recoilDirection.x, 0);
        }
        // stateMachine.RecoilBoth(new Vector2(-RecoilDir(), (stateMachine.transform.position.y >= stateMachine.player.position.y) ? 1f : -1f));

    }

    float RecoilDirX()
    {
        return stateMachine.transform.position.x >= stateMachine.player.position.x ? 1 : -1;
    }

    float RecoilDirY()
    {
        return stateMachine.transform.position.y >= stateMachine.player.position.y ? 1 : -1;
    }


    public override void Exit()
    {
        base.Exit();
        stateMachine.rb.velocity = Vector2.zero;
        // stateMachine.hitByAttack = false;
    }
    public override EnemyStateID? CheckNextState()
    {
        if (stateMachine.IsDead()) return EnemyStateID.Dead;
        float dist = Vector2.Distance(transform.position, stateMachine.player.position);
        if (dist <= 2.5f) return EnemyStateID.Attacking;
        if (dist <= 6f) return EnemyStateID.Chasing;
        return EnemyStateID.Patrolling;
    }

}
