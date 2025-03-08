using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MyMonobehaviour
{
    [SerializeField] private PlayerController playerMovement;
    public PlayerController PlayerMovement => playerMovement;
    private static PlayerManager instance;
    public static PlayerManager Instance => instance;
    private PlayerModel playerModel;
    public PlayerModel PlayerModel => playerModel;
    protected override void LoadComponents()
    {
        base.LoadComponents();
        this.LoadPlayerMovement();
        this.LoadModel();
    }
    protected virtual void LoadModel()
    {
        if (this.playerModel != null) return;
        this.playerModel = GetComponentInChildren<PlayerModel>();
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
