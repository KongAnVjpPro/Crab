using System.Collections.Generic;
using UnityEngine;
public class PlayerEffect : PlayerComponent
{
    [SerializeField] GameObject slashEffect;
    [SerializeField] List<EffectController> slashPool;
    Dictionary<EffectAnimationID, EffectController> effectDict = new Dictionary<EffectAnimationID, EffectController>();
    [SerializeField] GameObject effectHolder;


    [SerializeField] ParticleSystem bubbleEffect;
    // [SerializeField] ParticleSystem bloodEff;
    [Range(0, 20)]
    [SerializeField] int occurAfterVelocity;
    [Range(0, 0.2f)]
    [SerializeField] float bubbleFormationPeriod;
    protected override void LoadComponents()
    {
        base.LoadComponents();
        this.LoadBubbleEffect();
        LoadDict();
    }
    void Start()
    {
        if (effectHolder == null)
        {
            effectHolder = GameController.Instance.effectHolder.gameObject;
        }
    }
    protected virtual void LoadBubbleEffect()
    {
        if (this.bubbleEffect != null) return;
        this.bubbleEffect = GetComponentInChildren<ParticleSystem>();
    }
    #region  pool spawn eff
    protected virtual void LoadDict()
    {
        EffectController[] effs = Resources.LoadAll<EffectController>("EffectPrefab/");
        foreach (var eff in effs)
        {
            effectDict.Add(eff.effectID, eff);
            Debug.Log("1");
        }
    }
    EffectController TakeFromPool(EffectAnimationID id)
    {
        foreach (var eff in slashPool)
        {
            if (eff.effectID == id && !eff.gameObject.activeSelf)
            {
                return eff;
            }
        }
        return null;
    }

    #endregion

    #region Effect
    public void SpawnEffect(Transform target, EffectAnimationID effectID)
    {
        EffectController newEff = TakeFromPool(effectID);
        if (newEff != null)
        {
            newEff.transform.position = target.position;
            newEff.gameObject.SetActive(true);
            return;
        }

        newEff = Instantiate(effectDict[effectID], target.position, Quaternion.identity);
        if (newEff == null) Debug.LogError("Null Eff", gameObject);
        slashPool.Add(newEff);
        newEff.transform.SetParent(effectHolder.transform);
        slashEffect.transform.position = target.position;
        newEff.gameObject.SetActive(true);
    }
    public void KnockedBack(Vector2 knockedBackDir)
    {
        playerController.playerRecoil.RecoilBoth(knockedBackDir.x > 0 ? 1 : -1, true);
        SpawnEffect(transform, EffectAnimationID.Hitted);
        // bloodEff.Play();
    }
    #endregion
    #region Particle 
    float counter;
    void Update()
    {
        counter += Time.deltaTime;
        if (Mathf.Abs(playerController.rb.velocity.x) > occurAfterVelocity)
        {
            if (counter > bubbleFormationPeriod)
            {
                bubbleEffect.Play();
                counter = 0;
            }
        }
    }
    public void PlayRunEffect()
    {
        bubbleEffect.Play();
    }
    #endregion
}
public enum EffectAnimationID
{
    none = 0,
    Slash = 1,
    Heal = 2,
    Hitted = 3,
}