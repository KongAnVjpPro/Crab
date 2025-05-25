using System;
using System.Collections;
using DG.Tweening;
using TMPro;
using UnityEngine;
public class UINotification : UIComponent
{
    [Header("Unlocked: ")]
    [SerializeField] CanvasGroup unlockedCanvas;

    [SerializeField] float fadePercentDuration = 0.2f;
    [SerializeField] TextMeshProUGUI detailNotificationText;
    [SerializeField] TextMeshProUGUI mainNotificationText;

    public void UnlockedShellUI(float timer, string shellName)
    {
        mainNotificationText.text = "New Shell Unlocked";
        Sequence seq = DOTween.Sequence();
        detailNotificationText.alpha = 0;
        detailNotificationText.text = "'" + shellName + "'";

        // seq.Join(FadeIn(timer));
        seq.Join(unlockedCanvas.DOFade(1f, timer * fadePercentDuration * 0.5f));
        seq.Append(detailNotificationText.DOFade(1f, timer * fadePercentDuration * 0.5f));
        seq.AppendInterval(timer * Mathf.Clamp01(1 - 2 * fadePercentDuration));

        seq.Append(unlockedCanvas.DOFade(0f, timer * fadePercentDuration));
        seq.Join(detailNotificationText.DOFade(0, timer * fadePercentDuration));
        detailNotificationText.alpha = 0;

    }
    public void NoticeSomething(float timer, string mainText, string detailText)
    {
        mainNotificationText.text = mainText;
        Sequence seq = DOTween.Sequence();
        detailNotificationText.alpha = 0;
        detailNotificationText.text = detailText;

        // seq.Join(FadeIn(timer));
        seq.Join(unlockedCanvas.DOFade(1f, timer * fadePercentDuration * 0.5f));
        seq.Append(detailNotificationText.DOFade(1f, timer * fadePercentDuration * 0.5f));
        seq.AppendInterval(timer * Mathf.Clamp01(1 - 2 * fadePercentDuration));

        seq.Append(unlockedCanvas.DOFade(0f, timer * fadePercentDuration));
        seq.Join(detailNotificationText.DOFade(0, timer * fadePercentDuration));
        detailNotificationText.alpha = 0;
    }
}