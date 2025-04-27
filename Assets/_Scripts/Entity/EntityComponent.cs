using UnityEngine;
public abstract class EntityComponent : MyMonobehaviour
{
    protected EntityController entityController;
    protected override void LoadComponents()
    {
        base.LoadComponents();
        this.LoadEntityCenter();
    }
    protected virtual void LoadEntityCenter()
    {
        if (this.entityController != null) return;
        this.entityController = GetComponent<EntityController>();
    }
}