using UnityEngine;
public class GameController : MyMonobehaviour
{
    private static GameController instance;
    public static GameController Instance => instance;

    public PlayerEntity playerController;

    protected override void LoadComponents()
    {
        base.LoadComponents();
        this.LoadSingleton();
        this.LoadPlayerEntity();
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
    protected virtual void LoadPlayerEntity()
    {
        if (this.playerController != null) return;
        playerController = GameObject.FindAnyObjectByType<PlayerEntity>();
    }
}