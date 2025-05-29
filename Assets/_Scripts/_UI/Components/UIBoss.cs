using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
public class UIBoss : UIComponent
{
    [SerializeField] CanvasGroup mainCanvas;
    [SerializeField] TextMeshProUGUI bossName;
    [SerializeField] Slider hpBar;
    [SerializeField] Slider hpEasingBar;
    [SerializeField] BossController currentBoss;
    [SerializeField] protected float totalHealth;
    [SerializeField] protected float currentHealth;
    [SerializeField] float lerpSpeed = 0.05f;


    public void SetCurrentBoss(BossController boss)
    {
        if (currentBoss != null)
        {
            Unsubscribe();
            currentBoss = null;
        }
        currentBoss = boss;
        Subscribe();
        bossName.text = currentBoss.bossName;
    }
    public void Subscribe()
    {
        currentBoss.enemyStat.OnStatChange += UpdateHPUI;
    }
    public void Unsubscribe()
    {
        currentBoss.enemyStat.OnStatChange -= UpdateHPUI;
    }

    void UpdateHPUI()
    {
        currentHealth = currentBoss.enemyStat.CurrentHealth;
        totalHealth = currentBoss.enemyStat.TotalHealth;

        hpBar.maxValue = totalHealth;
        hpEasingBar.maxValue = totalHealth;

        hpBar.value = currentHealth;


    }
    public void FadeIn(float fadeTime)
    {
        mainCanvas.DOFade(1f, fadeTime);
    }
    public void FadeOut(float fadeTime)
    {
        mainCanvas.DOFade(0f, fadeTime);
    }
    void Update()
    {

        if (hpBar.value != hpEasingBar.value)
        {
            hpEasingBar.value = Mathf.Lerp(hpEasingBar.value, currentHealth, lerpSpeed);
        }


    }
}