using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
public class EnemyHealthBar : EnemyComponent
{
    [SerializeField] CanvasGroup hpBarCanvas;
    [Header("HP: ")]
    [SerializeField] protected Slider healthSlider;
    [SerializeField] protected Slider easeHealthSlider;
    [SerializeField] protected float totalHealth;
    [SerializeField] protected float currentHealth;
    [SerializeField] float lerpSpeed = 0.05f;
    Tween tweener;
    public void FadeIn()
    {
        if (tweener != null && tweener.active)
        {
            tweener.Kill();
        }

        tweener = hpBarCanvas.DOFade(1f, 2f);
        hpBarCanvas.interactable = false;
        hpBarCanvas.blocksRaycasts = false;
    }
    public void FadeOut()
    {
        if (tweener != null && tweener.active)
        {
            tweener.Kill();
        }
        tweener = hpBarCanvas.DOFade(0, 2f);
        hpBarCanvas.interactable = false;
        hpBarCanvas.blocksRaycasts = false;
    }
    public void Init()
    {
        this.totalHealth = enemyController.enemyStat.TotalHealth;
        this.currentHealth = enemyController.enemyStat.CurrentHealth;
        healthSlider.maxValue = totalHealth;
        easeHealthSlider.maxValue = totalHealth;

    }
    void OnEnable()
    {

        enemyController.enemyStat.OnStatChange += UpdateStatUI;
    }
    void OnDisable()
    {
        enemyController.enemyStat.OnStatChange -= UpdateStatUI;
    }
    void Start()
    {
        Init();
        FadeOut();
        // UpdateStatUI();
    }

    void Update()
    {

        // hpBarCanvas.transform.localScale =  ;

        if (healthSlider.value != easeHealthSlider.value)
        {
            easeHealthSlider.value = Mathf.Lerp(easeHealthSlider.value, currentHealth, lerpSpeed);
        }

    }
    void UpdateStatUI()
    {
        currentHealth = enemyController.enemyStat.CurrentHealth;
        totalHealth = enemyController.enemyStat.TotalHealth;

        healthSlider.maxValue = totalHealth;
        easeHealthSlider.maxValue = totalHealth;

        healthSlider.value = currentHealth;

    }
}