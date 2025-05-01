using Cinemachine.Utility;
using UnityEngine;

public class PatrolState : EnemyState
{
    private float timer;
    public float patrolDuration = 3f;
    public float speed = 2f;
    [SerializeField] Vector3 patrolCenter;
    [SerializeField] float patrolRadius;
    [SerializeField] float obstacleCheckDistance = 0.5f;
    private int moveDirection = 1;
    public bool returnCenter = false;
    public override void Enter()
    {
        base.Enter();
        timer = 0f;
        returnCenter = true;
    }

    public override void Do()
    {
        // Vector2 dirToTarget = stateMachine.DirecionToPlayer();
        // dirToTarget.x >= 0 ? stateMachine.Flip(EnemyRotator.FlipDirection.Right) : stateMachine.Flip(EnemyRotator.FlipDirection.Left);
        timer += Time.deltaTime;
        if (timer >= patrolDuration)
        {
            isComplete = true;
        }
        float distanceToCenter = Vector3.Distance(transform.position, patrolCenter);

        if (distanceToCenter > patrolRadius)
        {

            if (returnCenter)
            {
                stateMachine.Flip(EnemyRotator.FlipDirection.Up);
                ReturnToCenter();
                return;
            }
            FlipDirection();
        }
        returnCenter = false;

        if (IsObstacleAhead())
        {
            FlipDirection();
        }


        // stateMachine.Flip((dirToTarget.x >= 0) ? EnemyRotator.FlipDirection.Up : EnemyRotator.FlipDirection.Down);

        // stateMachine.RotateZ(new Vector2(1, 0));


        Vector2 moveVector = new Vector2(moveDirection, 0);
        stateMachine.rb.velocity = moveVector * speed;
        stateMachine.RotateZ(moveVector);
        stateMachine.Flip(moveDirection == 1 ? EnemyRotator.FlipDirection.Up : EnemyRotator.FlipDirection.Down);
        // stateMachine.rb.velocity = new Vector2(speed, stateMachine.rb.velocity.y);


    }
    void ReturnToCenter()
    {
        Vector3 dirToCenter = (patrolCenter - transform.position).normalized;
        stateMachine.RotateZ(new Vector2(dirToCenter.x, dirToCenter.y));
        stateMachine.Flip(dirToCenter.x >= 0 ? EnemyRotator.FlipDirection.Up : EnemyRotator.FlipDirection.Down);
        stateMachine.rb.velocity = dirToCenter * speed;
    }
    private bool IsObstacleAhead()
    {
        Vector2 origin = transform.position;
        Vector2 direction = new Vector2(moveDirection, 0);
        RaycastHit2D hit = Physics2D.Raycast(origin, direction, obstacleCheckDistance, LayerMask.GetMask("Ground"));
        return hit.collider != null;
    }
    private void FlipDirection()
    {
        moveDirection *= -1;
    }
}
