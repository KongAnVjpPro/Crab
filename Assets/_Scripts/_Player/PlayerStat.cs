using UnityEngine;
public class PlayerStat : StatComponent
{
    [SerializeField] protected PlayerEntity playerController;
    [SerializeField] bool isStopRecoverStamina = false;
    [SerializeField] float staminaRecoverSpeed = 2f;
    [SerializeField] float delayRecover = 1f;
    [SerializeField] float staminaTimer = 0;

    [Header("Parry: ")]
    [SerializeField] float damageReduceRate = 0.8f;
    [SerializeField] float staminaBlockedSuccess = 2f;
    void Update()
    {
        if (!playerController.pState.alive) return;
        if (isStopRecoverStamina)
        {
            staminaTimer += Time.deltaTime;
            if (staminaTimer >= delayRecover)
            {
                isStopRecoverStamina = false;
                staminaTimer = 0;
            }

            return;
        }
        ChangeCurrentStats(StatType.Stamina, staminaRecoverSpeed * Time.deltaTime);

    }
    protected override void Awake()
    {
        base.Awake();
        if (playerController == null)
            playerController = entityController.GetComponent<PlayerEntity>();
    }
    public override void ChangeCurrentStats(StatType statType, float amount)
    {
        CheckStamina(statType, amount);
        if (statType == StatType.Health)
        {
            if (playerController.playerBlocking.isBlocking)
            {
                amount *= (1 - damageReduceRate);
                currentStamina = Mathf.Clamp(currentStamina - staminaBlockedSuccess, 0, totalStamina);
            }
        }
        base.ChangeCurrentStats(statType, amount);
        CheckDead(statType);
    }
    protected virtual void CheckDead(StatType statType)
    {
        if (statType != StatType.Health) return;
        if (IsDead())
        {

            playerController.playerAnimator.ResetTrigger();
            playerController.pState.alive = false;
            playerController.playerAnimator.Death();
            //do some death mechanic
        }
    }
    void CheckStamina(StatType statType, float amountChange)
    {
        if (statType != StatType.Stamina) return;
        if (amountChange <= 0)
            isStopRecoverStamina = true;
        staminaTimer = 0;
    }
}