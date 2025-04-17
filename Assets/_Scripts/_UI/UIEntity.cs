using UnityEngine;
public class UIEntity : EntityController
{
    private static UIEntity instance;
    public static UIEntity Instance => instance;
    public UIPlayerStat playerStat;



    private void LoadSingleton()
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
    protected override void LoadComponents()
    {
        base.LoadComponents();
        this.LoadSingleton();
        this.LoadPlayerStat();
    }
    protected virtual void LoadPlayerStat()
    {
        if (this.playerStat != null) return;
        this.playerStat = GetComponent<UIPlayerStat>();
    }
}