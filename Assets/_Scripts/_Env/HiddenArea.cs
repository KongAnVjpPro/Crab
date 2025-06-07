using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
public class HiddenArea : MyMonobehaviour
{
    [SerializeField] float fadeTime = 2f;

    [SerializeField] List<SpriteRenderer> hiddenSr;
    Sequence s;
    public void Show()
    {
        if (s != null && s.IsActive())
        {
            s.Kill();
        }
        s = DOTween.Sequence();
        foreach (var sr in hiddenSr)
        {
            s.Join(sr.DOFade(0f, fadeTime));
        }
    }
    public void Hide()
    {
        if (s != null && s.IsActive())
        {
            s.Kill();
        }
        s = DOTween.Sequence();
        foreach (var sr in hiddenSr)
        {
            s.Join(sr.DOFade(1f, fadeTime));
        }
    }
}