
using System.Collections.Generic;
using UnityEngine;
public class BossPatrol : EnemyState
{
    [SerializeField] Transform pivotMoveRange;

    [SerializeField] float radiusMove = 5f;

    // [SerializeField] float moveSpeed = 5f;
    [SerializeField] float moveTimeMax = 3f;
    [SerializeField] float moveTimeMin = 2f;
    [SerializeField] float currentMoveTime;
    [SerializeField] float moveTimer = 0;

    [SerializeField] Vector2 newPos;
    [SerializeField] Vector2 currentPos;
    // [SerializationVersion]
    BossAnimator anim;
    void Awake()
    {
        anim = transform.parent.GetComponentInParent<BossAnimator>();
    }
    void OnDrawGizmos()
    {
        Gizmos.color = Color.black;
        Gizmos.DrawWireSphere(stateMachine.transform.position, stateMachine.rangeAttackDistanceCheck);
    }
    public override void Init(EnemyStateMachine machine)
    {
        base.Init(machine);
        stateID = EnemyStateID.Patrolling;
    }
    public override void Enter()
    {
        base.Enter();
        stateMachine.collie.enabled = true;
        currentPos = stateMachine.transform.position;
        newPos = Random.onUnitSphere * radiusMove + pivotMoveRange.position;
        if (newPos.x > stateMachine.transform.position.x)
        {
            stateMachine.Flip(EnemyRotator.FlipDirection.Right);
        }
        else
        {
            stateMachine.Flip(EnemyRotator.FlipDirection.Left);
        }
        currentMoveTime = Random.Range(moveTimeMin, moveTimeMax) / stateMachine.bossBoost;
        moveTimer = 0;
    }
    public override void Do()
    {
        base.Do();
        moveTimer += Time.deltaTime;
        if (moveTimer < currentMoveTime)
        {
            anim.Move(true);
            stateMachine.transform.position = Vector2.Lerp(currentPos, newPos, moveTimer / currentMoveTime);


            return;
        }
        isComplete = true;
        currentPos = newPos;
        newPos = Random.onUnitSphere * radiusMove + pivotMoveRange.position;
        if (newPos.x > stateMachine.transform.position.x)
        {
            stateMachine.Flip(EnemyRotator.FlipDirection.Right);
        }
        else
        {
            stateMachine.Flip(EnemyRotator.FlipDirection.Left);
        }
        moveTimer = 0;
        anim.Move(false);
    }
    public override void Exit()
    {
        base.Exit();
        stateMachine.Flip(PlayerEntity.Instance.transform.position.x > stateMachine.transform.position.x ? EnemyRotator.FlipDirection.Right : EnemyRotator.FlipDirection.Left);
    }
    public override EnemyStateID? CheckNextState()
    {
        if (!PlayerEntity.Instance.pState.alive) return EnemyStateID.Patrolling;
        if (Vector2.Distance(stateMachine.transform.position, PlayerEntity.Instance.transform.position) > stateMachine.rangeAttackDistanceCheck) return EnemyStateID.Patrolling;
        if (stateMachine.IsDead()) return EnemyStateID.SeaWeedPrepareDeath;
        float rdRate = Random.Range(0f, 1f);
        if (stateMachine.isOnPhase2 && stateMachine.isMinionAlive)
        {

            if (rdRate >= 0.5f)
            {
                return EnemyStateID.SeaWeedTieUp;
            }
            else if (rdRate >= 0.25f)
            {
                return EnemyStateID.SeaWeedPrepareWhip;
            }
            else
            {
                return EnemyStateID.SeaWeedStab;
            }
        }
        else
        {
            // Debug.Log(rdRate);
            if (rdRate >= 0.5f)
            {
                return EnemyStateID.SeaWeedPrepareWhip;
            }
            else
            {
                return EnemyStateID.SeaWeedStab;
            }
        }

    }
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.black;
        Gizmos.DrawWireSphere(pivotMoveRange.position, radiusMove);
    }
}