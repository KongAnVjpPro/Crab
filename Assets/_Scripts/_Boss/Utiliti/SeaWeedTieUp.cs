using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Rendering;
public class SeaWeedTieUp : MyMonobehaviour
{
    public ParticleSystem part;
    public List<SpriteRenderer> jailImg;
    float blockTime = 2f;
    void OnEnable()
    {
        Sequence s = DOTween.Sequence();
        part.Play();
        foreach (var sr in jailImg)
        {
            s.Join(sr.DOFade(1, 1));
        }
        StartCoroutine(Despawn());
    }
    IEnumerator Despawn()
    {
        yield return new WaitForSeconds(blockTime);
        Sequence s = DOTween.Sequence();
        foreach (var sr in jailImg)
        {
            s.Join(sr.DOFade(0, 0.5f));
        }
        yield return s.WaitForCompletion();
        part.Stop(true, ParticleSystemStopBehavior.StopEmitting);
        gameObject.SetActive(false);
    }
    void OnDisable()
    {

    }
}