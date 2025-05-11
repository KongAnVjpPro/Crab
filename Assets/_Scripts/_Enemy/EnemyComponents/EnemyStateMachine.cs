using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyStateMachine : EnemyComponent
{
    public Transform player;
    public LayerMask playerLayer;
    public Rigidbody2D rb;
    public EnemyEntity enemyEntity;

    public Dictionary<EnemyStateID, EnemyState> stateList = new Dictionary<EnemyStateID, EnemyState>();


    public bool hitByAttack = false;

    [Header("Action: ")]
    public Action<EnemyStateID> OnStateChanged;
    [SerializeField] private EnemyState currentState;



    protected override void Awake()
    {
        LoadComponents();

        if (player == null) player = FindAnyObjectByType<PlayerEntity>().transform;
        enemyEntity = enemyController;
    }
    protected override void LoadComponents()
    {
        base.LoadComponents();
        this.LoadAllState();
    }
    protected virtual void LoadAllState()
    {
        foreach (EnemyState state in transform.GetComponentsInChildren<EnemyState>())
        {
            stateList.Add(state.StateID, state);
            state.Init(this);
        }
    }

    private void Start()
    {
        if (stateList[EnemyStateID.Patrolling] != null)
            ChangeState(stateList[EnemyStateID.Patrolling]);
        player = PlayerEntity.Instance.transform;
    }

    private void Update()
    {
        currentState?.Do();

        if (hitByAttack)
        {
            ChangeState(stateList[EnemyStateID.Stunning]);
            return;
        }

        if (currentState != null && currentState.isComplete)
        {
            var next = currentState.CheckNextState();
            if (next.HasValue && stateList.ContainsKey(next.Value))
            {
                ChangeState(stateList[next.Value]);
            }

            // DecideNextState();
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



    #region Player Information
    public Vector2 DirecionToPlayer()
    {
        return new Vector2(player.transform.position.x - transform.position.x, player.transform.position.y - transform.position.y);
    }


    #endregion
    #region Link to Entity
    public void Flip(EnemyRotator.FlipDirection dir)
    {
        enemyController.enemyRotator.Flip(dir);
    }
    public void RotateZ(Vector2 vectorDir)
    {
        enemyController.enemyRotator.RotateZ(vectorDir);
    }

    public void MoveHorizontal(Vector2 direction, float xAxis, float moveSpeed)
    {
        enemyController.enemyMove.MoveHorizontal(direction, xAxis, moveSpeed);
    }
    public void MoveVertical(Vector2 direcion, float yAxis, float moveSpeed)
    {
        enemyController.enemyMove.MoveVertical(direcion, yAxis, moveSpeed);
    }
    #endregion
}
