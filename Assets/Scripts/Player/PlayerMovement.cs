using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEditor.Callbacks;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.Scripting.APIUpdating;
using UnityEngine.UI;

public class PlayerMovement : AnMonobehaviour
{
    [Header("Horizontal Movement Settings:")]

    [SerializeField] protected float walkSpeed = 7f;
    [Space(5)]

    [Header("Vertical Movement Settings:")]
    [SerializeField] protected float jumpForce = 10;
    private float jumpBufferCounter = 0;
    [SerializeField] private float jumpBufferFrames = 0.1f;
    private float coyoteTimeCounter = 0;
    [SerializeField] private float coyoteTime = 0.15f;
    private int airJumpCounter = 0;
    [SerializeField] private int maxAirJumps = 1;
    [Space(5)]

    [Header("Ground Check Settings:")]

    [SerializeField] protected Transform groundCheckPoint;
    [SerializeField] protected float groundCheckY = 0.2f;
    [SerializeField] protected float groundCheckX = 0.5f;
    [SerializeField] protected LayerMask whatIsGround;
    [Space(5)]

    [Header("Dash Settings:")]
    [SerializeField] private float dashSpeed = 3f;
    [SerializeField] private float dashTime = 0.2f;
    [SerializeField] private float dashCoolDown = 0.35f;
    [SerializeField] private GameObject dashEffect;
    [Space(5)]

    [Header("Attacking")]
    [SerializeField] float timeBetweenAttack = 0.5f;
    float timeSinceAttack = 0f;
    bool attack = false;
    [SerializeField] Transform sideAttackTransform, upAttackTransform, downAttackTransform;
    [SerializeField] Vector2 sideAttackArea, upAttackArea, downAttackArea;
    [SerializeField] LayerMask attackableLayer;
    [SerializeField] float damage = 1f;
    [SerializeField] GameObject slashEffect;
    bool restoreTime;
    float restoreTimeSpeed;
    [Space(5)]

    [Header("Recoil")]
    [SerializeField] int recoilXSteps = 5;
    [SerializeField] int recoilYSteps = 5;
    [SerializeField] float recoilXSpeed = 100;
    [SerializeField] float recoilYSpeed = 100;
    int stepsXRecoiled = 0, stepsYRecoiled = 0;
    [Space(5)]

    [Header("Health Settings:")]
    public int health = 10;
    public int maxHealth = 10;
    [SerializeField] GameObject bloodSpurt;
    [SerializeField] float hitFlashSpeed = 2f;
    public delegate void OnHealthChangedDelegate();
    [HideInInspector] public OnHealthChangedDelegate onHealthChangedCallback;


    float healTimer;
    [SerializeField] float timeToHeal = 1;
    [Space(5)]

    [Header("Mana Settings:")]
    [SerializeField] Image manaStorage;
    [SerializeField] float mana = 1f;
    [SerializeField] float manaDrainSpeed = 0.2f;
    [SerializeField] float manaGain;


    [Space(5)]


    private bool dashed = false;
    protected PlayerStateList pState;
    public PlayerStateList PState => pState;
    [SerializeField] protected Animator anim;
    [SerializeField] protected Rigidbody2D body;
    private SpriteRenderer sr;
    private float xAxis, yAxis;
    private bool canDash = true;
    private float gravity;


    protected override void LoadComponents()
    {
        base.LoadComponents();
        this.LoadRigibody();
        this.LoadGroundCheckPoint();
        this.LoadAnim();
        this.LoadPlayerStateList();
        gravity = body.gravityScale;
        Health = maxHealth;
        sr = GetComponent<SpriteRenderer>();
        Mana = mana;
        manaStorage.fillAmount = Mana;
    }

