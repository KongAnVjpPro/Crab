using System.Collections;
using UnityEngine;
public class PlayerAttack : PlayerComponent
{
    [SerializeField] float damage = 1f;
    public float Damage => damage;
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
    // [SerializeField] float attackRange = 0.52f;
    [Header("Attack Area: ")]
    [SerializeField] Vector2 downArea = new Vector2(2, 1);
    [SerializeField] Vector2 forwardArea = new Vector2(3, 1.5f);
    [SerializeField] Vector2 upwardArea = new Vector2(1.5f, 1.5f);
    [SerializeField] Vector2 currentAtkArea;

    [SerializeField] float attackRecoil = 2f;
    [SerializeField] Vector2 upattackForce = new Vector2(4f, 4f);
    [SerializeField] float appliedForceTime = 0.2f;
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

        if (currentCD >= (attackCD / playerController.speedBoost))
        {
            playerController.pState.attacking = true;
            hitTarget = false;
            AttackAnimation();
            // FindAndAttack();
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
        if (playerController.pState.lookingUp)
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
    bool hitTarget = false;
    void FindAndAttack()
    {
        // if (currentState == AttackState.upAttack)
        // {
        //     StartCoroutine(ApplyUpwardForce());
        // }
        Collider2D[] enemiesToDamage = Physics2D.OverlapBoxAll(currentAttackPos.position, currentAtkArea, enemyLayer);
        foreach (Collider2D enemy in enemiesToDamage)
        {
            EnemyEntity enemyCtrl = enemy.GetComponent<EnemyEntity>();
            if (enemyCtrl == null) continue;
            if (!enemyCtrl.state.IsDead())
            {
                enemyCtrl.enemyStat.ChangeCurrentStats(StatComponent.StatType.Health, -damage);
                enemyCtrl.state.SetInterruptState(EnemyStateID.Stunned);
                playerController.playerEffect.SpawnEffect(enemyCtrl.transform, EffectAnimationID.Slash);
                // enemyCtrl.enemyRecoil.RecoilHorizontal(playerController.pState.lookingRight ? 1 : -1);
                Debug.Log("Attack");
                hitTarget = true;
            }

        }

        Collider2D[] breakableObject = Physics2D.OverlapBoxAll(currentAttackPos.position, currentAtkArea, LayerMask.GetMask("DamageAble"));
        foreach (Collider2D obj in breakableObject)
        {
            DamagedAbleObject damagedAbleobj = obj.GetComponent<DamagedAbleObject>();
            if (damagedAbleobj == null) continue;
            damagedAbleobj.TakeDamage(damage);
            playerController.playerEffect.SpawnEffect(damagedAbleobj.transform, EffectAnimationID.Slash);
            hitTarget = true;
        }

        ApplyRecoil();
    }
    IEnumerator ApplyUpwardForce()
    {
        Vector2 currentV = playerController.rb.velocity;
        // Vector2 force = new Vector2(upattackForce.x * (playerController.pState.lookingRight ? 1 : -1), upattackForce.y);
        playerController.rb.velocity += new Vector2(0, upattackForce.y);
        yield return new WaitForSeconds(appliedForceTime);
        // while (playerController.rb.velocity.y > 0)
        // {
        //     yield return null;
        // }
        // playerController.rb.velocity = ;

    }
    public void ApplyRecoil()
    {
        if (!hitTarget) return;
        if (currentState == AttackState.forwardAttack)
        {
            Vector2 recoilDir = (forwardAttackPos.position - playerController.transform.position).normalized;
            playerController.playerRecoil.RecoilBoth(recoilDir.x * attackRecoil, false);
        }
        else if (currentState == AttackState.downAttack)
        {
            playerController.playerRecoil.RecoilVertical(true, attackRecoil);
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
        StartCoroutine(WaitForEndAnimation());
    }
    IEnumerator WaitForEndAnimation()
    {
        while (!playerController.playerAnimator.IsInAttackAnim())
        {
            yield return null;
        }
        yield return new WaitForSeconds(playerController.playerAnimator.GetCurrentAnimationTime());
        FindAndAttack();
    }
    private void DoInChangeState()
    {
        switch (currentState)
        {
            case AttackState.forwardAttack:
                SetAttackPosition(forwardAttackPos);
                currentAtkArea = forwardArea;
                break;
            case AttackState.upAttack:
                SetAttackPosition(upAttackPos);
                currentAtkArea = upwardArea;
                break;
            case AttackState.downAttack:
                SetAttackPosition(downAttackPos);
                currentAtkArea = downArea;
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
        Gizmos.DrawWireCube(upAttackPos.position, upwardArea);
        Gizmos.DrawWireCube(downAttackPos.position, downArea);
        Gizmos.DrawWireCube(forwardAttackPos.position, forwardArea);
    }
    public void EndAttack()
    {
        playerController.pState.attacking = false;
        Debug.Log("End");
    }
    #region  upgrade
    public void SetDamage(float value)
    {
        damage = value;
    }

    #endregion
}