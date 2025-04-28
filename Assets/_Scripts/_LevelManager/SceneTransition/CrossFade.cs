using System.Collections;
using DG.Tweening;
using UnityEngine;
public class CrossFade : New_SceneTransition
{


    public override IEnumerator AnimateTransitionIn()
    {
        var tweener = canvasGroup.DOFade(1f, 1f);
        yield return tweener.WaitForCompletion();
    }

    public override IEnumerator AnimateTransitionOut()
    {
        var tweener = canvasGroup.DOFade(0f, 1f);
        yield return tweener.WaitForCompletion();
    }
}