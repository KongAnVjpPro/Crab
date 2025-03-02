using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MyMonobehaviour
{
    [SerializeField] protected PlayerMovement playerMovement;
    public PlayerMovement PlayerMovement => playerMovement;
    private static PlayerController instance;
    public static PlayerController Instance => instance;
    protected override void LoadComponents()
    {
        base.LoadComponents();
        this.LoadPlayerMovement();
    }
    protected virtual void LoadPlayerMovement()
    {
        if (this.playerMovement != null) return;
        this.playerMovement = GetComponent<PlayerMovement>();
    }
    protected override void Awake()
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
        base.Awake();
    }
}
