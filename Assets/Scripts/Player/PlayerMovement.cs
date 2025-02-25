using System.Collections;
using System.Collections.Generic;
using UnityEditor.Callbacks;
using UnityEngine;
using UnityEngine.Scripting.APIUpdating;

public class PlayerMovement : AnMonobehaviour
{
    [Header("Horizontal Movement Settings:")]
    [SerializeField] protected Rigidbody2D body;
    [SerializeField] protected float walkSpeed = 7f;
    private float xAxis;
    [Header("Ground Check Settings:")]
    [SerializeField] protected float jumpForce = 10;
    [SerializeField] protected Transform groundCheckPoint;
    [SerializeField] protected float groundCheckY = 0.2f;
    [SerializeField] protected float groundCheckX = 0.5f;
    [SerializeField] protected LayerMask whatIsGround;

    [SerializeField] protected Animator anim;
    protected override void LoadComponents()
    {
        base.LoadComponents();
        this.LoadRigibody();
        this.LoadGroundCheckPoint();
        this.LoadAnim();
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
    private void Update()
    {
        GetInputs();
        Move();
        Jump();
        Flip();
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
        }
        else if (xAxis > 0)
        {
            transform.localScale = new Vector2(1, transform.localScale.y);
        }
    }
    void Jump()
    {
        if (Input.GetButtonUp("Jump") && body.velocity.y > 0)
        {
            body.velocity = new Vector2(body.velocity.x, 0);
        }
        if (Input.GetButtonDown("Jump") && Grounded())
        {
            body.velocity = new Vector2(body.velocity.x, jumpForce);
        }
        anim.SetBool("Jumping", !Grounded());
    }
}
