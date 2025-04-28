using System.Collections;
using UnityEngine;

public class AttackState : EnemyState
{
    public float attackCooldown = 1.0f;
    private bool hasAttacked;
    private float timer;
    public float attackAnimTime = 0.1f;
    [SerializeField] Transform attackPos;
    [SerializeField] float attackRange = 0.3f;
    [SerializeField] float damage = 1f;
    [SerializeField] float velocityMoveScale = 5f;
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
        // stateMachine.rb.velocity = Vector2.zero;
        Vector2 dirToTarget = stateMachine.DirecionToPlayer().normalized;

        stateMachine.Flip((dirToTarget.x >= 0) ? EnemyRotator.FlipDirection.Up : EnemyRotator.FlipDirection.Down);
        stateMachine.RotateZ(dirToTarget);



        timer += Time.deltaTime;
        hasAttacked = false;
        if (!hasAttacked && timer >= attackCooldown)
        {
            Debug.Log("Enemy attacks!");
            hasAttacked = true;
            isComplete = true;


            StartCoroutine(AttackProgress(dirToTarget));
            // Attack();
            timer = 0;
        }
    }
    void Attack()
    {
        Collider2D player = Physics2D.OverlapCircle(attackPos.position, attackRange, stateMachine.playerLayer);
        if (player != null)
        {
            PlayerEntity playerEntity = player.GetComponent<PlayerEntity>();
            playerEntity.playerStat.ChangeCurrentStats(StatComponent.StatType.Health, -damage);
        }
    }

    IEnumerator AttackProgress(Vector2 dirToTarget)
    {
        stateMachine.rb.velocity = -dirToTarget * velocityMoveScale;
        stateMachine.OnStateChanged.Invoke(this.stateID);
        yield return new WaitForSeconds(attackAnimTime);
        stateMachine.rb.velocity = dirToTarget * velocityMoveScale;
        Attack();
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(attackPos.position, attackRange);
    }
}
