using System.Collections;
using UnityEngine;
using UnityEngine.UI;
public class UIPlayerStat : UIComponent
{
    [SerializeField] protected Slider healthSlider;
    [SerializeField] protected Slider easeHealthSlider;
    [SerializeField] protected float totalHealth;
    [SerializeField] protected float currentHealth;
    [SerializeField] float lerpSpeed = 0.05f;
    private void Init()
    {
        this.totalHealth = PlayerEntity.Instance.playerStat.TotalHealth;
        this.currentHealth = PlayerEntity.Instance.playerStat.CurrentHealth;
        healthSlider.maxValue = totalHealth;
        easeHealthSlider.maxValue = totalHealth;
    }
    void OnEnable()
    {
        StartCoroutine(RegisterAction());
        // PlayerEntity.Instance.playerStat.OnStatChange += UpdateStatUI;
    }
    void OnDisable()
    {
        PlayerEntity.Instance.playerStat.OnStatChange -= UpdateStatUI;
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


    }
    //test
    void TakeDamage()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            PlayerEntity.Instance.playerStat.ChangeCurrentStats(StatComponent.StatType.Health, -1);
        }
    }
    void UpdateStatUI()
    {
        currentHealth = PlayerEntity.Instance.playerStat.CurrentHealth;
        totalHealth = PlayerEntity.Instance.playerStat.TotalHealth;

        healthSlider.maxValue = totalHealth;
        easeHealthSlider.maxValue = totalHealth;

        healthSlider.value = currentHealth;
        // Debug.Log("b");
    }
}