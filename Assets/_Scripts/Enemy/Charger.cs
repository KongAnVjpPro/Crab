using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Charger : Enemy
{
    private float timer;
    [SerializeField] private float ledgeCheckX;
    [SerializeField] private float ledgeCheckY;
    [SerializeField] private LayerMask whatIsGround;
    [SerializeField] private float chargeSpeedMultiplier;
    [SerializeField] private float jumpForce;
    [SerializeField] private float chargeDuration;
    [SerializeField] private float chargeCoolDown = 1f;
    private float currentCoolDown = 0;
    // [SerializeField] private float surprisedTime = 1f;
    protected override void Update()
    {
        base.Update();
        if (!PlayerController.Instance.PState.alive)
        {
            ChangeState(EnemyStates.Charger_Idle);
        }
    }
    protected override void Awake()
    {
        base.Awake();
        ChangeState(EnemyStates.Crawler_Idle);
    }
    protected virtual void Start()
    {
        ChangeState(EnemyStates.Charger_Idle);
        rb.gravityScale = 12f;
    }
    void OnTriggerEnter2D(Collider2D _collision)
    {

        if (_collision.gameObject.CompareTag("Enemy") && _collision.gameObject != gameObject)
        {
            transform.localScale = new Vector2(transform.localScale.x * -1, transform.localScale.y);
        }
    }
    void Flip()
    {


    }
    protected override void UpdateEnemyStates()
    {

        base.UpdateEnemyStates();
        if (health <= 0)
        {
            Death(0.05f);
        }

        Vector3 _ledgeCheckStart = transform.localScale.x > 0 ? new Vector3(ledgeCheckX, 0) : new Vector3(-ledgeCheckX, 0);//offet check ria vuc
        Vector2 _wallCheckDir = transform.localScale.x > 0 ? transform.right : -transform.right;//offset check tuong
        switch (GetCurrentEnemyState)
        {

            case EnemyStates.Charger_Idle:


                if (!Physics2D.Raycast(transform.position + _ledgeCheckStart, Vector2.down, ledgeCheckY, whatIsGround) || Physics2D.Raycast(transform.position, _wallCheckDir, ledgeCheckX, whatIsGround))
                {
                    // ChangeState(EnemyStates.Charger_Idle);
                    transform.localScale = new Vector2(transform.localScale.x * -1, transform.localScale.y);
                }

                if (transform.localScale.x > 0)
                {
                    rb.velocity = new Vector2(speed, rb.velocity.y);
                }
                else
                {
                    rb.velocity = new Vector2(-speed, rb.velocity.y);
                }
                RaycastHit2D _hit = Physics2D.Raycast(transform.position + _ledgeCheckStart, _wallCheckDir, ledgeCheckX * 10);
                if (_hit.collider != null && _hit.collider.gameObject.CompareTag("Player"))
                {
                    ChangeState(EnemyStates.Charger_Suprised);
                }

                break;
            case EnemyStates.Charger_Suprised:
                rb.velocity = new Vector2(0, jumpForce);
                ChangeState(EnemyStates.Charger_Charge);
                break;
            case EnemyStates.Charger_Charge:
                currentCoolDown += Time.deltaTime;
                if (currentCoolDown < chargeCoolDown) return;
                timer += Time.deltaTime;
                if (timer < chargeDuration)
                {
                    if (Physics2D.Raycast(transform.position, Vector2.down, ledgeCheckY, whatIsGround))
                    {
                        if (transform.localScale.x > 0)
                        {
                            rb.velocity = new Vector2(speed * chargeSpeedMultiplier, rb.velocity.y);
                        }
                        else
                        {
                            rb.velocity = new Vector2(-speed * chargeSpeedMultiplier, rb.velocity.y);
                        }
                    }
                    else
                    {
                        rb.velocity = new Vector2(0, rb.velocity.y);
                    }
                }
                else
                {
                    timer = 0;
                    ChangeState(EnemyStates.Charger_Idle);
                    currentCoolDown = 0;
                }


                break;

        }
    }
    protected override void ChangeCurrentAnimation()
    {
        base.ChangeCurrentAnimation();
        if (GetCurrentEnemyState == EnemyStates.Charger_Idle)
        {
            anim.speed = 1;
        }
        if (GetCurrentEnemyState == EnemyStates.Charger_Charge)
        {
            anim.speed = chargeSpeedMultiplier;
        }
    }

}
