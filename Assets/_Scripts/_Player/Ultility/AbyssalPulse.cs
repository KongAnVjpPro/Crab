using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class AbyssalPulseSpell : MyMonobehaviour
{
    [SerializeField] float existTime = 1f;
    [SerializeField] List<ParticleSystem> eff;
    [SerializeField] List<GameObject> octopusHand;
    [SerializeField] List<RangeAttack> octopusDealer;

    void StartParticle()
    {
        foreach (ParticleSystem p in eff)
        {
            p.Play();
        }
    }
    void StopParticle()
    {
        foreach (var p in eff)
        {
            p.Stop(true, ParticleSystemStopBehavior.StopEmitting);
        }
    }
    void OnEnable()
    {
        foreach (var hand in octopusHand)
        {
            hand.gameObject.SetActive(false);

        }
        foreach (var deal in octopusDealer)
        {
            deal.gameObject.SetActive(true);
        }
        StartParticle();
        StartCoroutine(DealAndDisable());
    }
    IEnumerator DealAndDisable()
    {
        foreach (var hand in octopusHand)
        {
            hand.gameObject.SetActive(true);

        }
        yield return new WaitForSeconds(existTime * 0.8f);
        StopParticle();
        yield return new WaitForSeconds(existTime * 0.2f);
        gameObject.SetActive(false);
    }
}