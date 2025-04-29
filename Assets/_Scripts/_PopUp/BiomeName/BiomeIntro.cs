using System.Collections;
using DG.Tweening;
using UnityEngine;
public class BiomeIntro : AnimationInOut
{
    [SerializeField] float introTime = 3f;
    public override IEnumerator AnimateTransitionIn()
    {
        var tweener = canvasGroup.DOFade(1f, 1f).SetEase(Ease.InQuad);
        yield return tweener.WaitForCompletion();
    }
    public override IEnumerator AnimateTransitionOut()
    {
        var tweener = canvasGroup.DOFade(0, 1f).SetEase(Ease.OutQuad);
        yield return tweener.WaitForCompletion();
    }
    protected override void Awake()
    {
        base.Awake();
        StartCoroutine(Intro());
    }
    protected virtual IEnumerator Intro()
    {
        yield return AnimateTransitionIn();
        yield return new WaitForSeconds(introTime);
        yield return AnimateTransitionOut();
    }
}