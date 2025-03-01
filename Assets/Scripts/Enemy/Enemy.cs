using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : AnMonobehaviour
{
    [SerializeField] float health = 10f;
    [SerializeField] float recoilLength = 0.5f;
    [SerializeField] float recoilFactor = 5;
    [SerializeField] bool isRecoiling = false;

    float recoilTimer;
    Rigidbody2D rb;
    // Start is called before the first frame update
    void Start()
    {

    }
    protected override void LoadComponents()
    {
        base.LoadComponents();
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
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
    public void EnemyHit(float _damageDone, Vector2 _hitDirection, float _hitForce)
    {
        health -= _damageDone;
        if (!isRecoiling)
        {
            rb.AddForce(-_hitForce * recoilFactor * _hitDirection);
        }
    }
}
