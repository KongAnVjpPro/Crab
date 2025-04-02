using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;

using UnityEngine;

using UnityEngine.UI;

public class PlayerController : MyMonobehaviour
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
    [SerializeField] protected float groundCheckX = 0.6f;//box collider size /2
    [SerializeField] protected LayerMask whatIsGround;
    [Space(5)]
    [Header("Wall Jump Settings:")]
    [SerializeField] private float wallSlidingSpeed = 2f;
    [SerializeField] private Transform wallCheck;
    [SerializeField] private LayerMask wallLayer;
    [SerializeField] private float wallJumpDuration;
    [SerializeField] private Vector2 wallJumpingPower;
    float wallJumpDirection;
    bool isWallSliding;
    bool isWallJumping;

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
    [SerializeField] float recoilYSpeed = 32;
    int stepsXRecoiled = 0, stepsYRecoiled = 0;
    [Space(5)]

    [Header("Health Settings:")]
    public int health = 10;
    public int maxHealth = 10;
    public int maxTotalHealth = 10;
    public int heartShards;


    [SerializeField] GameObject bloodSpurt;
    [SerializeField] float hitFlashSpeed = 15f;
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
    public bool halfMana;
    [Space(5)]


    [Header("Spell Castings:")]
    [SerializeField] float manaSpellCost = 0.3f;
    [SerializeField] float timeBetweenCast = 0.5f;
    float timeSinceCast;
    [SerializeField] float spellDamage;
    [SerializeField] float downSpellForce;
    [SerializeField] GameObject sideSpellFireball;
    [SerializeField] GameObject upSpellExplosion;
    [SerializeField] GameObject downSpellFireball;
    [Space(5)]
    [Header("Camera Stuff")]
    [SerializeField] private float playerFallSpeedThreshold = -10;


    private bool dashed = false;
    protected PlayerStateList pState;
    public PlayerStateList PState => pState;
    [SerializeField] protected Animator anim;
    [SerializeField] protected Rigidbody2D rb;
    public Rigidbody2D Rb => rb;
    private List<SpriteRenderer> sr;
    private float xAxis, yAxis;
    private bool canDash = true;
    private float gravity;
    bool openMap;
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


                if (!halfMana)
                {
                    mana = Mathf.Clamp(value, 0, 1);
                }
                else
                {
                    mana = Mathf.Clamp(value, 0, 0.5f);
                }
                manaStorage.fillAmount = Mana;
            }

        }
    }
    #region LoadComponents
    protected override void LoadComponents()
    {
        base.LoadComponents();
        this.LoadRigibody();
        this.LoadGroundCheckPoint();
        this.LoadAnim();
        this.LoadPlayerStateList();
        this.LoadSprite();
        this.ResetValue();
        this.LoadSingleton();
    }
    private static PlayerController instance;
    public static PlayerController Instance => instance;

    //unlocking
    public bool unlockedWallJump;
    public bool unlockedDash;
    public bool unlockedVarJump;
    public bool unlockedSideCast;
    public bool unlockedDownCast;
    public bool unlockedUpCast;



    protected virtual void LoadSingleton()
    {
        if (instance == null)
        {
            instance = this;

        }
        else
        {
            if (instance != this)
            {
                Destroy(gameObject);
            }
        }
        DontDestroyOnLoad(gameObject);
    }
    protected override void ResetValue()
    {
        base.ResetValue();
        gravity = rb.gravityScale;
        Health = maxHealth;
        Mana = mana;
        manaStorage.fillAmount = Mana;
    }
    protected virtual void LoadSprite()
    {
        if (this.sr != null) return;
        this.sr = PlayerManager.Instance.PlayerModel.Sr;
    }
    protected virtual void LoadRigibody()
    {
        if (this.rb != null) return;
        this.rb = GetComponent<Rigidbody2D>();
    }
    protected virtual void LoadGroundCheckPoint()
    {
        if (this.groundCheckPoint != null) return;
        this.groundCheckPoint = transform.Find("GroundCheckPoint");
    }
    protected virtual void LoadAnim()
    {
        if (this.anim != null) return;
        this.anim = GetComponentInChildren<Animator>();
    }
    protected virtual void LoadPlayerStateList()
    {
        if (this.pState != null) return;
        this.pState = GetComponent<PlayerStateList>();
    }
    #endregion
    // private void OnDrawGizmos()
    // {
    //     Gizmos.color = Color.red;
    //     Gizmos.DrawWireCube(sideAttackTransform.position, sideAttackArea);
    //     Gizmos.DrawWireCube(upAttackTransform.position, upAttackArea);
    //     Gizmos.DrawWireCube(downAttackTransform.position, downAttackArea);
    // }
    #region Move
    void Start()
    {
        SaveData.Instance.LoadPlayerData();
        if (Health == 0)
        {
            pState.alive = false;
            GameManager.Instance.RespawnPlayer();
        }
    }
    private void Update()
    {
        if (pState.cutscene) return;
        if (pState.alive)
        {
            GetInputs();
            ToggleMap();
        }

        UpdateJumpVariables();
        UpdateCameraYDampForPlayerFall();
        RestoreTimeScale();
        if (pState.alive)
        {
            Heal();
        }

        if (pState.dashing || pState.healing) return;


        if (pState.alive)
        {
            if (!isWallJumping)
            {
                Flip();
                Move();
                Jump();

            }
            if (unlockedWallJump)
            {
                WallSlide();
                WallJump();
            }
            if (unlockedDash)
            {
                StartDash();
            }

            Attack();

            CastSpell();

        }
        FlashWhileInvincible();




    }
    void OnTriggerEnter2D(Collider2D _other)
    {
        if (_other.GetComponent<Enemy>() != null && pState.casting)
        {
            _other.GetComponent<Enemy>().EnemyGetHit(spellDamage, (_other.transform.position - transform.position).normalized, -recoilYSpeed);

        }
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
        openMap = Input.GetButton("Map");
    }
    void ToggleMap()
    {
        if (openMap)
        {
            UIManager.Instance.mapHandler.SetActive(true);
        }
        else
        {
            UIManager.Instance.mapHandler.SetActive(false);
        }
    }
    protected virtual void Move()
    {
        if (pState.healing) rb.velocity = new Vector2(0, 0);
        rb.velocity = new Vector2(walkSpeed * xAxis, rb.velocity.y);
        anim.SetBool("Walking", rb.velocity.x != 0 && Grounded());

    }
    void UpdateCameraYDampForPlayerFall()
    {
        //fall pass threshold
        if (rb.velocity.y < playerFallSpeedThreshold && !CameraManager.Instance.isLerpingYDamping && !CameraManager.Instance.hasLerpedYDamping)
        {
            StartCoroutine(CameraManager.Instance.LerpYDamping(true));
        }
        //standing or move up
        if (rb.velocity.y >= 0 && !CameraManager.Instance.isLerpingYDamping && CameraManager.Instance.hasLerpedYDamping)
        {
            CameraManager.Instance.hasLerpedYDamping = false;
            StartCoroutine(CameraManager.Instance.LerpYDamping(false));
        }


    }
    #endregion
    #region Dash
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
        rb.gravityScale = 0;
        int _dir = pState.lookingRight ? 1 : -1;
        rb.velocity = new Vector2(_dir * dashSpeed, 0);
        if (Grounded())
        {
            Instantiate(dashEffect, transform);
        }
        yield return new WaitForSeconds(dashTime);
        pState.dashing = false;
        rb.gravityScale = gravity;
        yield return new WaitForSeconds(dashCoolDown);
        canDash = true;


    }
    #endregion
    #region Attack Enemy
    void Attack()
    {
        timeSinceAttack += Time.deltaTime;
        if (attack && timeSinceAttack >= timeBetweenAttack)
        {
            timeSinceAttack = 0;
            anim.SetTrigger("Attacking");

            if (yAxis == 0 || yAxis < 0 && Grounded())
            {
                Hit(sideAttackTransform, sideAttackArea, ref pState.recoilingX, (pState.lookingRight ? Vector2.left : Vector2.right), recoilXSpeed);
                Instantiate(slashEffect, sideAttackTransform);
            }
            else if (yAxis > 0)
            {
                Hit(upAttackTransform, upAttackArea, ref pState.recoilingY, Vector2.down, recoilYSpeed);
                SlashEffectAtAngle(slashEffect, 90, upAttackTransform);
            }
            else if (yAxis < 0 && !Grounded())
            {
                Hit(downAttackTransform, downAttackArea, ref pState.recoilingY, Vector2.up, recoilYSpeed);
                SlashEffectAtAngle(slashEffect, -90, downAttackTransform);
            }
        }
    }
    void Hit(Transform _attackTransform, Vector2 _attackArea, ref bool _recoilBool, Vector2 _recoilDir, float _recoilStrength)
    {
        Collider2D[] objectToHit = Physics2D.OverlapBoxAll(_attackTransform.position, _attackArea, 0, attackableLayer);
        List<Enemy> hitEnemies = new List<Enemy>();
        if (objectToHit.Length > 0)
        {
            _recoilBool = true;
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
                    // (transform.position - objectToHit[i].transform.position).normalized
                    hitEnemies.Add(e);
                    objectToHit[i].GetComponent<Enemy>().EnemyGetHit(damage, _recoilDir, _recoilStrength);
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
    #endregion
    #region Take Damage
    public void TakeDamage(float _damage)
    {
        if (pState.alive)
        {
            Health -= Mathf.RoundToInt(_damage);
            if (Health <= 0)
            {
                Health = 0;
                StartCoroutine(Death());
            }
            else
            {
                StartCoroutine(StopTakingDamage());
            }

        }

    }
    IEnumerator StopTakingDamage()
    {
        pState.invincible = true;
        GameObject _bloodSpurt = Instantiate(bloodSpurt, transform.position, quaternion.identity);
        Destroy(_bloodSpurt, 1.5f);
        anim.SetTrigger("TakeDamage");
        yield return new WaitForSeconds(1f);
        pState.invincible = false;
    }
    void FlashWhileInvincible()
    {
        if (pState.invincible && !pState.cutscene)
        {
            foreach (SpriteRenderer child in sr)
            {
                child.material.color = Color.Lerp(Color.white, Color.black, Mathf.PingPong(Time.time * hitFlashSpeed, 1.0f));
            }
        }
        else
        {
            foreach (SpriteRenderer child in sr)
            {
                child.material.color = Color.white;
            }
        }


    }
    #endregion
    #region Time Scale When Hit
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
        yield return new WaitForSecondsRealtime(_delay);
    }
    #endregion
    #region Death & Respawn
    IEnumerator Death()
    {
        pState.alive = false;
        Time.timeScale = 1f;
        GameObject _bloodSpurt = Instantiate(bloodSpurt, transform.position, quaternion.identity);
        Destroy(_bloodSpurt, 1.5f);
        anim.SetTrigger("Death");

        //deactive
        rb.constraints = RigidbodyConstraints2D.FreezePosition;
        GetComponent<BoxCollider2D>().enabled = false;


        yield return new WaitForSeconds(0.9f);
        StartCoroutine(UIManager.Instance.ActivateDeathScreen());

        yield return new WaitForSeconds(0.9f);
        Instantiate(GameManager.Instance.shade, transform.position, quaternion.identity);
    }
    public void Respawned()
    {
        Debug.Log("respawned");
        if (!pState.alive)
        {
            rb.constraints = RigidbodyConstraints2D.None;
            rb.constraints = RigidbodyConstraints2D.FreezeRotation;
            GetComponent<BoxCollider2D>().enabled = true;

            pState.alive = true;
            halfMana = true;
            UIManager.Instance.SwitchMana(UIManager.ManaState.HalfMana);
            Mana = 0;
            Health = maxHealth;
            anim.Play("Hermit_Idle");
        }
    }
    public void RestoreMana()
    {
        halfMana = false;
        UIManager.Instance.SwitchMana(UIManager.ManaState.FullMana);
    }
    #endregion
    #region Heal
    void Heal()
    {
        if (Input.GetButton("Healing") && Health < maxHealth && (mana > 0) && Grounded() && !pState.dashing)
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
    #endregion
    #region Spell
    void CastSpell()
    {
        if (Input.GetButtonDown("CastSpell") && timeSinceCast >= timeBetweenCast && mana > manaSpellCost)
        {
            pState.casting = true;
            timeSinceCast = 0;
            StartCoroutine(CastCoroutine());
        }
        else
        {
            timeSinceCast += Time.deltaTime;
        }
        if (Grounded())
        {
            //disable downspell
            downSpellFireball.SetActive(false);
        }
        //if downspell actived
        if (downSpellFireball.activeInHierarchy)
        {
            rb.velocity += downSpellForce * Vector2.down;
        }
    }
    IEnumerator CastCoroutine()
    {
        //the middle of anim

        //side cast
        if ((yAxis == 0 || (yAxis < 0 && Grounded()) && unlockedSideCast))
        {
            anim.SetBool("Casting", true);
            yield return new WaitForSeconds(0.15f);
            GameObject _fireBall = Instantiate(sideSpellFireball, sideAttackTransform.position, quaternion.identity);

            //flip fireball
            if (pState.lookingRight)
            {
                _fireBall.transform.eulerAngles = Vector3.zero;
            }
            else
            {
                _fireBall.transform.eulerAngles = new Vector2(_fireBall.transform.eulerAngles.x, 180);
            }
            pState.recoilingX = true;

            Mana -= manaSpellCost;
            yield return new WaitForSeconds(0.35f);
        }
        //up cast
        else if ((yAxis > 0) && unlockedUpCast)
        {
            anim.SetBool("Casting", true);
            yield return new WaitForSeconds(0.15f);
            Instantiate(upSpellExplosion, transform);
            rb.velocity = Vector2.zero;

            Mana -= manaSpellCost;
            yield return new WaitForSeconds(0.35f);
        }
        else if ((yAxis < 0) && (!Grounded()) && unlockedDownCast)
        {
            anim.SetBool("Casting", true);
            yield return new WaitForSeconds(0.15f);
            downSpellFireball.SetActive(true);

            Mana -= manaSpellCost;
            yield return new WaitForSeconds(0.35f);
        }

        anim.SetBool("Casting", false);
        pState.casting = false;
    }
    #endregion

    #region Check Grounded
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
    #endregion
    #region Recoil
    void Recoil()
    {
        if (pState.recoilingX)
        {

            if (pState.lookingRight)
            {
                rb.velocity = new Vector2(-recoilXSpeed, 0);
            }
            else
            {
                rb.velocity = new Vector2(recoilXSpeed, 0);
            }
        }
        if (pState.recoilingY)
        {

            rb.gravityScale = 0;
            if (yAxis < 0)
            {

                rb.velocity = new Vector2(rb.velocity.x, recoilYSpeed);
            }
            else
            {
                rb.velocity = new Vector2(rb.velocity.x, -recoilYSpeed);
            }
            airJumpCounter = 0;
        }
        else
        {
            rb.gravityScale = gravity;
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
    #endregion
    #region Flip
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
    #endregion
    #region Jump
    void Jump()
    {


        if (jumpBufferCounter > 0 && coyoteTimeCounter > 0 && !pState.jumping)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            pState.jumping = true;

        }
        if (!Grounded() && airJumpCounter < maxAirJumps && Input.GetButtonDown("Jump") && unlockedVarJump)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            pState.jumping = true;
            airJumpCounter++;

        }
        if (Input.GetButtonUp("Jump") && rb.velocity.y > 3)
        {
            rb.velocity = new Vector2(rb.velocity.x, 0);
            pState.jumping = false;
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
    #endregion
    #region Wall Jump
    private bool Walled()
    {
        return (Physics2D.OverlapCircle(wallCheck.position, 0.2f, wallLayer));
    }
    void WallSlide()
    {
        if (Walled() && !Grounded() && xAxis != 0)
        {
            isWallSliding = true;
            rb.velocity = new Vector2(rb.velocity.x, Mathf.Clamp(rb.velocity.y, -wallSlidingSpeed, float.MaxValue));

        }
        else
        {
            isWallSliding = false;
        }
    }
    void WallJump()
    {
        if (isWallSliding)
        {
            isWallJumping = false;
            wallJumpDirection = !pState.lookingRight ? 1 : -1;

            CancelInvoke(nameof(StopWallJumping));
        }
        if (Input.GetButtonDown("Jump") && isWallSliding)
        {
            isWallJumping = true;
            rb.velocity = new Vector2(wallJumpDirection * wallJumpingPower.x, wallJumpingPower.y);

            //mid-air acrobatics
            dashed = false;
            airJumpCounter = 0;

            //rotate
            // if ((pState.lookingRight && transform.eulerAngles.y == 0) || (!pState.lookingRight && transform.eulerAngles.y != 0))
            // {
            //     pState.lookingRight = !pState.lookingRight;
            //     int _yRotation = pState.lookingRight ? 0 : 180;
            //     transform.eulerAngles = new Vector2(transform.eulerAngles.x, _yRotation);

            // }
            Invoke(nameof(StopWallJumping), wallJumpDuration);
        }
    }
    void StopWallJumping()
    {
        isWallJumping = false;
    }
    #endregion
    #region Scene
    public IEnumerator WalkIntoNewScene(Vector2 _exitDir, float _delay)
    {

        pState.invincible = true;
        //if exit direction is upwards

        if (_exitDir.y > 0)
        {
            rb.velocity = jumpForce * _exitDir;
        }
        if (_exitDir.x != 0)
        {
            xAxis = _exitDir.x > 0 ? 1 : -1;
        }
        Flip();
        yield return new WaitForSeconds(_delay);
        pState.invincible = false;
        pState.cutscene = false;
    }
    #endregion
}
