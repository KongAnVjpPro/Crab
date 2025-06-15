using Unity.VisualScripting;
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
        if (stateMachine.isSwimming)
        {
            stateMachine.Flip((dirToTarget.x >= 0) ? EnemyRotator.FlipDirection.Up : EnemyRotator.FlipDirection.Down);
            stateMachine.RotateZ(dirToTarget);
            stateMachine.MoveVertical(dirToTarget, 1, speed);
        }
        else
        {
            stateMachine.Flip(dirToTarget.x >= 0 ? EnemyRotator.FlipDirection.Right : EnemyRotator.FlipDirection.Left);
        }



        // stateMachine.rb.velocity = new Vector2(direction.x * speed, stateMachine.rb.velocity.y);

        stateMachine.MoveHorizontal(dirToTarget, 1, speed);


        float distance = Vector2.Distance(transform.position, stateMachine.player.position);
        if (distance <= 3f || distance > 6f)
        {
            isComplete = true;
        }
    }
    public override EnemyStateID? CheckNextState()
    {
        if (!PlayerEntity.Instance.pState.alive)
        {
            return EnemyStateID.Patrolling;
        }
        float dist = Vector2.Distance(transform.position, stateMachine.player.position);
        if (dist <= stateMachine.closeCombatRange) return EnemyStateID.Attacking;
        if (dist <= 6f) return EnemyStateID.Chasing;
        return EnemyStateID.Patrolling;

    }
}
