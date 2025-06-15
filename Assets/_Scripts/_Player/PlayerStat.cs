using Unity.VisualScripting;
using UnityEngine;
public class PlayerStat : StatComponent
{
    [SerializeField] protected PlayerEntity playerController;
    [SerializeField] bool isStopRecoverStamina = false;
    [SerializeField] float staminaRecoverSpeed = 2f;
    [SerializeField] float delayRecover = 1f;
    [SerializeField] float staminaTimer = 0;

    [Header("Parry: ")]
    // [SerializeField] float damageReduceRate = 0.8f;
    [SerializeField] float staminaBlockedSuccess = 2f;

    [Header("Invincible: ")]
    [SerializeField] float invincibleTime = 0.5f;
    [SerializeField] float invincibleCounter = 0f;
    void Update()
    {
        if (!playerController.pState.alive)
        {
            // if (playerController.pState.invincible)
            // {
            //     playerController.pState.invincible = false;
            // }
            return;
        }
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


        if (playerController.pState.invincible)
        {
            invincibleCounter += Time.deltaTime;
            if (invincibleCounter >= invincibleTime)
            {
                playerController.pState.invincible = false;
                invincibleCounter = 0;
            }
        }
    }
    protected override void Awake()
    {
        base.Awake();
        if (playerController == null)
            playerController = entityController.GetComponent<PlayerEntity>();
        if (damageReduceRate == 0)
        {
            damageReduceRate = 0.8f;
        }
    }
    // public bool isHealDecrease = true;
    public override void ChangeCurrentStats(StatType statType, float amount)
    {
        CheckStamina(statType, amount);
        if (statType == StatType.Health)
        {
            // isHealDecrease = amount >= 0 ? false : true;
            if (playerController.playerBlocking.isBlocking)
            {
                amount *= (1 - damageReduceRate);
                currentStamina = Mathf.Clamp(currentStamina - staminaBlockedSuccess, 0, totalStamina);
            }
            if (amount > 0)
            {
                Debug.Log("Heal");
                playerController.playerEffect.SpawnEffect(playerController.transform, EffectAnimationID.Heal);
            }
        }
        // else
        // {
        //     // isHealDecrease = true;
        // }
        base.ChangeCurrentStats(statType, amount);
        CheckDead(statType);
    }
    protected virtual void CheckDead(StatType statType)
    {
        if (statType != StatType.Health) return;
        if (IsDead())
        {
            if (playerController.pState.invincible)
            {
                playerController.pState.invincible = false;
            }


            playerController.playerAnimator.ResetTrigger();
            playerController.pState.alive = false;
            playerController.playerAnimator.Death();
            // playerController.selfCollider.enabled = false;
            //do some death mechanic

            GameController.Instance.OnPlayerDeath();
        }
    }
    void CheckStamina(StatType statType, float amountChange)
    {
        if (statType != StatType.Stamina) return;
        if (amountChange <= 0)
            isStopRecoverStamina = true;
        staminaTimer = 0;
    }
    public override void ReceiveDamage(Vector2 knockedBackDir, float damageReceive)
    {

        base.ReceiveDamage(knockedBackDir, damageReceive);
        if (!playerController.pState.alive) return;
        if (playerController.pState.invincible) return;
        playerController.pState.invincible = true;

        ChangeCurrentStats(StatComponent.StatType.Health, -damageReceive);
        playerController.playerAnimator.Hurting();
        playerController.playerEffect.KnockedBack(knockedBackDir);


    }
    #region  Respawn
    public void RespawnPlayer()
    {

        RestoreStat();
        playerController.pState.alive = true;
        // playerController.playerAnimator.ResetTrigger();
        playerController.playerAnimator.Respawn();




    }
    public void RestoreStat()
    {

        ChangeCurrentStats(StatType.Health, totalHealth);
        ChangeCurrentStats(StatType.Mana, totalStamina);
        ChangeCurrentStats(StatType.Stamina, totalStamina);
    }


    #endregion

}