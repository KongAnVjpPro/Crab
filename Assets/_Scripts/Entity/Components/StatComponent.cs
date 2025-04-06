using UnityEngine;
public class StatComponent : EntityComponent
{
    public enum StatType
    {
        Health, Mana, Stamina
    }
    protected float currentHealth = 5;
    protected float currentMana = 5;
    protected float currentStamina = 5;
    public float CurrentHealth => currentHealth;
    public float CurrentMana => currentMana;
    public float CurrentStamina => currentStamina;

    [Header("Total Stats: ")]
    [SerializeField] protected float totalHealth = 20;
    [SerializeField] protected float totalMana = 20;
    [SerializeField] protected float totalStamina = 20;
    [Header("Max Stats: ")]
    [SerializeField] protected float maxHealth = 5;
    [SerializeField] protected float maxMana = 5;
    [SerializeField] protected float maxStamina = 5;

    public virtual void ChangeCurrentStats(StatType statType, float amount)
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
    public virtual void ChangeTotalStats(StatType statType, float amount)
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
}
