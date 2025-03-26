using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Swimmer : Enemy
{
    [SerializeField] private float chaseDistance = 5f; // default 
    [SerializeField] private float timer;
    [SerializeField] private float stunDuration = 1f;
    protected override void Update()
    {
        base.Update();
        if (!PlayerController.Instance.PState.alive)
        {
            ChangeState(EnemyStates.Swimmer_Idle);
        }
    }
    protected override void Awake()
    {
        base.Awake();
        ChangeState(EnemyStates.Swimmer_Idle);
    }
    protected override void UpdateEnemyStates()
    {
        // if (health <= 0)
        // {
        //     Death(0.05f);
        // }

        float _dist = Vector2.Distance(transform.position, PlayerController.Instance.transform.position);
        base.UpdateEnemyStates();
        switch (GetCurrentEnemyState)
        {
            case EnemyStates.Swimmer_Idle:
                rb.velocity = new Vector2(0, 0);
                if (_dist < chaseDistance)
                {
                    ChangeState(EnemyStates.Swimmer_Chase);
                }
                break;
            case EnemyStates.Swimmer_Chase:
                MoveToPlayer();
                // Debug.Log("Chase");
                FlipSwimmer();
                if (_dist > chaseDistance)
                {
                    ChangeState(EnemyStates.Swimmer_Idle);
                }
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
                // Destroy(gameObject);
                gameObject.layer = deathLayer;
                Death(Random.Range(5f, 10f));
                break;
        }
    }
    void FlipSwimmer()
    {
        sr.flipX = PlayerController.Instance.transform.position.x < transform.position.x;

    }
    protected override void ChangeCurrentAnimation()
    {
        base.ChangeCurrentAnimation();
        anim.SetBool("Idle", GetCurrentEnemyState == EnemyStates.Swimmer_Idle);
        anim.SetBool("Chase", GetCurrentEnemyState == EnemyStates.Swimmer_Chase);
        anim.SetBool("Stunned", GetCurrentEnemyState == EnemyStates.Swimmer_Stunned);
        if (GetCurrentEnemyState == EnemyStates.Swimmer_Death)
        {
            anim.SetTrigger("Death");
        }
    }
    protected override void Death(float _destroyTime)
    {
        base.Death(_destroyTime);
        rb.gravityScale = 12f;
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
