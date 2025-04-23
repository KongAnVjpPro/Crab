using Cinemachine.Utility;
using UnityEngine;

public class PatrolState : EnemyState
{
    private float timer;
    public float patrolDuration = 3f;
    public float speed = 2f;
    [SerializeField] Vector3 patrolCenter;
    [SerializeField] float patrolRadius;

    public override void Enter()
    {
        base.Enter();
        timer = 0f;
    }

    public override void Do()
    {
        // Vector2 dirToTarget = stateMachine.DirecionToPlayer();
        // dirToTarget.x >= 0 ? stateMachine.Flip(EnemyRotator.FlipDirection.Right) : stateMachine.Flip(EnemyRotator.FlipDirection.Left);


        // stateMachine.Flip((dirToTarget.x >= 0) ? EnemyRotator.FlipDirection.Up : EnemyRotator.FlipDirection.Down);
        stateMachine.RotateZ(new Vector2(1, 0));
        stateMachine.Flip(EnemyRotator.FlipDirection.Up);
        timer += Time.deltaTime;
        stateMachine.rb.velocity = new Vector2(speed, stateMachine.rb.velocity.y);

        if (timer >= patrolDuration)
        {
            isComplete = true;
        }
    }
    void ReturnToCenter()
    {
        Vector3 dirToCenter = (patrolCenter - transform.position).normalized;
        stateMachine.RotateZ(new Vector2(dirToCenter.x, dirToCenter.y));
        stateMachine.Flip(dirToCenter.x >= 0 ? EnemyRotator.FlipDirection.Up : EnemyRotator.FlipDirection.Down);

    }
}
