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

    void Update()
    {
        UpdateVariables();
        UpdateAnimation();
    }
}