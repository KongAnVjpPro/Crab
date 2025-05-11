using UnityEngine;
public class PlayerAttack : PlayerComponent
{
    [SerializeField] float damage = 1f;
    [SerializeField] float attackCD = 1f;
    [SerializeField] float currentCD = 0;

    [SerializeField] bool canAttack = false;
    private bool attackBuffered = false;
    private float bufferDuration = 0.2f;
    private float bufferTimer = 0f;

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
            playerController.pState.attacking = true;
            AttackAnimation();
            FindAndAttack();
            currentCD = 0;
            attackBuffered = false;
        }

    }
    void Update()
    {
        currentCD += Time.deltaTime;
        // if (playerController.playerInput.attack)
        // {
        //     if (playerController.pState.dashing) return;
        //     UpdateAttackVariable();
        //     Attack();

        // }

        CheckAttackBuffer();

        if (playerController.playerInput.attack)
        {
            if (playerController.pState.dashing) return;

            UpdateAttackVariable();

            if (currentCD >= attackCD)
            {
                Attack();
            }
            else
            {
                attackBuffered = true;
                bufferTimer = bufferDuration;
            }
        }

    }
    private void CheckAttackBuffer()
    {
        if (!attackBuffered) return;

        bufferTimer -= Time.deltaTime;
        if (bufferTimer <= 0f)
        {
            attackBuffered = false;
            return;
        }

        if (currentCD >= attackCD)
        {
            Attack();
            attackBuffered = false;
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
            enemyCtrl.state.hitByAttack = true;
            // enemyCtrl.enemyRecoil.RecoilHorizontal(playerController.pState.lookingRight ? 1 : -1);
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
                SetAttackPosition(forwardAttackPos);
                break;
            case AttackState.upAttack:
                SetAttackPosition(upAttackPos);
                break;
            case AttackState.downAttack:
                SetAttackPosition(downAttackPos);
                break;
        }
    }
    private void SetAttackPosition(Transform pos)
    {
        if (currentAttackPos != null)
            currentAttackPos.position = pos.position;
    }
    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(currentAttackPos.position, attackRange);
    }
    public void EndAttack()
    {
        playerController.pState.attacking = false;
        Debug.Log("End");
    }
}