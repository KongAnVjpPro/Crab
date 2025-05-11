using System;
using UnityEngine;

public abstract class EnemyState : MonoBehaviour
{
    [SerializeField] protected EnemyStateID stateID;
    public EnemyStateID StateID => stateID;
    protected EnemyStateMachine stateMachine;
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

}



[Serializable]
public enum EnemyStateID
{
    None = 0,
    Patrolling = 1,
    Chasing = 2,
    Attacking = 3,
    Stunning = 4
}
