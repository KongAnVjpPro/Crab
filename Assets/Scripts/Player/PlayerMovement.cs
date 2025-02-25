using System.Collections;
using System.Collections.Generic;
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
    private int jumpBufferCounter = 0;
    [SerializeField] private int jumpBufferFrames = 60;
    private float coyoteTimeCounter = 0;
    [SerializeField] private float coyoteTime = 0.1f;
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

    private bool dashed = false;
    PlayerStateList pState;
    [SerializeField] protected Animator anim;
    [SerializeField] protected Rigidbody2D body;
    private float xAxis;
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
    private void Update()
    {
        GetInputs();
        UpdateJumpVariables();
        if (pState.dashing) return;
        Move();
        Jump();
        Flip();
        StartDash();
    }
    protected virtual void GetInputs()
    {
        xAxis = Input.GetAxis("Horizontal");
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
        if (Input.GetButtonUp("Jump") && body.velocity.y > 0)
        {
            body.velocity = new Vector2(body.velocity.x, 0);
            pState.jumping = false;
        }
        if (!pState.jumping)
        {
            if (jumpBufferCounter > 0 && coyoteTimeCounter > 0)//coyoteTime la khoang thoi gian giua 2 lan nhay
            {                                                   //jumpBufferCounter la bien check xem co phai nhan nut nhay ko
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
            jumpBufferCounter--;
        }
    }
}
