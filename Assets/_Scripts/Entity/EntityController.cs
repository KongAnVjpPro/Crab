using System;
using UnityEngine;
public abstract class EntityController : MyMonobehaviour
{
    //     public SpriteRenderer spriteRenderer;
    //     public StatComponent statComponent;
    //     public AnimatorComponent animatorComponent;
    public Rigidbody2D rb;
    //     public MovementComponent moveComponent;


    protected override void LoadComponents()
    {
        base.LoadComponents();
        // this.LoadStat();
        // this.LoadAnimator();
        this.LoadRigidbody();
        // this.LoadMoveComponent();
        // this.LoadSprite();
    }
    //     protected virtual void LoadStat()
    //     {
    //         if (this.statComponent != null) return;
    //         statComponent = GetComponent<StatComponent>();
    //     }
    //     protected virtual void LoadAnimator()
    //     {
    //         if (this.animatorComponent != null) return;
    //         animatorComponent = GetComponent<AnimatorComponent>();
    //     }
    protected virtual void LoadRigidbody()
    {
        if (this.rb != null) return;
        this.rb = GetComponent<Rigidbody2D>();
    }
    //     protected virtual void LoadMoveComponent()
    //     {
    //         if (this.moveComponent != null) return;
    //         this.moveComponent = GetComponent<MovementComponent>();
    //     }
    //     protected virtual void LoadSprite()
    //     {
    //         if (this.spriteRenderer != null) return;
    //         this.spriteRenderer = GetComponent<SpriteRenderer>();
    //     }
}