    protected virtual void LoadRigibody()
    {
        if (this.body != null) return;
        this.body = GetComponent<Rigidbody2D>();
    }
    protected virtual void LoadGroundCheckPoint()
    {
        if (this.groundCheckPoint != null) return;
        this.groundCheckPoint = transform.Find("GroundCheckPoint");
    }
    protected virtual void LoadAnim()
    {
        if (this.anim != null) return;
        this.anim = GetComponent<Animator>();
    }
    protected virtual void LoadPlayerStateList()
    {
        if (this.pState != null) return;
        this.pState = GetComponent<PlayerStateList>();
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(sideAttackTransform.position, sideAttackArea);
        Gizmos.DrawWireCube(upAttackTransform.position, upAttackArea);
        Gizmos.DrawWireCube(downAttackTransform.position, downAttackArea);
    }
    private void Update()
    {
        GetInputs();
        UpdateJumpVariables();
        if (pState.dashing) return;
        Move();
        Jump();
        Flip();
        StartDash();
        Attack();
        RestoreTimeScale();
        FlashWhileInvincible();
        Heal();
    }
    void FixedUpdate()
    {
        if (pState.dashing) return;
        Recoil();
    }
    protected virtual void GetInputs()
    {
        xAxis = Input.GetAxis("Horizontal");
        yAxis = Input.GetAxis("Vertical");
        attack = Input.GetButtonDown("Attack");
    }
    protected virtual void Move()
    {
        body.velocity = new Vector2(walkSpeed * xAxis, body.velocity.y);
        anim.SetBool("Walking", body.velocity.x != 0 && Grounded());

    }
    void StartDash()
    {
        if (Input.GetButtonDown("Dash") && canDash && !dashed)
        {
            StartCoroutine(Dash());
            dashed = true;
        }
        if (Grounded())
        {
            dashed = false;
        }
    }
    IEnumerator Dash()
    {

        canDash = false;
        pState.dashing = true;
        anim.SetTrigger("Dashing");
        body.gravityScale = 0;

        body.velocity = new Vector2(transform.localScale.x * dashSpeed, 0);
        if (Grounded())
        {
            Instantiate(dashEffect, transform);
        }
        yield return new WaitForSeconds(dashTime);
        pState.dashing = false;
        body.gravityScale = gravity;
        yield return new WaitForSeconds(dashCoolDown);
        canDash = true;


    }
    void Attack()
    {
        timeSinceAttack += Time.deltaTime;
        if (attack && timeSinceAttack >= timeBetweenAttack)
        {
            timeSinceAttack = 0;
            anim.SetTrigger("Attacking");

            if (yAxis == 0 || yAxis < 0 && Grounded())
            {
                Hit(sideAttackTransform, sideAttackArea, ref pState.recoilingX, recoilXSpeed);
                Instantiate(slashEffect, sideAttackTransform);
            }
            else if (yAxis > 0)
            {
                Hit(upAttackTransform, upAttackArea, ref pState.recoilingY, recoilYSpeed);
                SlashEffectAtAngle(slashEffect, 90, upAttackTransform);
            }
            else if (yAxis < 0 && !Grounded())
            {
                Hit(downAttackTransform, downAttackArea, ref pState.recoilingY, recoilYSpeed);
                SlashEffectAtAngle(slashEffect, -90, downAttackTransform);
            }
        }
    }
    void Hit(Transform _attackTransform, Vector2 _attackArea, ref bool _recoilDir, float _recoilStrength)
    {
        Collider2D[] objectToHit = Physics2D.OverlapBoxAll(_attackTransform.position, _attackArea, 0, attackableLayer);
        List<Enemy> hitEnemies = new List<Enemy>();
        if (objectToHit.Length > 0)
        {
            _recoilDir = true;
        }
        for (int i = 0; i < objectToHit.Length; i++)
        {
            Enemy e = objectToHit[i].GetComponent<Enemy>();
            if (e != null)
            {
                if (hitEnemies.Contains(e))
                {
                    continue;
                }
                else
                {
                    hitEnemies.Add(e);
                    objectToHit[i].GetComponent<Enemy>().EnemyHit(damage, (transform.position - objectToHit[i].transform.position).normalized, _recoilStrength);
                    if (objectToHit[i].CompareTag("Enemy"))
                    {
                        Mana += manaGain;
                    }
                }

            }
        }
    }
    void SlashEffectAtAngle(GameObject _slashEffect, int _effectAngle, Transform _attackTransform)
    {
        _slashEffect = Instantiate(_slashEffect, _attackTransform);
        _slashEffect.transform.eulerAngles = new Vector3(0, 0, _effectAngle);
        _slashEffect.transform.localScale = new Vector2(transform.localScale.x, transform.localScale.y);
    }
    void StopRecoilX()
    {
        stepsXRecoiled = 0;
        pState.recoilingX = false;
    }
    void StopRecoilY()
    {
        stepsYRecoiled = 0;
        pState.recoilingY = false;

    }
    public void TakeDamage(float _damage)
    {
        Health -= Mathf.RoundToInt(_damage);
        StartCoroutine(StopTakingDamage());
    }
    IEnumerator StopTakingDamage()
    {
        pState.invicible = true;
        GameObject _bloodSpurt = Instantiate(bloodSpurt, transform.position, quaternion.identity);
        Destroy(_bloodSpurt, 1.5f);
        anim.SetTrigger("TakeDamage");
        yield return new WaitForSeconds(1f);
        pState.invicible = false;
    }
    void FlashWhileInvincible()
    {
        sr.material.color = pState.invicible ? Color.Lerp(Color.white, Color.black, Mathf.PingPong(Time.time * hitFlashSpeed, 1.0f)) : Color.white;
    }
    void RestoreTimeScale()
    {
        if (restoreTime)
        {
            if (Time.timeScale < 1)
            {
                Time.timeScale += Time.deltaTime * restoreTimeSpeed;
            }
            else
            {
                Time.timeScale = 1;
                restoreTime = false;
            }
        }
    }
    public void HitStopTime(float _newTimeScale, int _restoreSpeed, float _delay)
    {
        restoreTimeSpeed = _restoreSpeed;
        Time.timeScale = _newTimeScale;
        if (_delay > 0) //been attacked
        {
            StopCoroutine(StartTimeAgain(_delay));//avoid multiples hit stacking
            StartCoroutine(StartTimeAgain(_delay));
        }
        else
        {
            restoreTime = true;
        }
    }
    IEnumerator StartTimeAgain(float _delay)
    {
        restoreTime = true;
        yield return new WaitForSeconds(_delay);
    }
    public int Health
    {
        get { return health; }
        set
        {
            if (health != value)
            {
                health = Mathf.Clamp(value, 0, maxHealth);

                if (onHealthChangedCallback != null)
                {
                    onHealthChangedCallback.Invoke();
                }
            }
        }
    }
    public float Mana
    {
        get { return mana; }
        set
        {
            //if mana stats change
            if (mana != value)
            {
                mana = Mathf.Clamp(value, 0, 1);
                manaStorage.fillAmount = Mana;
            }
        }
    }
    void Heal()
    {
        if (Input.GetButton("Healing") && Health < maxHealth && (mana > 0) && !pState.jumping && !pState.dashing)
        {
            pState.healing = true;
            anim.SetBool("Healing", true);

            //healing
            healTimer += Time.deltaTime;
            if (healTimer >= timeToHeal)
            {
                Health++;
                healTimer = 0;
            }
            //drain mana
            Mana -= Time.deltaTime * manaDrainSpeed;

        }
        else
        {
            pState.healing = false;
            anim.SetBool("Healing", false);
            healTimer = 0;
        }
    }
    public bool Grounded()
    {
        if (Physics2D.Raycast(groundCheckPoint.position, Vector2.down, groundCheckY, whatIsGround) ||
        Physics2D.Raycast(groundCheckPoint.position + new Vector3(groundCheckX, 0, 0), Vector2.down, groundCheckY, whatIsGround) ||
        Physics2D.Raycast(groundCheckPoint.position + new Vector3(-groundCheckX, 0, 0), Vector2.down, groundCheckY, whatIsGround))
        {
            return true;
        }
        else
        {
            return false;
        }

    }
    void Recoil()
    {
        if (pState.recoilingX)
        {

            if (pState.lookingRight)
            {
                body.velocity = new Vector2(-recoilXSpeed, 0);
            }
            else
            {
                body.velocity = new Vector2(recoilXSpeed, 0);
            }
        }
        if (pState.recoilingY)
        {

            body.gravityScale = 0;
            if (yAxis < 0)
            {

                body.velocity = new Vector2(body.velocity.x, recoilYSpeed);
            }
            else
            {
                body.velocity = new Vector2(body.velocity.x, -recoilYSpeed);
            }
            airJumpCounter = 0;
        }
        else
        {
            body.gravityScale = gravity;
        }

        //stop recoil
        if (pState.recoilingX && recoilXSteps > stepsXRecoiled)
        {
            stepsXRecoiled++;
        }
        else
        {
            StopRecoilX();

        }


        if (pState.recoilingY && recoilYSteps > stepsYRecoiled)
        {
            stepsYRecoiled++;
        }
        else
        {
            StopRecoilY();
        }
        if (Grounded())
        {
            StopRecoilY();
        }
    }
    void Flip()
    {
        if (xAxis < 0)
        {
            transform.localScale = new Vector2(-1, transform.localScale.y);
            pState.lookingRight = false;
            // transform.eulerAngles = new Vector2(0, 180);
        }
        else if (xAxis > 0)
        {
            transform.localScale = new Vector2(1, transform.localScale.y);
            pState.lookingRight = true;
            // transform.eulerAngles = new Vector2(0, 0);
        }
    }
    void Jump()
    {
        if (Input.GetButtonUp("Jump") && body.velocity.y > 0)//tha nup jump
        {
            body.velocity = new Vector2(body.velocity.x, 0);
            pState.jumping = false;
        }
        if (!pState.jumping)//neu ko nhay
        {       //o day nghia la neu vua roi mat dat khoang thoi gian coyotetime > 0 va nhan nut nhay thi se nhay
            if (jumpBufferCounter > 0 && coyoteTimeCounter > 0)//coyoteTime la khoang thoi gian khi roi mat dat
            {                                                   //jumpBuffer khoang thoi gian nhan nut som
                body.velocity = new Vector2(body.velocity.x, jumpForce);
                pState.jumping = true;

            }
            else if (!Grounded() && airJumpCounter < maxAirJumps && Input.GetButtonDown("Jump"))
            {
                body.velocity = new Vector2(body.velocity.x, jumpForce);
                pState.jumping = true;
                airJumpCounter++;

            }

        }

        anim.SetBool("Jumping", !Grounded());
    }
    void UpdateJumpVariables()
    {
        if (Grounded())
        {
            this.pState.jumping = false;
            coyoteTimeCounter = coyoteTime;
            airJumpCounter = 0;
        }
        else
        {
            coyoteTimeCounter -= Time.deltaTime;
        }
        if (Input.GetButtonDown("Jump"))
        {
            jumpBufferCounter = jumpBufferFrames;
        }
        else
        {
            jumpBufferCounter -= Time.deltaTime;
        }
    }
}
