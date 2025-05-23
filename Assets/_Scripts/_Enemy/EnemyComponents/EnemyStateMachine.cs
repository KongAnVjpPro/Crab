using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

// [Serializable]
public class EnemyStateMachine : EnemyComponent
{
    public Transform player;
    public LayerMask playerLayer;
    public Rigidbody2D rb;
    public EnemyEntity enemyEntity;

    public Dictionary<EnemyStateID, EnemyState> stateList = new Dictionary<EnemyStateID, EnemyState>();

    [SerializeField] private EnemyStateID? interruptState = null;
    public bool isSwimming = false;
    public bool isRangeAttack = false;
    public Animator enemyAnim;
    public void SetInterruptState(EnemyStateID stateID)
    {
        interruptState = stateID;
    }

    [Header("Action: ")]
    public Action<EnemyStateID> OnStateChanged;
    [SerializeField] private EnemyState currentState;
    [Header("For rangeAttack")]
    [SerializeField] public float rangeAttackDistanceCheck = 10f;



    protected override void Awake()
    {
        LoadComponents();

        // if (player == null) player = FindAnyObjectByType<PlayerEntity>().transform;
        StartCoroutine(WaitForPlayer());
        enemyEntity = enemyController;
        enemyAnim = enemyController.enemyAnimator.GetAnimator();
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
        StartCoroutine(WaitForPlayer());
    }
    IEnumerator WaitForPlayer()
    {
        yield return new WaitForSeconds(1f);
        while (PlayerEntity.Instance.transform == null)
        {
            yield return null;
        }
        player = PlayerEntity.Instance.transform;
    }
    private void Update()
    {
        if (interruptState.HasValue)
        {
            ChangeState(stateList[interruptState.Value]);
            interruptState = null;
            return;
        }


        currentState?.Do();

        // if (hitByAttack)
        // {
        //     ChangeState(stateList[EnemyStateID.Stunned]);
        //     return;
        // }

        if (currentState != null && currentState.isComplete)
        {
            var next = currentState.CheckNextState();
            if (next.HasValue && stateList.ContainsKey(next.Value))
            {
                ChangeState(stateList[next.Value]);
            }
            else
            {
                ChangeState(stateList[EnemyStateID.Patrolling]);

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
        if (PlayerEntity.Instance.pState.alive == false)
        {
            currentState = stateList[EnemyStateID.Patrolling];
        }
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
    public void RecoilBoth(Vector2 dir)
    {
        enemyController.enemyRecoil.RecoilBoth(dir.x, dir.y > 0 ? true : false);
    }

    public void MoveHorizontal(Vector2 direction, float xAxis, float moveSpeed)
    {
        enemyController.enemyMove.MoveHorizontal(direction, xAxis, moveSpeed);
    }
    public void MoveVertical(Vector2 direcion, float yAxis, float moveSpeed)
    {
        enemyController.enemyMove.MoveVertical(direcion, yAxis, moveSpeed);
    }
    public bool IsDead()
    {
        return enemyController.enemyStat.IsDead();
    }
    public void Drop()
    {
        enemyController.enemyDrop.Drop();
    }
    public void HPBarFadeIn()
    {
        enemyController.enemyHealthBar.FadeIn();
    }
    public void HPBarFadeOut()
    {
        enemyController.enemyHealthBar.FadeOut();
    }
    #endregion
}
