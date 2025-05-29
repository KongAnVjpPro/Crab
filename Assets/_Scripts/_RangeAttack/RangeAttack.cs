using System.Collections;
using Cinemachine;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Events;
public class RangeAttack : MyMonobehaviour
{
    [Header("Layer Target: ")]
    [SerializeField] LayerMask targetLayer;//playerlayer
    [SerializeField] LayerMask groundLayer;

    [Header("Bullet Config: ")]
    [SerializeField] float bulletDamage = 1f;
    [SerializeField] RangeAttackSpawner spawner;
    [SerializeField] float bulletSpeed = 10f;
    [SerializeField] Animator bulletAnim;
    [SerializeField] Rigidbody2D rgBullet;
    [SerializeField] Collider2D bulletCollide;
    [SerializeField] float despawnTime = 1f;
    [SerializeField] Vector2 currentDir;
    [SerializeField] float currentTimeExist = 0;
    [SerializeField] float maxTimeExist = 10f;
    [SerializeField] CinemachineImpulseSource impulseSource;
    public void SetSpawner(RangeAttackSpawner spawner)
    {
        this.spawner = spawner;
    }

    public void Fire(Vector3 startPos, Vector2 dir)
    {
        currentDir = dir;

        transform.position = startPos;
        bulletAnim.SetTrigger("PreFire");
        rgBullet.velocity = dir.normalized * bulletSpeed;
        // transform.rotation = dir.
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        Quaternion rot = Quaternion.Euler(0, 0, angle);
        transform.rotation = rot;
    }
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (((1 << collision.gameObject.layer) & groundLayer.value) != 0)
        {
            if (bounceCount < bounceLimit)
            {
                bounceCount++;
                //logic dan nay
                RaycastHit2D hit = Physics2D.Raycast(transform.position, currentDir, 0.5f, groundLayer);
                if (hit.collider != null)
                {
                    currentDir = Vector2.Reflect(currentDir, hit.normal);
                    Debug.Log(currentDir);
                    rgBullet.velocity = currentDir.normalized * bulletSpeed;
                    float angle = Mathf.Atan2(currentDir.y, currentDir.x) * Mathf.Rad2Deg;
                    Quaternion rot = Quaternion.Euler(0, 0, angle);
                }

                return;
            }
            DespawnBullet();
            return;
        }
        if (((1 << collision.gameObject.layer) & targetLayer.value) != 0)
        {
            DespawnBullet();
            TargetCallBack?.Invoke();
        }
    }
    [SerializeField] int bounceCount = 0;
    [SerializeField] int bounceLimit = 0;
    public void DespawnBullet()
    {
        // if (bounceCount < bounceLimit)
        // {
        //     bounceCount++;

        //     return;
        // }
        bounceCount = 0;
        rgBullet.velocity = Vector2.zero;
        bulletCollide.enabled = false;

        // gameObject.SetActive(false);
        StartCoroutine(WaitForDespawn());
    }

    IEnumerator WaitForDespawn()
    {
        bulletAnim.SetBool("Despawn", true);
        yield return new WaitForSeconds(despawnTime);
        currentTimeExist = 0;
        gameObject.SetActive(false);
    }
    void ResetTrigger()
    {
        bulletAnim.SetBool("Despawn", false);
        bulletAnim.ResetTrigger("PreFire");
    }
    void OnEnable()
    {
        ResetTrigger();
        rgBullet.velocity = Vector2.zero;
        bulletCollide.enabled = true;
        currentTimeExist = 0;
    }
    void Update()
    {
        currentTimeExist += Time.deltaTime;
        if (currentTimeExist > maxTimeExist)
        {
            DespawnBullet();
            return;
        }
    }
    public UnityEvent TargetCallBack;

    #region  CallBackBind
    public void DealDmg()
    {
        PlayerEntity.Instance.playerStat.ReceiveDamage(new Vector2(currentDir.x * -1, currentDir.y), bulletDamage);
    }
    public void Impact()
    {
        impulseSource?.GenerateImpulse();
    }
    #endregion

}