using DG.Tweening;
using UnityEngine;
public class TrapAppear : MyMonobehaviour
{
    [SerializeField] Vector2 offsetAppear = new Vector2(0, 1);
    // [SerializeField] float appearSpeed = 5f;
    [SerializeField] Vector2 originPos;
    [SerializeField] Vector2 inertia = new Vector2(0, 0.2f);
    [SerializeField] float originToInertiaTime = 0.5f;
    [SerializeField] float inertiaToOriginTime = 0.2f;
    [SerializeField] ParticleSystem dustEffect;

    [Header("Config time between Appear: ")]
    [SerializeField] float cooldownTime = 1.0f;
    [SerializeField] float cooldownTimer = 0f;
    [SerializeField] bool isTrapActive = false;
    [SerializeField] bool playerInside = false;
    [SerializeField] bool isAnimating = false;// anim cua disappear, tranh chong lan
    void Update()
    {
        if (!PlayerEntity.Instance.pState.alive)
        {
            if (isTrapActive)
            {
                AppearOut();
            }
            return;
        }
        if (cooldownTimer > 0)
        {
            cooldownTimer -= Time.deltaTime;
        }


        if (cooldownTimer <= 0 && !isTrapActive && playerInside && !isAnimating)
        {
            AppearIn();
        }
    }
    protected override void Awake()
    {
        base.Awake();
        originPos = transform.position;
    }
    void DustEffect()
    {
        if (dustEffect.isPlaying)
        {
            dustEffect.Clear();
        }
        dustEffect.Play();
    }

    Sequence s;
    public void TriggerEnter()
    {

        playerInside = true;
        // if (!isTrapActive || cooldownTimer <= 0)
        // {
        //     AppearIn();
        // }
    }
    public void TriggerExit()
    {
        playerInside = false;
        if (isTrapActive)
        {
            AppearOut();
        }


    }
    public void AppearIn()
    {

        isTrapActive = true;
        // cooldownTimer = cooldownTime;

        if (s != null && s.active) s.Kill();
        s = DOTween.Sequence();

        transform.position = originPos;
        DustEffect();


        // s.Join(transform.DOMove(originPos + inertia + offsetAppear, originToInertiaTime));
        // s.Append(transform.DOMove(originPos + offsetAppear, inertiaToOriginTime));

        s.Join(transform.DOMove(originPos + inertia + offsetAppear, originToInertiaTime).SetEase(Ease.OutCirc));
        s.Append(transform.DOMove(originPos + offsetAppear, inertiaToOriginTime).SetEase(Ease.InBounce));


    }
    public void AppearOut()
    {
        isTrapActive = false;
        isAnimating = true;
        cooldownTimer = cooldownTime;

        transform.position = originPos + inertia + offsetAppear;
        // transform.DOMove(tr)
        // DisappearEffect();
        if (s != null && s.active) s.Kill();
        s = DOTween.Sequence();
        s.Append(transform.DOMove(originPos, originToInertiaTime).SetEase(Ease.InBack));
        s.OnComplete(() =>
        {
            isAnimating = false;
            DustEffect();

        });
    }
}