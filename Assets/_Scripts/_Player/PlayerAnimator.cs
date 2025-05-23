using System;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
public class PlayerAnimator : PlayerComponent
{
    [SerializeField] protected Animator anim;
    protected override void LoadComponents()
    {
        base.LoadComponents();
        this.LoadAnim();
    }
    public void ResetTrigger()
    {
        anim.ResetTrigger("Blocking");
        anim.ResetTrigger("Dashing");
        anim.ResetTrigger("Hurting");

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
    public void Attacking()
    {
        anim.SetTrigger("Attacking");
    }
    public void AirAttacking()
    {
        anim.SetTrigger("AirAttacking");
    }
    public void DownAttacking()
    {
        anim.SetTrigger("DownAttacking");
    }
    public void Blocking(bool value)
    {
        anim.SetBool("Blocking", value);
    }

    public void Hurting()
    {
        anim.ResetTrigger("Hurting");
        anim.SetTrigger("Hurting");
    }
    public void Death()
    {
        anim.SetTrigger("Death");
    }

    void LateUpdate()
    {
        AnimatorStateInfo state = anim.GetCurrentAnimatorStateInfo(0);

        if (playerController.pState.attacking && !state.IsTag("Attack"))
        {
            playerController.pState.attacking = false;
        }
    }
    public bool IsInAttackAnim()
    {
        AnimatorStateInfo state = anim.GetCurrentAnimatorStateInfo(0);
        return state.IsTag("Attack");
    }
    public float GetCurrentAnimationTime()
    {
        AnimatorStateInfo state = anim.GetCurrentAnimatorStateInfo(0);
        float timeRemaining = state.length * Mathf.Clamp01(1 - state.normalizedTime % 1);
        return timeRemaining;
    }
}