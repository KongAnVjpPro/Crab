using System;
using UnityEngine;

public abstract class EnemyState : MonoBehaviour
{
    [SerializeField] protected EnemyStateID stateID;
    public EnemyStateID StateID => stateID;
    [SerializeField] protected EnemyStateMachine stateMachine;
    public bool isComplete { get; protected set; }

    public virtual void Init(EnemyStateMachine machine)
    {
        stateMachine = machine;
    }

    public virtual void Enter() => isComplete = false;
    public virtual void Exit() { }
    public virtual void Do() { }
    public virtual void FixedDo() { }

    public virtual EnemyStateID? CheckNextState() => null;

    #region  Boss

    public void SetTalkLayer()
    {
        stateMachine.gameObject.layer = LayerMask.NameToLayer("NPC");
        // stateMachine.collie.enabled = false;
        stateMachine.shield.SetActive(true);
        // Debug.Log("kaye");
    }
    public void SetAttackLayer()
    {
        stateMachine.gameObject.layer = LayerMask.NameToLayer("Enemy");
        // stateMachine.collie.enabled = true;
        stateMachine.shield.SetActive(false);
    }
    #endregion
}


[Serializable]
public enum EnemyStateID
{
    //yeu to chu quan
    None = 0,
    Patrolling = 1,
    Chasing = 2,
    Attacking = 3,
    RangeAttack = 7,

    //khach quan
    Stunned = 4,
    KnockBacked = 5,
    Dead = 6,
    //Boss SW
    SeaWeedTalk = 8,
    SeaWeedStab = 9, //triệu hồi gai
    SeaWeedPrepareWhip = 10, // chuẩn bị đánh thướng
    SeaWeedWhip = 11, //đánh thg
    SeaWeedPhaseTwo = 12,
    SeaWeedSummon = 13, // tạo khi ở phase 2, tảo bắn
    SeaWeedTieUp = 14,// trói khi ở phase2
    SeaWeedPrepareDeath = 15
}
