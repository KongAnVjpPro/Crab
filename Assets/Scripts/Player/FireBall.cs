using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireBall : MyMonobehaviour
{
    [SerializeField] float damage;
    [SerializeField] float hitForce;
    [SerializeField] int speed;
    [SerializeField] float lifeTime = 1;
    // Start is called before the first frame update
    void Start()
    {
        Destroy(gameObject, lifeTime);
    }
    void FixedUpdate()
    {
        transform.position += speed * transform.right;
    }
    // Update is called once per frame
    void Update()
    {

    }
    void OnTriggerEnter2D(Collider2D _other)
    {
        if (_other.tag == "enemy")
        {
            _other.GetComponent<Enemy>().EnemyHit(damage, (_other.transform.position - transform.position).normalized, -hitForce);
        }
    }
}
