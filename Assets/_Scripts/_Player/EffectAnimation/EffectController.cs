using UnityEngine;
public class EffectController : MyMonobehaviour
{
    public EffectAnimationID effectID = EffectAnimationID.none;
    public float destroyedTime = 0.2f;
    public void SelfDestruct()
    {
        Invoke(nameof(this.Despawn), destroyedTime);
    }
    public void Despawn()
    {
        gameObject.SetActive(false);
    }

    protected virtual void OnEnable()
    {
        SelfDestruct();
    }
    protected virtual void OnDisable()
    {

    }

}