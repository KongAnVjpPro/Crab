using System.Collections;
using UnityEngine;
public abstract class AnimationInOut : MyMonobehaviour
{
    public CanvasGroup canvasGroup;
    protected override void LoadComponents()
    {
        base.LoadComponents();
        LoadCanvasGroup();
    }
    protected virtual void LoadCanvasGroup()
    {
        if (this.canvasGroup != null) return;
        this.canvasGroup = GetComponent<CanvasGroup>();
    }
    public abstract IEnumerator AnimateTransitionIn();
    public abstract IEnumerator AnimateTransitionOut();
}