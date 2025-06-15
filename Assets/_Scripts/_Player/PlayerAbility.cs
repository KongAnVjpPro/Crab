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
    public void UnLockSpell(SpellType id)
    {
        switch (id)
        {
            case SpellType.TideBurst:
                playerController.pState.unlockedTideBurst = true;
                UIEntity.Instance.uiNotification.NoticeSomething(4f, "Unlocked Tide Burst Spell", "Check Inventory For Details");
                break;

            case SpellType.CrushingWave:
                playerController.pState.unlockedCurshingWave = true;
                UIEntity.Instance.uiNotification.NoticeSomething(4f, "Unlocked Crushing Wave Spell", "Check Inventory For Details");
                break;

            case SpellType.AbyssalPulse:
                playerController.pState.unlockedAbyssalPulse = true;
                UIEntity.Instance.uiNotification.NoticeSomething(4f, "Unlocked Abyssal Pulse Spell", "Check Inventory For Details");
                break;

            default:
                Debug.LogWarning("Unknown ability ID.");
                break;
        }
    }
}