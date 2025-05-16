using UnityEngine;
public class PlayerDash : DashComponent
{
    [SerializeField] protected PlayerEntity playerController;
    [SerializeField] float staminaUse = 3f;
    protected virtual void ExplicitCasting()
    {
        this.playerController = (PlayerEntity)entityController;
        // this.entityController = GetComponent<PlayerEntity>();
    }
    protected override void LoadComponents()
    {
        base.LoadComponents();
        this.ExplicitCasting();
    }
    public override void StartDash(float _dir)
    {
        if (playerController.playerStat.CurrentStamina < staminaUse)
        {
            return;
            //canh bao something
        }
        playerController.playerStat.ChangeCurrentStats(StatComponent.StatType.Stamina, -staminaUse);
        base.StartDash(_dir);
    }
    public override void DashAnimation()
    {
        base.DashAnimation();
        playerController.playerAnimator.Dashing();
    }
}