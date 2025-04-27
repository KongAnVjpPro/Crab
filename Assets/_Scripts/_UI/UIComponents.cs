using UnityEngine;
public abstract class UIComponent : EntityComponent
{
    [SerializeField] protected UIEntity UIController;

    protected virtual void ExplicitCasting()
    {
        this.UIController = (UIEntity)entityController;
    }
    protected override void LoadComponents()
    {
        base.LoadComponents();
        this.ExplicitCasting();
    }
}