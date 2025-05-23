using UnityEngine;
public class ParticleController : EffectController
{
    [SerializeField] ParticleSystem ps;
    protected override void OnEnable()
    {
        ps.Play();
        base.OnEnable();
    }
    protected override void OnDisable()
    {
        ps.Stop();
        base.OnDisable();
    }
}