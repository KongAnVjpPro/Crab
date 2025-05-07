using UnityEngine;
public class StatComponent : EntityComponent
{
    public enum StatType
    {
        Health, Mana, Stamina
    }
    [Header("Current Stats: ")]
    [SerializeField] protected float currentHealth = 5;
    [SerializeField] protected float currentMana = 5;
    [SerializeField] protected float currentStamina = 5;
    public float CurrentHealth => currentHealth;
    public float CurrentMana => currentMana;
    public float CurrentStamina => currentStamina;

    [Header("Total Stats: ")] // current max value
    [SerializeField] protected float totalHealth = 5;
    public float TotalHealth => totalHealth;
    [SerializeField] protected float totalMana = 5;
    public float TotalMana => totalMana;
    [SerializeField] protected float totalStamina = 5;
    public float TotalStamina => totalStamina;
    [Header("Max Stats: ")] // max can reach
    [SerializeField] protected float maxHealth = 25;
    [SerializeField] protected float maxMana = 25;
    [SerializeField] protected float maxStamina = 25;

    public virtual void ChangeCurrentStats(StatType statType, float amount)//add or sub
    {
        switch (statType)
        {
            case StatType.Health:
                currentHealth = Mathf.Clamp(currentHealth + amount, 0, totalHealth);
                break;
            case StatType.Mana:
                currentMana = Mathf.Clamp(currentMana + amount, 0, totalMana);
                break;
            case StatType.Stamina:
                currentStamina = Mathf.Clamp(currentStamina + amount, 0, totalStamina);
                break;
        }
    }
    public virtual void ChangeTotalStats(StatType statType, float amount)//add or sub
    {
        switch (statType)
        {
            case StatType.Health:
                totalHealth = Mathf.Clamp(totalHealth + amount, 0, maxHealth);
                break;
            case StatType.Mana:
                totalMana = Mathf.Clamp(totalMana + amount, 0, maxMana);
                break;
            case StatType.Stamina:
                totalStamina = Mathf.Clamp(totalStamina + amount, 0, maxStamina);
                break;
        }
    }
    public void Heal(float amount)
    {
        currentHealth = Mathf.Clamp(currentHealth + amount, 0, totalHealth);
    }
    public bool IsDead()
    {
        return currentHealth <= 0;
    }
}
