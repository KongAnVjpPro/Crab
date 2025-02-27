using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Callbacks;
using UnityEngine;
using UnityEngine.Scripting.APIUpdating;

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



    private bool dashed = false;
    PlayerStateList pState;
    [SerializeField] protected Animator anim;
    [SerializeField] protected Rigidbody2D body;
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
    }
    protected virtual void GetInputs()
    {
        xAxis = Input.GetAxis("Horizontal");
        yAxis = Input.GetAxis("Vertical");
        attack = Input.GetMouseButtonDown(0);
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
                Hit(sideAttackTransform, sideAttackArea);
                Instantiate(slashEffect, sideAttackTransform);
            }
            else if (yAxis > 0)
            {
                Hit(upAttackTransform, upAttackArea);
                SlashEffectAtAngle(slashEffect, 90, upAttackTransform);
            }
            else if (yAxis < 0 && !Grounded())
            {
                Hit(downAttackTransform, downAttackArea);
                SlashEffectAtAngle(slashEffect, -90, downAttackTransform);
            }
        }
    }
    void Hit(Transform _attackTransform, Vector2 _attackArea)
    {
        Collider2D[] objectToHit = Physics2D.OverlapBoxAll(_attackTransform.position, _attackArea, 0, attackableLayer);

        if (objectToHit.Length > 0)
        {
            Debug.Log("HIt");
        }
        for (int i = 0; i < objectToHit.Length; i++)
        {
            if (objectToHit[i].GetComponent<Enemy>() != null)
            {
                objectToHit[i].GetComponent<Enemy>().EnemyHit(damage);
            }
        }
    }
    void SlashEffectAtAngle(GameObject _slashEffect, int _effectAngle, Transform _attackTransform)
    {
        _slashEffect = Instantiate(_slashEffect, _attackTransform);
        _slashEffect.transform.eulerAngles = new Vector3(0, 0, _effectAngle);
        _slashEffect.transform.localScale = new Vector2(transform.localScale.x, transform.localScale.y);
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
    void Flip()
    {
        if (xAxis < 0)
        {
            transform.localScale = new Vector2(-1, transform.localScale.y);
            // transform.eulerAngles = new Vector2(0, 180);
        }
        else if (xAxis > 0)
        {
            transform.localScale = new Vector2(1, transform.localScale.y);
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
