using System.Collections.Generic;
using UnityEngine;
public class PlayerEffect : PlayerComponent
{
    [SerializeField] GameObject slashEffect;
    [SerializeField] List<GameObject> slashPool;

    [SerializeField] ParticleSystem bubbleEffect;
    protected override void LoadComponents()
    {
        base.LoadComponents();
        this.LoadBubbleEffect();
    }
    protected virtual void LoadBubbleEffect()
    {
        if (this.bubbleEffect != null) return;
        this.bubbleEffect = GetComponentInChildren<ParticleSystem>();
    }
    void SpawnEffect()
    {

    }
    public void PlayRunEffect()
    {
        bubbleEffect.Play();
    }
}