using UnityEngine;
public class PlayerDash : DashComponent
{
    [SerializeField] protected PlayerEntity playerController;
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
        base.StartDash(_dir);
    }
    public override void DashAnimation()
    {
        base.DashAnimation();
        playerController.playerAnimator.Dashing();
    }
}