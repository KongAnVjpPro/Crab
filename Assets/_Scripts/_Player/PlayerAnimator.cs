using UnityEngine;
public class PlayerAnimator : PlayerComponent
{
    [SerializeField] protected Animator anim;
    protected override void LoadComponents()
    {
        base.LoadComponents();
        this.LoadAnim();
    }
    protected virtual void LoadAnim()
    {
        if (this.anim != null) return;
        anim = GetComponentInChildren<Animator>();
    }
    public void Running(bool value)
    {
        anim.SetBool("Running", value);
    }
    public void Jumping(bool value)
    {
        anim.SetBool("Jumping", value);
    }
    public void Falling(float value)
    {
        anim.SetFloat("yVelocity", value);
    }
    public void Dashing()
    {
        anim.SetTrigger("Dashing");
    }
}