using UnityEngine;
public class PlayerComponent : EntityComponent
{
    [SerializeField] protected PlayerEntity playerController;
    protected virtual void ExplicitCasting()
    {
        this.playerController = (PlayerEntity)entityController;
        // this.entityController = GetComponent<PlayerEntity>();
    }
    protected override void LoadComponents()
    {
        base.LoadComponents();
        this.ExplicitCasting();
    }
}