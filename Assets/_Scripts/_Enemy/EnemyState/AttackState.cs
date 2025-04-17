using UnityEngine;

public class AttackState : EnemyState
{
    public float attackCooldown = 1.0f;
    private bool hasAttacked;
    private float timer;

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

        if (!hasAttacked && timer >= attackCooldown)
        {
            Debug.Log("Enemy attacks!");
            hasAttacked = true;
            isComplete = true;
        }
    }
}
