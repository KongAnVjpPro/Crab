using UnityEngine;
public class PlatformAction : MyMonobehaviour // co tac dung voi player
{
    protected override void Awake()
    {
        base.Awake();
        pushDir = transform.up;
    }

    #region Push Platform
    // [SerializeField]
    [SerializeField] float pushForce = 10f;
    [SerializeField] Vector2 pushDir;
    [SerializeField] float pushCoolDown = 4f;
    [SerializeField] float pushTimer = 0;
    [SerializeField] bool isPushing = false;
    [SerializeField] ParticleSystem pushParticle;
    [SerializeField] Animator animator;
    public void PushPlayer()
    {
        if (isPushing)
        {
            return;
        }
        isPushing = true;
        pushParticle.Play();
        animator.SetTrigger("Jump");
        PlayerEntity.Instance.rb.velocity = pushForce * pushDir;
    }

    #endregion
    void Update()
    {
        if (isPushing)
        {
            pushTimer += Time.deltaTime;
            if (pushTimer >= pushCoolDown)
            {
                pushTimer = 0;
                isPushing = false;
            }
        }
    }
}