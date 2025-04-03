using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeUI : MyMonobehaviour
{
    CanvasGroup canvasGroup;
    protected override void Awake()
    {
        base.Awake();
        this.LoadCanvasGroup();
    }
    protected virtual void LoadCanvasGroup()
    {
        if (this.canvasGroup != null) return;
        this.canvasGroup = GetComponent<CanvasGroup>();
    }
    IEnumerator FadeOut(float _seconds)
    {
        canvasGroup.interactable = false;
        canvasGroup.blocksRaycasts = false;
        canvasGroup.alpha = 1;
        while (canvasGroup.alpha > 0)
        {
            canvasGroup.alpha -= Time.unscaledDeltaTime / _seconds;
            yield return null;
        }
        yield return null;
    }
    IEnumerator Fadein(float _seconds)
    {

        canvasGroup.alpha = 0;
        while (canvasGroup.alpha < 1)
        {
            canvasGroup.alpha += Time.unscaledDeltaTime / _seconds;
            yield return null;
        }
        canvasGroup.interactable = true;
        canvasGroup.blocksRaycasts = true;
        yield return null;
    }
    public void FadeUIOut(float _seconds)
    {
        StartCoroutine(FadeOut(_seconds));
    }
    public void FadeUIIn(float _seconds)
    {
        StartCoroutine(Fadein(_seconds));
    }
}
