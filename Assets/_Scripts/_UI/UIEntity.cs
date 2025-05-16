using UnityEngine;
public class UIEntity : EntityController
{
    private static UIEntity instance;
    public static UIEntity Instance => instance;
    public UIPlayerStat playerStat;
    public DialogueManager dialogueManager;
    public UIInventory uiInventory;
    public UIFadeScreen uISaveScreen;


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

        this.LoadPlayerStat();
        LoadDialog();
        this.LoadUIInventory();
        LoadSaveScreen();


        this.LoadSingleton();
    }
    protected virtual void LoadSaveScreen()
    {
        if (uISaveScreen != null) return;
        uISaveScreen = GetComponent<UIFadeScreen>();
    }
    protected virtual void LoadUIInventory()
    {
        if (this.uiInventory != null) return;
        uiInventory = GetComponent<UIInventory>();
    }
    protected virtual void LoadDialog()
    {
        if (this.dialogueManager != null) return;
        this.dialogueManager = GetComponent<DialogueManager>();
    }
    protected virtual void LoadPlayerStat()
    {
        if (this.playerStat != null) return;
        this.playerStat = GetComponent<UIPlayerStat>();
    }
}