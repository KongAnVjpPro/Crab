using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MyMonobehaviour
{
    [SerializeField] private PlayerController playerMovement;
    public PlayerController PlayerMovement => playerMovement;
    private static PlayerManager instance;
    public static PlayerManager Instance => instance;
    protected override void LoadComponents()
    {
        base.LoadComponents();
        this.LoadPlayerMovement();
    }
    protected virtual void LoadPlayerMovement()
    {
        if (this.playerMovement != null) return;
        this.playerMovement = GetComponent<PlayerController>();
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
