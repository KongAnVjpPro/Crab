using System.Collections;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
public class UIPlayerStat : UIComponent
{
    [Header("HP: ")]
    [SerializeField] protected Slider healthSlider;
    [SerializeField] protected Slider easeHealthSlider;
    [SerializeField] protected float totalHealth;
    [SerializeField] protected float currentHealth;
    [SerializeField] float lerpSpeed = 0.05f;

    [Header("Stamina: ")]
    [SerializeField] protected Slider staminaSlider;
    [SerializeField] protected Slider easeStaminaSlider;
    [SerializeField] protected float totalStamina;
    [SerializeField] protected float currentStamina;



    [Header("Others: ")]
    [SerializeField] TextMeshProUGUI coinText;
    [SerializeField] int previousCoin = -1;

    private void Init()
    {
        this.totalHealth = PlayerEntity.Instance.playerStat.TotalHealth;
        this.currentHealth = PlayerEntity.Instance.playerStat.CurrentHealth;
        healthSlider.maxValue = totalHealth;
        easeHealthSlider.maxValue = totalHealth;

        this.totalStamina = PlayerEntity.Instance.playerStat.TotalStamina;
        this.currentStamina = PlayerEntity.Instance.playerStat.CurrentStamina;
        staminaSlider.maxValue = totalStamina;
        easeStaminaSlider.maxValue = totalStamina;
    }
    void OnEnable()
    {
        StartCoroutine(RegisterAction());
        // PlayerEntity.Instance.playerStat.OnStatChange += UpdateStatUI;
    }
    void OnDisable()
    {
        PlayerEntity.Instance.playerStat.OnStatChange -= UpdateStatUI;
        PlayerEntity.Instance.playerInventory.OnCoinChange -= UpdateCoin;
    }
    void Start()
    {
        Init();
        // UpdateStatUI();
    }
    IEnumerator RegisterAction()
    {
        // Init();

        yield return new WaitUntil(() => PlayerEntity.Instance != null && PlayerEntity.Instance.playerStat != null);

        PlayerEntity.Instance.playerStat.OnStatChange += UpdateStatUI;
        PlayerEntity.Instance.playerInventory.OnCoinChange += UpdateCoin;
        UpdateStatUI();

    }
    void Update()
    {
        // TakeDamage();
        // if (healthSlider.value != currentHealth)
        // {
        //     healthSlider.value = currentHealth;

        // }
        // TakeDamage();
        if (healthSlider.value != easeHealthSlider.value)
        {
            easeHealthSlider.value = Mathf.Lerp(easeHealthSlider.value, currentHealth, lerpSpeed);
        }

        if (staminaSlider.value != easeStaminaSlider.value)
        {
            easeStaminaSlider.value = Mathf.Lerp(easeStaminaSlider.value, currentStamina, lerpSpeed);
        }

    }
    //test

    void UpdateStatUI()
    {
        currentHealth = PlayerEntity.Instance.playerStat.CurrentHealth;
        totalHealth = PlayerEntity.Instance.playerStat.TotalHealth;

        healthSlider.maxValue = totalHealth;
        easeHealthSlider.maxValue = totalHealth;

        healthSlider.value = currentHealth;


        currentStamina = PlayerEntity.Instance.playerStat.CurrentStamina;
        totalStamina = PlayerEntity.Instance.playerStat.TotalStamina;

        staminaSlider.maxValue = totalStamina;
        easeStaminaSlider.maxValue = totalStamina;

        staminaSlider.value = currentStamina;
        // Debug.Log("b");
    }
    void UpdateCoin()
    {
        int currentCoin = PlayerEntity.Instance.playerInventory.GetCurrentCoin();

        if (previousCoin == -1)
        {
            previousCoin = currentCoin;
            coinText.text = currentCoin.ToString();
            return;
        }

        bool isIncrease = currentCoin > previousCoin;


        Color originalColor = Color.white;
        Color targetColor = isIncrease ? Color.green : Color.red;


        Vector3 originalScale = new Vector3(1, 1, 1);
        Vector3 punchScale = originalScale * 1.2f;


        coinText.text = currentCoin.ToString();


        Sequence s = DOTween.Sequence();
        s.Append(coinText.DOColor(targetColor, 0.2f))
         .Join(coinText.transform.DOPunchScale(punchScale, 0.3f, 10, 1))
         .AppendInterval(0.5f)
         .Append(coinText.DOColor(originalColor, 0.2f));

        previousCoin = currentCoin;
    }
}