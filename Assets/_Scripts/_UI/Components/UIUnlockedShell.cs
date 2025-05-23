using System;
using System.Collections;
using DG.Tweening;
using TMPro;
using UnityEngine;
public class UIShellManager : UIComponent
{
    [Header("Unlocked: ")]
    [SerializeField] CanvasGroup unlockedCanvas;

    [SerializeField] float fadePercentDuration = 0.2f;
    [SerializeField] TextMeshProUGUI shellTextName;

    public void UnlockedShellUI(float timer, string shellName)
    {
        Sequence seq = DOTween.Sequence();
        shellTextName.alpha = 0;
        shellTextName.text = "'" + shellName + "'";

        // seq.Join(FadeIn(timer));
        seq.Join(unlockedCanvas.DOFade(1f, timer * fadePercentDuration * 0.5f));
        seq.Append(shellTextName.DOFade(1f, timer * fadePercentDuration * 0.5f));
        seq.AppendInterval(timer * Mathf.Clamp01(1 - 2 * fadePercentDuration));

        seq.Append(unlockedCanvas.DOFade(0f, timer * fadePercentDuration));
        seq.Join(shellTextName.DOFade(0, timer * fadePercentDuration));
        shellTextName.alpha = 0;

    }
}