using UnityEngine;

public abstract class EnemyState : MonoBehaviour
{
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
}
