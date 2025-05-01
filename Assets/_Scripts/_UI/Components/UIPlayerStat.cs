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
    void Start()
    {
        Init();
    }
    void Update()
    {
        TakeDamage();
        if (healthSlider.value != currentHealth)
        {
            healthSlider.value = currentHealth;

        }
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
            currentHealth -= 1;
        }
    }
}