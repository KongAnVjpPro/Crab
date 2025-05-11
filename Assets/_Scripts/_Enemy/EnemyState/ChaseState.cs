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
        // Vector2 direction = (stateMachine.player.position - transform.position).normalized;
        Vector2 dirToTarget = stateMachine.DirecionToPlayer().normalized;

        stateMachine.Flip((dirToTarget.x >= 0) ? EnemyRotator.FlipDirection.Up : EnemyRotator.FlipDirection.Down);
        stateMachine.RotateZ(dirToTarget);


        // stateMachine.rb.velocity = new Vector2(direction.x * speed, stateMachine.rb.velocity.y);
        stateMachine.MoveHorizontal(dirToTarget, 1, speed);
        stateMachine.MoveVertical(dirToTarget, 1, speed);

        float distance = Vector2.Distance(transform.position, stateMachine.player.position);
        if (distance <= 3f || distance > 6f)
        {
            isComplete = true;
        }
    }
    public override EnemyStateID? CheckNextState()
    {
        float dist = Vector2.Distance(transform.position, stateMachine.player.position);
        if (dist <= 2.5f) return EnemyStateID.Attacking;
        if (dist <= 6f) return EnemyStateID.Chasing;
        return EnemyStateID.Patrolling;

    }
}
