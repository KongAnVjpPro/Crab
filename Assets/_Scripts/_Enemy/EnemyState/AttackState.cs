using System.Collections;
using UnityEngine;

public class AttackState : EnemyState
{
    public float attackCooldown = 1.0f;
    private bool hasAttacked;
    private float timer;
    // public float attackAnimTime = 0.1f;
    [SerializeField] Transform attackPos;
    // [SerializeField] float attackRange = 0.3f;
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
        stateMachine.enemyEntity.enemyStat.damageReduceRate = 0;
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
        Collider2D player = Physics2D.OverlapBox(attackPos.position, hitBoxSize, Mathf.Atan2(dirToTarget.y, dirToTarget.x) * Mathf.Rad2Deg, stateMachine.playerLayer);
        // }

        // Collider2D target = P
        if (player != null)
        {
            PlayerEntity playerEntity = player.GetComponent<PlayerEntity>();
            if (playerEntity.pState.alive == false) return;
            // playerEntity.playerStat.ChangeCurrentStats(StatComponent.StatType.Health, -damage);
            // playerEntity.playerAnimator.Hurting();
            // playerEntity.playerEffect.KnockedBack(dirToTarget);
            playerEntity.playerStat.ReceiveDamage(dirToTarget, damage);

        }
    }

    IEnumerator AttackProgress(Vector2 dirToTarget)
    {
        if (PlayerEntity.Instance.pState.alive == false)
        {
            // stateMachine.player = null;
            yield break;
        }
        if (!stateMachine.isSwimming)
        {
            dirToTarget = new Vector2(dirToTarget.x, 0);
        }
        stateMachine.rb.velocity = -dirToTarget * velocityMoveScale;
        stateMachine.OnStateChanged.Invoke(this.stateID);
        // yield return new WaitForSeconds(attackAnimTime);
        yield return WaitForAttackAnimation();
        stateMachine.rb.velocity = dirToTarget * velocityMoveScale;
        Attack(-1 * dirToTarget);
    }
    IEnumerator WaitForAttackAnimation()
    {
        // stateMachine.enemyEntity.enemyAnimator.PrepareRangeAttack();
        // stateMachine.enemyEntity.enemyAnimator.Attacking();

        while (!stateMachine.enemyAnim.GetCurrentAnimatorStateInfo(0).IsTag("Attack"))
        {
            yield return null;
        }


        AnimatorStateInfo aif = stateMachine.enemyAnim.GetCurrentAnimatorStateInfo(0);

        float animLenght = aif.length * Mathf.Clamp01(1 - aif.normalizedTime % 1);
        yield return new WaitForSeconds(animLenght);

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
