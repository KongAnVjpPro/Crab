using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crawler : Enemy
{
    float timer;
    [SerializeField] private float flipWaitTime;
    [SerializeField] private float ledgeCheckX;
    [SerializeField] private float ledgeCheckY;
    [SerializeField] private LayerMask whatIsGround;
    protected override void Update()
    {
        base.Update();
        if (!PlayerController.Instance.PState.alive)
        {
            ChangeState(EnemyStates.Crawler_Idle);
        }
    }
    protected override void Awake()
    {
        base.Awake();
        ChangeState(EnemyStates.Crawler_Idle);
    }
    protected virtual void Start()
    {
        rb.gravityScale = 12f;
    }
    // protected override void Update()
    // {
    //     base.Update();
    //     if (!isRecoiling)
    //     {
    //         transform.position = Vector2.MoveTowards(transform.position, PlayerController.Instance.transform.position, speed * Time.deltaTime);
    //     }
    // }
    void Flip()
    {


    }
    void OnTriggerEnter2D(Collider2D _collision)
    {

        if (_collision.gameObject.CompareTag("Enemy") && _collision.gameObject != gameObject)
        {

            ChangeState(EnemyStates.Crawler_Flip);
        }
    }
    protected override void UpdateEnemyStates()
    {

        base.UpdateEnemyStates();
        if (health <= 0)
        {
            Death(0.05f);
        }

        switch (GetCurrentEnemyState)
        {
            case EnemyStates.Crawler_Idle:

                Vector3 _ledgeCheckStart = transform.localScale.x > 0 ? new Vector3(ledgeCheckX, 0) : new Vector3(-ledgeCheckX, 0);//offet check ria vuc
                Vector2 _wallCheckDir = transform.localScale.x > 0 ? transform.right : -transform.right;//offset check tuong

                if (!Physics2D.Raycast(transform.position + _ledgeCheckStart, Vector2.down, ledgeCheckY, whatIsGround) || Physics2D.Raycast(transform.position, _wallCheckDir, ledgeCheckX, whatIsGround))
                {
                    ChangeState(EnemyStates.Crawler_Flip);
                }

                if (transform.localScale.x > 0)
                {
                    rb.velocity = new Vector2(speed, rb.velocity.y);
                }
                else
                {
                    rb.velocity = new Vector2(-speed, rb.velocity.y);
                }
                break;
            case EnemyStates.Crawler_Flip:
                timer += Time.deltaTime;
                if (timer > flipWaitTime)
                {
                    timer = 0;
                    transform.localScale = new Vector2(transform.localScale.x * -1, transform.localScale.y);
                    ChangeState(EnemyStates.Crawler_Idle);
                }
                break;

        }
    }

}
