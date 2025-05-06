using UnityEngine;
public class PlayerEntity : EntityController
{
    private static PlayerEntity instance;
    public static PlayerEntity Instance => instance;

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

    public PlayerAnimator playerAnimator;
    public PlayerInput playerInput;
    public PlayerMovement playerMovement;
    public StatComponent playerStat;
    public SpriteRenderer sr;
    public PlayerAttack playerAttack;
    public PlayerState pState;
    public GroundCheck groundCheck;
    public WallCheck wallCheck;
    public PlayerDash playerDash;
    public PlayerEffect playerEffect;
    public PlayerBlocking playerBlocking;
    public PlayerInventory playerInventory;
    protected virtual void LoadSprite()
    {
        if (this.sr != null) return;
        this.sr = GetComponent<SpriteRenderer>();
    }
    protected virtual void LoadMovement()
    {
        if (this.playerMovement != null) return;
        playerMovement = GetComponent<PlayerMovement>();
    }
    protected virtual void LoadStat()
    {
        if (this.playerStat != null) return;
        playerStat = GetComponent<StatComponent>();
    }
    protected virtual void LoadAnim()
    {
        if (this.playerAnimator != null) return;
        playerAnimator = GetComponent<PlayerAnimator>();
    }
    protected virtual void LoadInput()
    {
        if (this.playerInput != null) return;
        playerInput = GetComponent<PlayerInput>();
    }
    protected virtual void LoadAttack()
    {
        if (this.playerAttack != null) return;
        playerAttack = GetComponent<PlayerAttack>();
    }
    protected virtual void LoadState()
    {
        if (this.pState != null) return;
        pState = GetComponent<PlayerState>();
    }
    protected virtual void LoadCheckGround()
    {
        if (this.groundCheck != null) return;
        groundCheck = GetComponent<GroundCheck>();
    }
    protected virtual void LoadWallCheck()
    {
        if (this.wallCheck != null) return;
        wallCheck = GetComponent<WallCheck>();
    }
    protected virtual void LoadPlayerDash()
    {
        if (playerDash != null) return;
        playerDash = GetComponent<PlayerDash>();
    }
    protected virtual void LoadPlayerEffect()
    {
        if (playerEffect != null) return;
        playerEffect = GetComponent<PlayerEffect>();
    }
    protected virtual void LoadPlayerBlock()
    {
        if (playerBlocking != null) return;
        playerBlocking = GetComponent<PlayerBlocking>();
    }
    protected virtual void LoadInventory()
    {
        if (playerInventory != null) return;
        playerInventory = GetComponent<PlayerInventory>();
    }
    protected override void LoadComponents()
    {
        base.LoadComponents();
        LoadSingleton();

        this.LoadSprite();
        LoadMovement();
        LoadAnim();
        LoadInput();
        LoadStat();
        LoadAttack();
        LoadState();
        LoadCheckGround();
        LoadWallCheck();
        LoadPlayerDash();
        LoadPlayerEffect();
        LoadPlayerBlock();
        LoadInventory();

    }
}