using System.Collections;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
public class WaveFade : New_SceneTransition
{
    public RectTransform upperSaw;
    public RectTransform lowerSaw;
    // [SerializeField] Vector2 originLeft;
    // [SerializeField] Vector2 originRight;
    protected override void LoadComponents()
    {
        base.LoadComponents();
        this.LoadOrigin();
    }
    protected virtual void LoadOrigin()
    {
    }
    public override IEnumerator AnimateTransitionIn()
    {
        Sequence seq = DOTween.Sequence();
        // seq.Join(upperSaw.DORotate(new Vector3(0, 0, -80), 0.25f));
        seq.Join(upperSaw.DORotate(new Vector3(0, 0, -90), 1f)).SetEase(Ease.InBounce);
        // seq.Join(lowerSaw.DORotate(new Vector3(0, 0, 80), 0.25f));
        seq.Join(lowerSaw.DORotate(new Vector3(0, 0, 90), 1f)).SetEase(Ease.InBounce);
        seq.Join(canvasGroup.DOFade(1, 1f).SetEase(Ease.InBounce));
        yield return seq.WaitForCompletion();


    }
    public override IEnumerator AnimateTransitionOut()
    {
        Sequence seq = DOTween.Sequence();
        seq.Join(upperSaw.DORotate(new Vector3(0, 0, 0), 1f)).SetEase(Ease.InBack);
        seq.Join(lowerSaw.DORotate(new Vector3(0, 0, 0), 1f)).SetEase(Ease.InBack);
        seq.Join(canvasGroup.DOFade(0, .75f).SetEase(Ease.Linear));
        yield return seq.WaitForCompletion();

    }
}