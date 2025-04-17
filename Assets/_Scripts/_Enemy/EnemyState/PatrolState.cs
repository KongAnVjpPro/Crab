using UnityEngine;

public class PatrolState : EnemyState
{
    private float timer;
    public float patrolDuration = 3f;
    public float speed = 2f;

    public override void Enter()
    {
        base.Enter();
        timer = 0f;
    }

    public override void Do()
    {
        timer += Time.deltaTime;
        stateMachine.rb.velocity = new Vector2(speed, stateMachine.rb.velocity.y);

        if (timer >= patrolDuration)
        {
            isComplete = true;
        }
    }
}
