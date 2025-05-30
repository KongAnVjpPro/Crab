using UnityEngine;
[RequireComponent(typeof(GroundCheck))]
[RequireComponent(typeof(WallCheck))]
public class PlayerMovement : PlayerComponent
{

    [Header("Move")]
    [SerializeField] protected float moveSpeed = 5f;
    [SerializeField] protected float scaledMoveSpeed = 1f;

    [SerializeField] bool canMove = true;
    [SerializeField] protected float jumpForce = 5f;
    float xAxis;
    float yAxis;
    [Header("Jump")]
    [SerializeField] protected float scaledJumpForce = 1f;
    [SerializeField] private float jumpBufferFrames = 0.1f;

    [SerializeField] private float coyoteTime = 0.15f;

    [SerializeField] private int maxAirJumps = 1;
    private int airJumpCounter = 0;
    private float jumpBufferCounter = 0;
    private float coyoteTimeCounter = 0;
    [SerializeField] float doubleJumpStamina = 2f;
    [Header("Wall Jump")]
    [SerializeField] private float wallSlidingSpeed = 2f;

    [SerializeField] private float wallJumpDuration;
    [SerializeField] private Vector2 wallJumpingPower;
    [SerializeField] float wallJumpThreshHold = 2f;

    float wallJumpDirection;
    bool isWallSliding;
    bool isWallJumping;
    void Update()
    {

        UpdateMoveVariable();
        UpdateJumpVariables();

        if (playerController.pState.blocking)
        {
            Move();
            return;
        }
        Dash();
        if (playerController.pState.dashing) return;
        if (!isWallJumping)
        {
            Move();
            Jump();
        }


        WallSlide();
        WallJump();
        PlayAnimation();
        PlayEffect();

    }
    #region Checker
    bool IsOnGround()
    {
        bool grounded = playerController.groundCheck.Grounded() || playerController.groundCheck.OtherLayerCheck(LayerMask.GetMask("Enemy"));
        if (grounded) playerController.playerDash.dashed = false;
        return grounded;
    }
    bool IsOnWall()
    {
        return playerController.wallCheck.Walled();
    }
    #endregion
    #region Move
    void UpdateMoveVariable()
    {
        xAxis = playerController.playerInput.xAxis;
        yAxis = playerController.playerInput.yAxis;
    }
    protected virtual void MoveHorizontal(float _xAxis)
    {
        if (canMove)
        {
            entityController.rb.velocity = new Vector2(moveSpeed * _xAxis * scaledMoveSpeed, entityController.rb.velocity.y);
        }
    }



    void Flip()
    {
        if (playerController.pState.attacking) return;
        if (xAxis < 0)
        {
            transform.localScale = new Vector2(-1, transform.localScale.y);
            playerController.pState.lookingRight = false;

        }
        else if (xAxis > 0)
        {
            transform.localScale = new Vector2(1, transform.localScale.y);
            playerController.pState.lookingRight = true;
        }
    }
    public virtual void Move()
    {
        if (canMove)
        {

            MoveHorizontal(xAxis);
        }
        Flip();

        //anim
        // bool isRunning = Mathf.Abs(xAxis) > 0.01f && IsOnGround();
        // playerController.playerAnimator.Running(isRunning);
    }
    #endregion
    #region Jump

