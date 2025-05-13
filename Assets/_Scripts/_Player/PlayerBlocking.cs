using UnityEngine;
public class PlayerBlocking : PlayerComponent
{
    void UpdateVariables()
    {
        if (playerController.playerInput.block)
        {
            playerController.pState.blocking = true;
        }
        else
        {
            playerController.pState.blocking = false;
        }
    }
    void UpdateAnimation()
    {
        playerController.playerAnimator.Blocking(playerController.pState.blocking);
    }
    public void BlockProgress()
    {
        if (!playerController.pState.unlockedParry) return;
        UpdateAnimation();
        //handle block logic
    }
    void Update()
    {
        UpdateVariables();
        // UpdateAnimation();
        BlockProgress();
    }
}