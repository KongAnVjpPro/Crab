using UnityEngine;
public class GameController : MyMonobehaviour
{
    private static GameController instance;
    public static GameController Instance => instance;
    public bool isBlockPlayerControl = false;
    public ItemSpawner itemSpawner;

    // public PlayerEntity playerController;

    protected override void LoadComponents()
    {
        base.LoadComponents();
        this.LoadSingleton();
        // this.LoadPlayerEntity();
        this.LoadItemSpawner();
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
}