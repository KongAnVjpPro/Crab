using System;
using UnityEngine;
public class GameController : MyMonobehaviour
{
    private static GameController instance;
    public static GameController Instance => instance;
    public bool isBlockPlayerControl = false;
    public ItemSpawner itemSpawner;
    public Transform effectHolder;
    // public Transform bulletHolder;
    // public AbilityManager abilityManager;

    // public PlayerEntity playerController;
    [Header("Save Point: ")]
    [SerializeField] SavePoint savePoint;
    public Vector2 respawnPoint;
    public Vector2 trapRespawnPoint;
    protected override void LoadComponents()
    {
        base.LoadComponents();
        this.LoadSingleton();
        // this.LoadPlayerEntity();
        this.LoadItemSpawner();
        // this.LoadAbilityManager();
    }
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
    // protected virtual void LoadPlayerEntity()
    // {
    //     if (this.playerController != null) return;
    //     playerController = GameObject.FindAnyObjectByType<PlayerEntity>();
    // }
    protected virtual void LoadItemSpawner()
    {
        if (this.itemSpawner != null) return;
        this.itemSpawner = GetComponent<ItemSpawner>();
    }


    // protected virtual void LoadAbilityManager()
    // {
    //     if (this.abilityManager != null) return;
    //     this.abilityManager = GetComponent<AbilityManager>();
    // }
    #region Handle Player Death
    public void OnPlayerDeath()
    {
        //ui do sth
        UIEntity.Instance.uiDeathScene.ShowDeathScene();

        //block sth

        isBlockPlayerControl = true;
    }
    #endregion
    #region  Respawn Player
    public void RespawnPlayer()
    {

    }
    #endregion

}