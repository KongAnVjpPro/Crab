using UnityEngine;
public class PlayerAudio : PlayerComponent
{
    public AudioSource footStepSource;


    public void ActivateFootStepAudio(bool isActive)
    {
        if (footStepSource == null) return;
        if (isActive)
        {
            footStepSource.enabled = true;
        }
        else
        {
            footStepSource.enabled = false;
        }
    }
}