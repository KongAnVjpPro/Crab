using System;
using UnityEngine;
public abstract class EntityController : MyMonobehaviour
{

    public Rigidbody2D rb;

    protected override void LoadComponents()
    {
        base.LoadComponents();
        this.LoadRigidbody();
    }
    protected virtual void LoadRigidbody()
    {
        if (this.rb != null) return;
        this.rb = GetComponent<Rigidbody2D>();
    }
}