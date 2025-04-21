using UnityEngine;
public class PlayerAttack : PlayerComponent
{
    [SerializeField] float damage = 1f;
    [SerializeField] float attackCD = 1f;
    [SerializeField] float currentCD = 0;

    [Header("Attack Config: ")]
    [SerializeField] Transform forwardAttackPos;
    [SerializeField] Transform downAttackPos;
    [SerializeField] Transform upAttackPos;
    [SerializeField] Transform currentAttackPos;
    [SerializeField] LayerMask enemyLayer;
    [SerializeField] float attackRange;
    [Header("Attack State: ")]
    [SerializeField] private AttackState currentState = AttackState.forwardAttack;
    public AttackState ChangeCurrentState
    {
        get
        {
            return currentState;
        }
        set
        {
            if (currentState != value)
            {
                currentState = value;
                DoInChangeState();
            }
        }
    }
    public enum AttackState
    {
        none,
        forwardAttack,
        upAttack,
        downAttack
    }

    public void Attack()
    {

        if (currentCD >= attackCD)
        {
            AttackAnimation();
            FindAndAttack();
            currentCD = 0;
        }

    }
    void Update()
    {
        currentCD += Time.deltaTime;
        if (playerController.playerInput.attack)
        {
            if (playerController.pState.dashing) return;
            UpdateAttackVariable();
            Attack();

        }
    }
    void UpdateAttackVariable()
    {
        if (playerController.pState.jumping && playerController.pState.lookingUp)
        {
            ChangeCurrentState = AttackState.upAttack;
        }
        else if (playerController.pState.jumping && playerController.pState.lookingDown)
        {
            ChangeCurrentState = AttackState.downAttack;
        }
        else
        {
            ChangeCurrentState = AttackState.forwardAttack;
        }
    }
    void FindAndAttack()
    {
        Collider2D[] enemiesToDamage = Physics2D.OverlapCircleAll(currentAttackPos.position, attackRange, enemyLayer);
        foreach (Collider2D enemy in enemiesToDamage)
        {
            EnemyEntity enemyCtrl = enemy.GetComponent<EnemyEntity>();
            if (enemyCtrl == null) continue;
            enemyCtrl.enemyStat.ChangeCurrentStats(StatComponent.StatType.Health, -damage);
            Debug.Log("Attack");
        }
    }
    void AttackAnimation()
    {
        if (currentState == AttackState.upAttack)
        {
            playerController.playerAnimator.AirAttacking();
        }
        else if (currentState == AttackState.downAttack)
        {
            playerController.playerAnimator.DownAttacking();
        }
        else
        {
            playerController.playerAnimator.Attacking();
        }

    }
    private void DoInChangeState()
    {
        switch (currentState)
        {
            case AttackState.forwardAttack:
                currentAttackPos.position = forwardAttackPos.position;
                break;
            case AttackState.upAttack:
                currentAttackPos.position = upAttackPos.position;
                break;
            case AttackState.downAttack:
                currentAttackPos.position = downAttackPos.position;
                break;
                //   currentAttackPos.position = forwardAttackPos.position;
        }
    }
    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(currentAttackPos.position, attackRange);
    }
}