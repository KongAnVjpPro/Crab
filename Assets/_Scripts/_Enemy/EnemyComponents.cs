using UnityEngine;
public class EnemyComponent : EntityComponent
{
    [SerializeField] protected EnemyEntity enemyController;
    protected virtual void ExplicitCasting()
    {
        this.enemyController = (EnemyEntity)entityController;
        // this.entityController = GetComponent<PlayerEntity>();
    }
    protected override void LoadComponents()
    {
        base.LoadComponents();
        this.ExplicitCasting();
    }
}