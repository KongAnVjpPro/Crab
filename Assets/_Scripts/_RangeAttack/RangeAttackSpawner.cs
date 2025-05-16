using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
public class RangeAttackSpawner : EntityComponent
{
    [SerializeField] protected List<RangeAttack> pool;
    [SerializeField] protected RangeAttack prefab;
    [SerializeField] protected Transform bulletHolder;

    public void Spawn(Vector3 spawnPos, Vector2 bulletDir)
    {
        RangeAttack ra = TakeFromPool();
        if (ra == null)
        {
            ra = Instantiate(prefab, spawnPos, Quaternion.identity);
            ra.transform.SetParent(bulletHolder); ;
            ra.SetSpawner(this);
        }

        ra.transform.position = spawnPos;
        ra.gameObject.SetActive(true);
        ra.Fire(spawnPos, bulletDir);
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