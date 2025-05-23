using System.Collections;
using DG.Tweening;
using UnityEngine;
public class GuideTriggerEffect : AnimationInOut
{
    public override IEnumerator AnimateTransitionIn()
    {
        var tweener = canvasGroup.DOFade(1f, 1f).SetEase(Ease.InQuad);
        yield return tweener.WaitForCompletion();
    }
    public override IEnumerator AnimateTransitionOut()
    {
        var tweener = canvasGroup.DOFade(0f, 1f).SetEase(Ease.OutQuad);
        yield return tweener.WaitForCompletion();
    }

}