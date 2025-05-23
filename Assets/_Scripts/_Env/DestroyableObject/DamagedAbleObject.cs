using System.Collections;
using UnityEngine;
using UnityEngine.Events;
public class DamagedAbleObject : MyMonobehaviour
{
    [SerializeField] Animator objectAnimator;
    [Header("Config object stats: ")]
    [SerializeField] float objectHP = 3f;
    [SerializeField] float currentObjectHp = 3f;
    [SerializeField] Collider2D objectCollide;
    [SerializeField] bool canRevive = false;
    [SerializeField] bool isRevived = false;

    [SerializeField] float destroyedTime = 2f;
    [SerializeField] ParticleSystem particle;
    [SerializeField] bool isInvincible = false;
    public void PlayParticle()
    {
        if (particle == null) return;
        if (particle.isPlaying)
        {
            particle.Stop();
        }
        particle?.Play();
    }

    public void ReviveAnimation()
    {
        objectAnimator.SetTrigger("Revive");
    }
    public void DestroyedAnimation()
    {
        objectAnimator.SetTrigger("Death");
    }
    public void HittedAnimation()
    {
        objectAnimator.SetTrigger("Stunned");
    }
    public void ResetTrigger()
    {
        objectAnimator.ResetTrigger("Death");
        objectAnimator.ResetTrigger("Stunned");
        objectAnimator.ResetTrigger("Revive");
    }

    public void TakeDamage(float damage)
    {
        if (isInvincible)
        {
            return;
        }
        currentObjectHp -= damage;
        if (currentObjectHp <= 0)
        {
            DestroyObject();
        }
        else
        {
            HitObject();
        }
    }
    public virtual void HitObject()
    {
        ResetTrigger();
        HittedAnimation();
        OnHittedEvent?.Invoke();
    }
    public void DestroyObject()
    {
        objectCollide.enabled = false;


        DestroyedAnimation();
        if (!canRevive)
        {
            StartCoroutine(WaitForDestroyedTime());
            return;
        }
        if (isRevived)
        {
            StartCoroutine(WaitForDestroyedTime());
            return;
        }
        isRevived = true;
        // ResetAll();

        // gameObject.SetActive(false);
    }
    IEnumerator WaitForDestroyedTime()
    {
        OnDestroyedEvent?.Invoke();
        yield return new WaitForSeconds(destroyedTime);
        gameObject.SetActive(false);
    }
    public void ResetAll()
    {
        currentObjectHp = objectHP;
        objectCollide.enabled = true;
        ResetTrigger();

        if (canRevive)
        {
            ReviveAnimation();
        }
    }
    public UnityEvent OnHittedEvent;
    public UnityEvent OnDestroyedEvent;
}