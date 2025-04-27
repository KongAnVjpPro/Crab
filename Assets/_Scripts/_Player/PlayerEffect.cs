using System.Collections.Generic;
using UnityEngine;
public class PlayerEffect : PlayerComponent
{
    [SerializeField] GameObject slashEffect;
    [SerializeField] List<GameObject> slashPool;

    [SerializeField] ParticleSystem bubbleEffect;
    [Range(0, 20)]
    [SerializeField] int occurAfterVelocity;
    [Range(0, 0.2f)]
    [SerializeField] float bubbleFormationPeriod;
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
    #region Particle 
    float counter;
    void Update()
    {
        counter += Time.deltaTime;
        if (Mathf.Abs(playerController.rb.velocity.x) > occurAfterVelocity)
        {
            if (counter > bubbleFormationPeriod)
            {
                bubbleEffect.Play();
                counter = 0;
            }
        }
    }
    #endregion
}