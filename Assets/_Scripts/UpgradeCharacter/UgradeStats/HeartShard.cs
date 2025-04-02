using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HeartShard : MyMonobehaviour
{
    public Image fill;
    public float targetFillAmount;
    public float lerpDuration = 1.5f;
    public float initialFIllAmount;
    public IEnumerator LerpFill()
    {
        float elapsedTime = 0f;

        while (elapsedTime < lerpDuration)
        {

            elapsedTime += Time.deltaTime;
            float t = Mathf.Clamp01(elapsedTime / lerpDuration);

            float lerpFillAmount = Mathf.Lerp(initialFIllAmount, targetFillAmount, t);
            fill.fillAmount = lerpFillAmount;
            yield return null;
        }
        fill.fillAmount = targetFillAmount;

        if (fill.fillAmount == 1)
        {
            PlayerController.Instance.maxHealth++;
            PlayerController.Instance.onHealthChangedCallback();
            PlayerController.Instance.heartShards = 0;
        }
    }
}
