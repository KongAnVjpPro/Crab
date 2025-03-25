using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;

public class Enemy : MyMonobehaviour
{
    [SerializeField] protected float health = 10f;
    [SerializeField] protected float recoilLength = 0.5f;
    [SerializeField] protected float recoilFactor = 5;
    [SerializeField] protected bool isRecoiling = false;
    [SerializeField] protected float speed = 5f;
    [SerializeField] protected float damage = 1f;
    [SerializeField] protected GameObject orangeBlood;
    [SerializeField] protected LayerMask deathLayer;


    protected float recoilTimer;
    protected Rigidbody2D rb;
    protected SpriteRenderer sr;
    protected Animator anim;
    // Start is called before the first frame update
    void Start()
    {

    }
    protected override void LoadComponents()
    {
        base.LoadComponents();
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        deathLayer = LayerMask.NameToLayer("DeathCreature");
        // playerMovement = PlayerController.Instance.PlayerMovement;
    }
    protected enum EnemyStates
    {
        //Crawler
        Crawler_Idle,
        Crawler_Flip,
        //Swimmer
        Swimmer_Idle,
        Swimmer_Chase,
        Swimmer_Stunned,
        Swimmer_Death,
        //Charger
        Charger_Idle,
        Charger_Suprised,
        Charger_Charge
    }
    protected virtual EnemyStates GetCurrentEnemyState
    {
        get { return currentEnemyState; }
        set
        {
            if (currentEnemyState != value)
            {
                currentEnemyState = value;
                ChangeCurrentAnimation();
            }
        }
    }
    protected EnemyStates currentEnemyState;

    // Update is called once per frame
    protected virtual void Update()
    {

        // if (health <= 0)
        // {
        //     Destroy(gameObject);
        // }
        if (isRecoiling)
        {
            if (recoilTimer < recoilLength)
            {
                recoilTimer += Time.deltaTime;
            }
            else
            {
                isRecoiling = false;
                recoilTimer = 0;
            }
        }
        else
        {
            UpdateEnemyStates();
        }
    }
    public virtual void EnemyGetHit(float _damageDone, Vector2 _hitDirection, float _hitForce)
    {
        health -= _damageDone;
        if (!isRecoiling)
        {
            //blood effect
            GameObject _orangeBlood = Instantiate(orangeBlood, transform.position, quaternion.identity);
            Destroy(_orangeBlood, 5.5f);
            // rb.AddForce(-_hitForce * recoilFactor * _hitDirection);
            rb.velocity = -_hitForce * recoilFactor * _hitDirection;
            isRecoiling = true;
        }
    }
    protected virtual void Death(float _destroyTime)
    {
        Destroy(gameObject, _destroyTime);
    }
    protected virtual void Attack()
    {
        PlayerController.Instance.TakeDamage(damage);
    }

    protected virtual void OnCollisionStay2D(Collision2D _other)
    {
        if (_other.gameObject.CompareTag("Player") && !PlayerController.Instance.PState.invincible && health > 0)
        {
            Attack();
            if (PlayerController.Instance.PState.alive)
            {
                PlayerController.Instance.HitStopTime(0.1f, 5, 0.5f);//avoid time stop
            }

        }
    }
    protected virtual void UpdateEnemyStates()
    {

    }
    protected virtual void ChangeCurrentAnimation()
    {

    }
    protected virtual void ChangeState(EnemyStates _newState)
    {
        GetCurrentEnemyState = _newState;
    }
    protected virtual void MoveToPlayer()
    {
        transform.position = Vector2.MoveTowards(transform.position, PlayerController.Instance.transform.position, Time.deltaTime * speed);
    }
}
