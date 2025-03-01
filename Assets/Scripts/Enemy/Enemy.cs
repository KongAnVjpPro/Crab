using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : AnMonobehaviour
{
    [SerializeField] protected float health = 10f;
    [SerializeField] protected float recoilLength = 0.5f;
    [SerializeField] protected float recoilFactor = 5;
    [SerializeField] protected bool isRecoiling = false;
    [SerializeField] protected PlayerMovement playerMovement;
    [SerializeField] protected float speed = 5f;
    [SerializeField] protected float damage = 1f;

    protected float recoilTimer;
    protected Rigidbody2D rb;
    // Start is called before the first frame update
    void Start()
    {

    }
    protected override void LoadComponents()
    {
        base.LoadComponents();
        rb = GetComponent<Rigidbody2D>();
        // playerMovement = PlayerController.Instance.PlayerMovement;
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        if (health <= 0)
        {
            Destroy(gameObject);
        }
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
    }
    public virtual void EnemyHit(float _damageDone, Vector2 _hitDirection, float _hitForce)
    {
        health -= _damageDone;
        if (!isRecoiling)
        {
            rb.AddForce(-_hitForce * recoilFactor * _hitDirection);
        }
    }
    protected virtual void Attack()
    {
        PlayerController.Instance.PlayerMovement.TakeDamage(damage);
    }

    protected virtual void OnTriggerStay2D(Collider2D _other)
    {
        if (_other.CompareTag("Player") && !PlayerController.Instance.PlayerMovement.PState.invicible)
        {
            Attack();

        }
    }
}
