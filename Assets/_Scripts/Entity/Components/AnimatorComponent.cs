using UnityEngine;
public class AnimatorComponent : EntityComponent
{
    [SerializeField] protected Animator animator;

    protected override void LoadComponents()
    {
        base.LoadComponents();
        this.LoadAnim();
    }
    protected virtual void LoadAnim()
    {
        if (this.animator != null) return;
        this.animator = GetComponent<Animator>();
    }
    public virtual void PlayAnim()//truyen state vao hoac dinh nghia tung ham
    {

    }
}