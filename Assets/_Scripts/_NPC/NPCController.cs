using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UIElements;
public class NPCController : EntityController
{
    public DialogueTrigger dialogueTrigger;
    public NPCAction npcAction;
    [SerializeField] bool canAppear = true;
    // public NPNCCanAppear npcEnum;
    public string npcKey;
    public Animator anim;
    // public Rigidbody2D rb;
    [Header("Disappear func: ")]
    [SerializeField] float disappearTime = 5f;
    public bool canTurn = true;
    public UnityEvent ActionOnDisappear;
    protected override void LoadComponents()
    {
        base.LoadComponents();
        LoadDialogue();
        LoadAction();
        LoadAnim();


        //

        // CanAppear();
    }
    void Start()
    {
        GetCanAppear();
        DoOnChangeAppear();
    }
    protected virtual void LoadDialogue()
    {
        if (this.dialogueTrigger != null) return;
        dialogueTrigger = GetComponent<DialogueTrigger>();
    }
    protected virtual void LoadAction()
    {
        if (this.npcAction != null) return;
        this.npcAction = GetComponent<NPCAction>();
    }
    protected virtual void LoadAnim()
    {
        if (this.anim != null) return;
        this.anim = GetComponentInChildren<Animator>();
    }

    public void DoOnChangeAppear()
    {
        // gameObject.SetActive(canAppear);
        if (canAppear)
        {

        }
        else
        {
            ActionOnDisappear?.Invoke();
            // gameObject.SetActive(false);
        }
    }
    public void Disappear()
    {
        anim.SetBool("Turn", false);
        StartCoroutine(DisappearAnim());
    }
    IEnumerator DisappearAnim()
    {
        rb.velocity = new Vector2(0, -1);
        anim.SetTrigger("Disappear");
        yield return new WaitForSeconds(0.1f);
        rb.velocity = new Vector2(0, velocityDisappear);
        // AnimationState astate = anim.GetCurrentAnimatorStateInfo(0);
        // while(astate.)
        // gameObject.GetComponent<DialogueTrigger
        dialogueTrigger.enabled = false;
        yield return new WaitForSeconds(disappearTime);
        // gameObject.SetActive(false);
        SetCanAppear(false);

    }
    public void GetCanAppear()//load 
    {
        CanAppear = SaveSystem.Instance.GetNPCAppear(npcKey);
    }
    public void SetCanAppear(bool val)
    {
        SaveSystem.Instance.SetNPCAppear(npcKey, val);
        GetCanAppear();
    }
    public bool CanAppear
    {
        get
        {
            return canAppear;
        }
        set
        {
            if (canAppear != value)
            {
                canAppear = value;
                DoOnChangeAppear();
            }
        }
    }
    [Header("Disappear eff: ")]
    [SerializeField] ParticleSystem bubbleEffect;
    // [SerializeField] ParticleSystem bloodEff;
    [Range(0, 20)]
    [SerializeField] int occurAfterVelocity;
    [Range(0, 0.2f)]
    [SerializeField] float bubbleFormationPeriod;
    float counter;
    [SerializeField] float velocityDisappear = 5f;
    void Update()
    {
        counter += Time.deltaTime;
        if (Mathf.Abs(rb.velocity.y) > occurAfterVelocity)
        {
            if (counter > bubbleFormationPeriod)
            {
                bubbleEffect.Play();
                counter = 0;
            }
        }



        if (!canAppear) return;
        if (!canTurn) return;
        bool turnValue = transform.position.x > PlayerEntity.Instance.transform.position.x;
        anim.SetBool("Turn", turnValue);



    }
    #region wait for disappear
    public void WaitForDisappear(float Time)
    {
        StartCoroutine(WaitForDisappearCoroutine(Time));
    }
    IEnumerator WaitForDisappearCoroutine(float Time)
    {
        yield return new WaitForSeconds(Time);
        // Disappear();
        SetCanAppear(false);
    }
    #endregion
    #region  sin move
    [Header("Sin Move Settings")]
    public Vector2 moveDirection = Vector2.right;
    public float moveSpeed = 2f;
    public float amplitude = 0.5f;
    public float frequency = 2f;

    public void StartSinMove(float duration)
    {

        StartCoroutine(MoveSinusoidal(duration));
    }

    private IEnumerator MoveSinusoidal(float duration)
    {

        Vector2 direction = moveDirection.normalized;
        Vector2 perpendicular = new Vector2(-direction.y, direction.x);

        float timeCounter = 0f;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            timeCounter += Time.deltaTime;
            elapsed += Time.deltaTime;

            float sinOffset = Mathf.Sin(timeCounter * frequency) * amplitude;
            Vector2 offset = perpendicular * sinOffset;

            Vector2 forwardMove = direction * moveSpeed * Time.deltaTime;

            transform.position += (Vector3)(forwardMove + offset * Time.deltaTime);

            yield return null;
        }

    }
    #endregion
}