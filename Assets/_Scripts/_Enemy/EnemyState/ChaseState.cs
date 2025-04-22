using UnityEngine;

public class ChaseState : EnemyState
{
    public float speed = 3f;

    void Awake()
    {

    }
    public override void Init(EnemyStateMachine machine)
    {
        base.Init(machine);
        stateID = EnemyStateID.Chasing;
    }
    public override void Do()
    {
        Vector2 direction = (stateMachine.player.position - transform.position).normalized;
        stateMachine.rb.velocity = new Vector2(direction.x * speed, stateMachine.rb.velocity.y);

        float distance = Vector2.Distance(transform.position, stateMachine.player.position);
        if (distance <= 1.5f || distance > 6f)
        {
            isComplete = true;
        }
    }
}
