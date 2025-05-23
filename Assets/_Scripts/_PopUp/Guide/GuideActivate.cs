using UnityEngine;
[RequireComponent(typeof(GuideTriggerEffect))]
public class GuideActivate : MyMonobehaviour
{
    [SerializeField] protected GuideTriggerEffect eff;
    protected override void LoadComponents()
    {
        base.LoadComponents();
        LoadEffect();
    }
    protected virtual void LoadEffect()
    {
        if (this.eff != null) return;
        this.eff = GetComponent<GuideTriggerEffect>();
    }
    public void EnterZone()
    {
        StartCoroutine(eff.AnimateTransitionIn());
    }
    public void ExitZone()
    {
        StartCoroutine(eff.AnimateTransitionOut());
    }
}