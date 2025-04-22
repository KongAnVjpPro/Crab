using UnityEngine;

public class AttackState : EnemyState
{
    public float attackCooldown = 1.0f;
    private bool hasAttacked;
    private float timer;
    void Awake()
    {

    }
    public override void Init(EnemyStateMachine machine)
    {
        base.Init(machine);
        stateID = EnemyStateID.Attacking;
    }
    public override void Enter()
    {
        base.Enter();
        timer = 0f;
        hasAttacked = false;
        stateMachine.rb.velocity = Vector2.zero;
    }

    public override void Do()
    {
        stateMachine.rb.velocity = Vector2.zero;
        timer += Time.deltaTime;
        hasAttacked = false;
        if (!hasAttacked && timer >= attackCooldown)
        {
            Debug.Log("Enemy attacks!");
            hasAttacked = true;
            isComplete = true;
            stateMachine.OnStateChanged.Invoke(this.stateID);
            timer = 0;
        }
    }
}
