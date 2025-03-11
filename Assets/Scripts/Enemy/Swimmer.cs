using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Swimmer : Enemy
{
    [SerializeField] private float chaseDistance = 5f; // default 
    [SerializeField] private float timer;
    [SerializeField] private float stunDuration = 1f;

    protected override void Awake()
    {
        base.Awake();
        ChangeState(EnemyStates.Swimmer_Idle);
    }
    protected override void UpdateEnemyStates()
    {
        float _dist = Vector2.Distance(transform.position, PlayerController.Instance.transform.position);
        base.UpdateEnemyStates();
        switch (currentEnemyState)
        {
            case EnemyStates.Swimmer_Idle:
                if (_dist < chaseDistance)
                {
                    ChangeState(EnemyStates.Swimmer_Chase);
                }
                break;
            case EnemyStates.Swimmer_Chase:
                MoveToPlayer();
                // Debug.Log("Chase");
                FlipBat();
                break;
            case EnemyStates.Swimmer_Stunned:
                timer += Time.deltaTime;
                if (timer > stunDuration)
                {
                    ChangeState(EnemyStates.Swimmer_Idle);
                    timer = 0;
                }
                break;
            case EnemyStates.Swimmer_Death:
                Destroy(gameObject);
                break;
        }
    }
    void FlipBat()
    {
        sr.flipX = PlayerController.Instance.transform.position.x < transform.position.x;

    }
    public override void EnemyGetHit(float _damageDone, Vector2 _hitDirection, float _hitForce)
    {
        base.EnemyGetHit(_damageDone, _hitDirection, _hitForce);
        if (health > 0)
        {
            ChangeState(EnemyStates.Swimmer_Stunned);
        }
        else
        {
            ChangeState(EnemyStates.Swimmer_Death);
        }
    }
}
