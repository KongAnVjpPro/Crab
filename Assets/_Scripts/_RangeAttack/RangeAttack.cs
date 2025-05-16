using UnityEngine;
public class RangeAttack : MyMonobehaviour
{

    [SerializeField] RangeAttackSpawner spawner;
    public void SetSpawner(RangeAttackSpawner spawner)
    {
        this.spawner = spawner;
    }

    public void Fire(Vector3 startPos, Vector2 dir)
    {

    }

}