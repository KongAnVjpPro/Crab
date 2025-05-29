using System.Collections;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
public class UIFadeScreen : UIComponent
{
    [SerializeField] CanvasGroup fadeScreen;
    public IEnumerator EnterSaveScreen()
    {
        Tween tween = fadeScreen.DOFade(1, 1f);
        yield return tween.WaitForCompletion();
        // yield return null;
    }
    public IEnumerator ExitScreen()
    {
        // yield return null;
        Tween tween = fadeScreen.DOFade(0, 1f);
        yield return tween.WaitForCompletion();
    }
}