    void UpdateJumpVariables()
    {

        playerController.pState.lookingUp = yAxis > 0 ? true : false;
        playerController.pState.lookingDown = yAxis < 0 ? true : false;
        if (IsOnGround())
        {
            playerController.pState.jumping = false;
            coyoteTimeCounter = coyoteTime;
            airJumpCounter = 0;
        }
        else
        {
            coyoteTimeCounter -= Time.deltaTime;
        }
        if (playerController.playerInput.jumpStart)
        {
            jumpBufferCounter = jumpBufferFrames;
        }
        else
        {
            jumpBufferCounter -= Time.deltaTime;
        }
    }
    void Jump()
    {
        // if (playerController.playerInput.jumpPress)
        // {
        //     playerController.rb.velocity = new Vector2(playerController.rb.velocity.x, jumpForce);
        // }
        // if (playerController.playerInput.jumpEnd)
        // {
        //     playerController.rb.velocity = new Vector2(playerController.rb.velocity.x, 0);
        // }
        if (jumpBufferCounter > 0 && coyoteTimeCounter > 0 && !playerController.pState.jumping)
        {
            playerController.rb.velocity = new Vector2(playerController.rb.velocity.x, jumpForce * scaledJumpForce);
            playerController.pState.jumping = true;


        }
        //double jump
        if (!IsOnGround() && airJumpCounter < maxAirJumps && playerController.playerInput.jumpStart)
        {
            if (playerController.pState.unlockedDoubleJump)
            {
                if (playerController.playerStat.CurrentStamina > doubleJumpStamina)
                {
                    playerController.playerStat.ChangeCurrentStats(StatComponent.StatType.Stamina, -doubleJumpStamina);
                    playerController.rb.velocity = new Vector2(playerController.rb.velocity.x, jumpForce * scaledJumpForce);
                    playerController.pState.jumping = true;
                    airJumpCounter++;
                }

            }


        }
        if (playerController.playerInput.jumpEnd && playerController.rb.velocity.y > 3)
        {
            playerController.rb.velocity = new Vector2(playerController.rb.velocity.x, 0);
            playerController.pState.jumping = false;
        }
    }

    #endregion
    #region Wall Jump
    void WallSlide()
    {
        if (playerController.playerStat.CurrentStamina < wallJumpThreshHold)
        {
            return;
        }
        if (IsOnWall() && !IsOnGround() && xAxis != 0)
        {
            isWallSliding = true;
            playerController.rb.velocity = new Vector2(playerController.rb.velocity.x, Mathf.Clamp(playerController.rb.velocity.y, -wallSlidingSpeed, float.MaxValue));

        }
        else
        {
            isWallSliding = false;
        }
    }
    void WallJump()
    {
        if (playerController.playerStat.CurrentStamina < wallJumpThreshHold)
        {
            return;
        }
        if (isWallSliding)
        {
            isWallJumping = false;
            wallJumpDirection = !playerController.pState.lookingRight ? 1 : -1;

            CancelInvoke(nameof(StopWallJumping));
        }
        if (playerController.playerInput.jumpStart && isWallSliding)
        {

            isWallJumping = true;
            playerController.rb.velocity = new Vector2(wallJumpDirection * wallJumpingPower.x, wallJumpingPower.y);
            // Debug.Log(playerController.rb.velocity.x);
            playerController.playerDash.dashed = false;
            airJumpCounter = 0;

            playerController.pState.lookingRight = !playerController.pState.lookingRight;
            transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
            Invoke(nameof(StopWallJumping), wallJumpDuration);
        }
    }
    void StopWallJumping()
    {
        isWallJumping = false;
        transform.localScale = new Vector3(1, 1, 1);
    }
    #endregion
    #region Dash
    void Dash()
    {
        if (!playerController.pState.unlockedDash) return;
        int _dir = playerController.pState.lookingRight ? 1 : -1;
        if (playerController.playerInput.dash)
        {
            playerController.playerDash.StartDash(_dir);
            // playerController.playerAnimator.Dashing();
        }
        playerController.pState.dashing = playerController.playerDash.dashing;
    }
    #endregion
    #region MovementAnim
    void PlayAnimation()
    {

        playerController.playerAnimator.Jumping(!IsOnGround() && playerController.rb.velocity.y > 0.1f);
        playerController.playerAnimator.Running((IsOnGround() && xAxis != 0));
        playerController.playerAnimator.Falling(playerController.rb.velocity.y);
        //dashing anim in dash func (playerdash)
    }
    void PlayEffect()
    {
        if (IsOnGround() && xAxis != 0)
        {
            // playerController.playerEffect.PlayRunEffect();
        }
    }
    #endregion
    #region Boost

    public void BoostSpeedAndJump(float boostMove, float boostJump)
    {
        scaledJumpForce = boostJump;
        scaledMoveSpeed = boostMove;
    }
    public void ResetBoost()
    {
        scaledJumpForce = 1f;
        scaledMoveSpeed = 1f;
    }
    #endregion

}