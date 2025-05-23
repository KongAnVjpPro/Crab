using UnityEngine;
public class NPCComponent : EntityComponent
{
    [SerializeField] protected NPCController npcController;
    protected virtual void ExplicitCasting()
    {
        this.npcController = (NPCController)entityController;
        // this.entityController = GetComponent<PlayerEntity>();
    }
    protected override void LoadComponents()
    {
        base.LoadComponents();
        this.ExplicitCasting();
    }
}