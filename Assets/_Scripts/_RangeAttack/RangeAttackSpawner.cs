using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
public class RangeAttackSpawner : EntityComponent
{
    [SerializeField] protected List<RangeAttack> pool;
    [SerializeField] protected RangeAttack prefab;
    [Header("Bullet parent (null able): ")]
    [SerializeField] GameObject holder;

    // [SerializeField] protected Transform bulletHolder;
    void Start()
    {
        // bulletHolder = 
    }
    public void Spawn(Vector3 spawnPos, Vector2 bulletDir)
    {
        RangeAttack ra = TakeFromPool();
        if (ra == null)
        {
            ra = Instantiate(prefab, spawnPos, Quaternion.identity);
            // ra.transform.SetParent(bulletHolder); 

            if (holder != null)
            {
                ra.transform.SetParent(holder.transform);
            }
            ra.SetSpawner(this);
            pool.Add(ra);
        }

        ra.transform.position = spawnPos;
        ra.gameObject.SetActive(true);
        ra.Fire(spawnPos, bulletDir);
    }
    public RangeAttack DelaySpawn(Vector3 spawnPos, Vector2 bulletDir)
    {
        RangeAttack ra = TakeFromPool();
        if (ra == null)
        {
            ra = Instantiate(prefab, spawnPos, Quaternion.identity);
            // ra.transform.SetParent(bulletHolder); 

            if (holder != null)
            {
                ra.transform.SetParent(holder.transform);
            }
            ra.SetSpawner(this);
            pool.Add(ra);
        }

        ra.transform.position = spawnPos;
        // ra.Fire(spawnPos, bulletDir);
        float angle = Mathf.Atan2(bulletDir.y, bulletDir.x) * Mathf.Rad2Deg;
        Quaternion rot = Quaternion.Euler(0, 0, angle);
        ra.transform.rotation = rot;

        ra.gameObject.SetActive(true);
        // StartCoroutine(DelayFireTime(delayTime, ra, spawnPos, bulletDir));
        return ra;
    }
    IEnumerator DelayFireTime(float time, RangeAttack bullet, Vector3 spawnPos, Vector2 bulletDir)
    {
        yield return new WaitForSeconds(time);
        bullet.Fire(spawnPos, bulletDir);
    }
    RangeAttack TakeFromPool()
    {
        foreach (var atk in pool)
        {
            if (atk.gameObject.activeSelf == false)
            {
                return atk;
            }
        }
        return null;
    }
}