using System;
using UnityEngine;

public class EnemyStateMachine : EnemyComponent
{
    public Transform player;
    public Rigidbody2D rb;

    [Header("State: ")]
    public PatrolState patrolState;
    public ChaseState chaseState;
    public AttackState attackState;
    [Header("Action: ")]
    public Action<EnemyStateID> OnStateChanged;
    [SerializeField] private EnemyState currentState;

    protected override void Awake()
    {

        if (patrolState != null) patrolState.Init(this);
        if (chaseState != null) chaseState.Init(this);
        if (attackState != null) attackState.Init(this);
        if (player == null) player = FindAnyObjectByType<PlayerEntity>().transform;
    }

    private void Start()
    {
        if (patrolState != null)
            ChangeState(patrolState);
    }

    private void Update()
    {
        currentState?.Do();

        if (currentState != null && currentState.isComplete)
        {
            DecideNextState();
        }
    }

    private void FixedUpdate()
    {
        currentState?.FixedDo();
    }

    void ChangeState(EnemyState newState)
    {
        if (currentState == newState || newState == null) return;

        currentState?.Exit();
        currentState = newState;
        currentState.Enter();

        OnStateChanged?.Invoke(currentState.StateID);
    }

    void DecideNextState()
    {
        float dist = Vector2.Distance(transform.position, player.position);
        if (dist <= 1.5f)
        {
            ChangeState(attackState);
        }
        else if (dist <= 5f)
        {
            ChangeState(chaseState);
        }
        else
        {
            ChangeState(patrolState);
        }
    }



}
