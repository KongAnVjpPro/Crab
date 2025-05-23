using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
public class UIDeathScene : UIComponent
{
    [SerializeField] CanvasGroup deathCanvas;
    [SerializeField] Image bgText;
    [SerializeField] Color redC = Color.red;
    [SerializeField] Color blackC = Color.black;
    [SerializeField] Vector2 startSizeBg = new Vector2(1920, 200);
    [SerializeField] Vector2 endSizeBg = new Vector2(1920, 160);
    [SerializeField] float lerpTimeBg = 3f;
    [Header("Respawn Button: ")]
    [SerializeField] Button respawnButton;
    [SerializeField] CanvasGroup respawnButtonGroup;

    public void ShowDeathScene()
    {
        UIController.uiInventory.HideInventory();
        ActivateCanvas(true);

    }
    public void OnClickRespawn()
    {
        ActivateCanvas(false);
        GameController.Instance.isBlockPlayerControl = false;

        //do some respawn mechanic
    }

    Sequence seq;
    public void ActivateCanvas(bool value)
    {
        UIController.isSomethingOpened = value;
        if (seq != null && seq.active)
        {
            seq.Kill();
        }
        seq = DOTween.Sequence();
        seq.Join(deathCanvas.DOFade(value ? 1 : 0, 1));
        // seq.Append
        bgText.color = value ? redC : blackC;
        bgText.rectTransform.sizeDelta = value ? startSizeBg : endSizeBg;
        seq.Append(bgText.DOColor(value ? blackC : redC, lerpTimeBg));
        seq.Join(bgText.rectTransform.DOSizeDelta(value ? endSizeBg : startSizeBg, lerpTimeBg));
        deathCanvas.blocksRaycasts = value;
        deathCanvas.interactable = value;
        // respawnButton.image.DOFade(1f, 1f);
        seq.Append(respawnButtonGroup.DOFade(value ? 1 : 0, 1f));
        respawnButtonGroup.interactable = value;
        respawnButtonGroup.blocksRaycasts = value;
    }
    void BGIn()
    {
        // bgText.rectTransform.do
    }
}