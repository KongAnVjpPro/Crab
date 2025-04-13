using System.Collections;
using UnityEditor.Callbacks;
using UnityEngine;
public class DashComponent : EntityComponent
{
    protected float gravity;
    public bool canDash = true;
    public bool dashed;
    public bool dashing;
    [Header("Dash Settings:")]
    [SerializeField] protected float dashSpeed = 3f;
    [SerializeField] protected float dashTime = 0.2f;
    [SerializeField] protected float dashCoolDown = 0.35f;

    protected virtual void InitValue()
    {
        gravity = entityController.rb.gravityScale;
    }
    protected override void LoadComponents()
    {
        base.LoadComponents();
        InitValue();
    }
    public virtual void StartDash(float _dir)
    {
        if (canDash && !dashed)
        {
            StartCoroutine(Dash(_dir));
            dashed = true;
        }
    }
    IEnumerator Dash(float _dir)
    {

        canDash = false;
        dashing = true;
        entityController.rb.gravityScale = 0;
        DashAnimation();
        entityController.rb.velocity = new Vector2(_dir * dashSpeed, 0);

        yield return new WaitForSeconds(dashTime);
        dashing = false;
        entityController.rb.gravityScale = gravity;
        yield return new WaitForSeconds(dashCoolDown);
        canDash = true;
    }
    public virtual void DashAnimation()
    {

    }
}