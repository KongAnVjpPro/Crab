using System.Collections;
using System.Linq;
using UnityEngine;
[RequireComponent(typeof(RangeAttackSpawner))]
public class RangeAttackState : EnemyState
{
    [SerializeField] RangeAttackSpawner bulletSpawner;
    [SerializeField] float shootCoolDown = 3f;
    [SerializeField] float shootTimer = 0;
    [SerializeField] bool hasAttacked = false;
    // [SerializeField] float animLenght = 0.25f;
    // [SerializeField] Animator enemyAni
    void Awake()
    {
        LoadBulletSP();
    }
    protected virtual void LoadBulletSP()
    {
        if (this.bulletSpawner != null) return;
        this.bulletSpawner = GetComponent<RangeAttackSpawner>();
    }
    public override void Enter()
    {
        base.Enter();
        shootTimer = shootCoolDown * 0.8f;
        hasAttacked = false;
        // stateMachine.enemyEntity.enemyAnimator.PrepareRangeAttack();

        Vector2 dirToTarget = stateMachine.DirecionToPlayer().normalized;
        stateMachine.Flip(dirToTarget.x >= 0 ? EnemyRotator.FlipDirection.Right : EnemyRotator.FlipDirection.Left);
    }
    public override void Do()
    {
        // base.Do();
        shootTimer += Time.deltaTime;

        if (shootTimer < shootCoolDown)
            return;
        shootTimer = 0;
        Vector2 dirToTarget = stateMachine.DirecionToPlayer().normalized;
        stateMachine.Flip(dirToTarget.x >= 0 ? EnemyRotator.FlipDirection.Right : EnemyRotator.FlipDirection.Left);
        hasAttacked = true;

        // bulletSpawner.Spawn(stateMachine.transform.position, new Vector2(dirToTarget.x, dirToTarget.y));

        // if (hasAttacked)
        // {
        //     isComplete = true;
        // }
        StartCoroutine(WaitForAttackAnimation(dirToTarget));
    }
    IEnumerator WaitForAttackAnimation(Vector2 dirToTarget)
    {
        stateMachine.enemyEntity.enemyAnimator.PrepareRangeAttack();

        while (!stateMachine.enemyAnim.GetCurrentAnimatorStateInfo(0).IsTag("Attack"))
        {
            yield return null;
        }

        //     AnimationClip clip = stateMachine.enemyAnim.runtimeAnimatorController.animationClips
        // .FirstOrDefault(c => c.name == "SW_Thrower_RangeAtk");
        AnimatorStateInfo aif = stateMachine.enemyAnim.GetCurrentAnimatorStateInfo(0);
        // if (clip != null)
        // {
        //     float animLength = .length * (1 - clip.);

        //     yield return new WaitForSeconds(animLength * 0.9f);
        // }
        float animLenght = aif.length * Mathf.Clamp01(1 - aif.normalizedTime % 1);
        yield return new WaitForSeconds(animLenght);
        bulletSpawner.Spawn(stateMachine.transform.position, new Vector2(dirToTarget.x, dirToTarget.y));

        if (hasAttacked)
        {
            isComplete = true;
        }
    }
    // public void 
    public override EnemyStateID? CheckNextState()
    {
        if (!PlayerEntity.Instance.pState.alive)
        {
            return EnemyStateID.Patrolling;
        }
        float dist = Vector2.Distance(transform.position, stateMachine.player.position);
        if (dist <= stateMachine.rangeAttackDistanceCheck) return EnemyStateID.RangeAttack;
        return EnemyStateID.Patrolling;

    }
    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(stateMachine.transform.position, stateMachine.rangeAttackDistanceCheck);
    }
}