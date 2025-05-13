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
    [Header("Config for not swimming creature: ")]
    [SerializeField] Vector2 hitBoxSize = new Vector2(1, 1);
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
        if (stateMachine.isSwimming)
        {
            stateMachine.Flip((dirToTarget.x >= 0) ? EnemyRotator.FlipDirection.Up : EnemyRotator.FlipDirection.Down);
            stateMachine.RotateZ(dirToTarget);
        }
        else
        {
            stateMachine.Flip(dirToTarget.x >= 0 ? EnemyRotator.FlipDirection.Right : EnemyRotator.FlipDirection.Left);
        }




        timer += Time.deltaTime;
        hasAttacked = false;
        if (!hasAttacked && timer >= attackCooldown)
        {
            // Debug.Log("Enemy attacks!");
            hasAttacked = true;
            isComplete = true;


            StartCoroutine(AttackProgress(dirToTarget));
            // Attack();
            timer = 0;
        }
    }
    void Attack(Vector2 dirToTarget)
    {

        // Collider2D player = Physics2D.OverlapCircle(attackPos.position, attackRange, stateMachine.playerLayer);

        // if (!stateMachine.isSwimming)
        // {
        // Vector2 boxSiz = Vector2.down;
        Collider2D player = Physics2D.OverlapBox(attackPos.position, hitBoxSize, 0, stateMachine.playerLayer);
        // }

        // Collider2D target = P
        if (player != null)
        {
            PlayerEntity playerEntity = player.GetComponent<PlayerEntity>();
            playerEntity.playerStat.ChangeCurrentStats(StatComponent.StatType.Health, -damage);
            playerEntity.playerEffect.KnockedBack(dirToTarget);

        }
    }

    IEnumerator AttackProgress(Vector2 dirToTarget)
    {
        if (!stateMachine.isSwimming)
        {
            dirToTarget = new Vector2(dirToTarget.x, 0);
        }
        stateMachine.rb.velocity = -dirToTarget * velocityMoveScale;
        stateMachine.OnStateChanged.Invoke(this.stateID);
        yield return new WaitForSeconds(attackAnimTime);
        stateMachine.rb.velocity = dirToTarget * velocityMoveScale;
        Attack(-1 * dirToTarget);
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        // if (stateMachine.isSwimming)
        // {
        //     Gizmos.DrawWireSphere(attackPos.position, attackRange);
        // }
        // else
        {
            Gizmos.DrawWireCube(attackPos.position, hitBoxSize);
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
