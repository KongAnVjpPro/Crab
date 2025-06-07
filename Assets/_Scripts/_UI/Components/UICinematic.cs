using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
public class UICinematic : UIComponent
{
    [SerializeField] CanvasGroup mainCanvas;
    [SerializeField] Image upImg, downImg;
    [SerializeField] Vector2 originAnchorUpimg, originAnchorDownImg;
    [SerializeField] Vector2 anchorOffset = new Vector2(0, 250);
    protected override void LoadComponents()
    {
        base.LoadComponents();
        LoadAnchor();
    }
    protected virtual void LoadAnchor()
    {
        originAnchorUpimg = upImg.rectTransform.anchoredPosition;
        originAnchorDownImg = downImg.rectTransform.anchoredPosition;
        mainCanvas.interactable = false;
        mainCanvas.blocksRaycasts = false;
    }
    public void EnterCinematic(float time)
    {
        Sequence s = DOTween.Sequence();
        s.Join(upImg.rectTransform.DOAnchorPos(originAnchorUpimg - anchorOffset, time).SetEase(Ease.OutQuad));
        s.Join(downImg.rectTransform.DOAnchorPos(originAnchorDownImg + anchorOffset, time).SetEase(Ease.OutQuad));
    }
    public void ExitCinematic(float time)
    {
        Sequence s = DOTween.Sequence();
        s.Join(upImg.rectTransform.DOAnchorPos(originAnchorUpimg, time).SetEase(Ease.InQuad));
        s.Join(downImg.rectTransform.DOAnchorPos(originAnchorDownImg, time).SetEase(Ease.InQuad));
    }


}