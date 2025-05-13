using UnityEngine;
public class PlayerAbility : PlayerComponent
{
    public void UnlockAbility(CombinedShellData shell)
    {
        if (shell == null) return;
        foreach (var aID in shell.combinedAbilities)
        {
            UnlockAbility(aID, true);
        }
    }
    public void RemoveAbility(CombinedShellData shell)
    {
        if (shell == null) return;
        foreach (var aID in shell.combinedAbilities)
        {
            UnlockAbility(aID, false);
        }
    }
    void UnlockAbility(ShellAbilityID id, bool unlockedValue)
    {
        switch (id)
        {
            case ShellAbilityID.Dash:
                playerController.pState.unlockedDash = unlockedValue;
                break;

            case ShellAbilityID.DoubleJump:
                playerController.pState.unlockedDoubleJump = unlockedValue;
                break;

            case ShellAbilityID.Parry:
                playerController.pState.unlockedParry = unlockedValue;
                break;

            default:
                Debug.LogWarning("Unknown ability ID.");
                break;
        }
    }
}