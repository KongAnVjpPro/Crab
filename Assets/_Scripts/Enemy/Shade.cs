using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shade : Enemy
{
    [SerializeField] private float chaseDistance = 10f; // default 
    [SerializeField] private float timer;
    [SerializeField] private float stunDuration = 1f;
    private static Shade instance;
    public static Shade Instance => instance;
    public GameObject model;
    protected override void Update()
    {
        base.Update();
        if (!PlayerController.Instance.PState.alive)
        {
            ChangeState(EnemyStates.Shade_Idle);
        }
    }
    protected override void Awake()
    {
        base.Awake();
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            if (instance != this)
            {
                Destroy(gameObject);
            }
        }
        SaveData.Instance.SaveShadeData();
        // DontDestroyOnLoad(gameObject);
        ChangeState(EnemyStates.Shade_Idle);
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
            case EnemyStates.Shade_Idle:
                rb.velocity = new Vector2(0, 0);
                if (_dist < chaseDistance)
                {
                    ChangeState(EnemyStates.Shade_Chase);
                }
                break;
            case EnemyStates.Shade_Chase:
                MoveToPlayer();
                // Debug.Log("Chase");
                FlipShade();
                if (_dist > chaseDistance)
                {
                    ChangeState(EnemyStates.Shade_Idle);
                }
                break;
            case EnemyStates.Shade_Stunned:
                timer += Time.deltaTime;
                if (timer > stunDuration)
                {
                    ChangeState(EnemyStates.Shade_Idle);
                    timer = 0;
                }
                break;
            case EnemyStates.Shade_Death:
                // Destroy(gameObject);
                gameObject.layer = deathLayer;
                Death(Random.Range(5f, 10f));
                break;
        }
    }
    void FlipShade()
    {
        // sr.flipX = 
        if (PlayerController.Instance.transform.position.x < transform.position.x)
        {
            // transform.localScale = new Vector2(-1, transform.localScale.y);
            model.transform.localScale = new Vector3(-1, 1, 1);
        }
        else
        {
            // transform.localScale = new Vector2(1, transform.localScale.y);
            model.transform.localScale = new Vector3(1, 1, 1);
        }


    }
    protected override void ChangeCurrentAnimation()
    {
        if (GetCurrentEnemyState == EnemyStates.Shade_Idle)
        {
            // anim.Play(""); // chay idle
        }
        // base.ChangeCurrentAnimation();
        // anim.SetBool("Idle", GetCurrentEnemyState == EnemyStates.Shade_Idle);
        // anim.SetBool("Chase", GetCurrentEnemyState == EnemyStates.Shade_Chase);
        // anim.SetBool("Stunned", GetCurrentEnemyState == EnemyStates.Shade_Stunned);
        if (GetCurrentEnemyState == EnemyStates.Shade_Death)
        {
            PlayerController.Instance.RestoreMana();
            SaveData.Instance.SavePlayerData();
            // anim.SetTrigger(""); //chay Death
            Destroy(gameObject, 0.5f);
        }
    }
    protected override void Attack()
    {
        // anim.SetTrigger("");//chay atk anim
        PlayerController.Instance.TakeDamage(damage);
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
            ChangeState(EnemyStates.Shade_Stunned);
        }
        else
        {
            ChangeState(EnemyStates.Shade_Death);
        }
    }
}
