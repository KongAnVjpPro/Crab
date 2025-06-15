using UnityEngine;
public class PlayerBlocking : PlayerComponent
{
    [SerializeField] float staminaUsePerSecond = 0.5f;
    [SerializeField] float staminaThreshHold = 0.1f;
    [SerializeField] float reduceMovementWhileBlock = 0.5f;
    public bool isBlocking = false;

    void UpdateVariables()
    {
        if (playerController.playerInput.block)
        {
            playerController.pState.blocking = true;
        }
        else
        {
            playerController.pState.blocking = false;
            if (!playerController.playerMovement.isOnBuffMove)
            {
                playerController.playerMovement.ResetBoost();
            }

            isBlocking = false;
        }
    }
    void UpdateAnimation()
    {
        if (!playerController.pState.unlockedParry) return;
        playerController.playerAnimator.Blocking(playerController.pState.blocking);
    }
    public void BlockProgress()
    {
        // isBlocking = false;
        UpdateAnimation();
        if (!playerController.pState.unlockedParry) return;
        if (!playerController.pState.blocking) return;
        // UpdateAnimation();
        //handle block logic

        UpdateLogic();

    }
    protected virtual void UpdateLogic()
    {
        if (playerController.playerStat.CurrentStamina <= staminaThreshHold)
        {
            isBlocking = false;
            playerController.playerAnimator.Blocking(false);
            // playerController.playerMovement.ResetBoost();
            if (!playerController.playerMovement.isOnBuffMove)
            {
                playerController.playerMovement.ResetBoost();
            }
            return;
        }



        playerController.playerStat.ChangeCurrentStats(StatComponent.StatType.Stamina, -Time.deltaTime * staminaUsePerSecond);
        if (!playerController.playerMovement.isOnBuffMove)
        {
            // playerController.playerMovement.ResetBoost();
            playerController.playerMovement.BoostSpeedAndJump(reduceMovementWhileBlock, reduceMovementWhileBlock);
        }

        //block or do sth
        isBlocking = true;
    }
    void Update()
    {
        UpdateVariables();
        // UpdateAnimation();
        BlockProgress();
    }
}