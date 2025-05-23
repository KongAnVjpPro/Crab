using UnityEngine;
public class UIEntity : EntityController
{
    private static UIEntity instance;
    public static UIEntity Instance => instance;
    public UIPlayerStat playerStat;
    public DialogueManager dialogueManager;
    public UIInventory uiInventory;
    public UIFadeScreen uISaveScreen;
    public UIInventoryHotKey uiHotKey;
    public UIDeathScene uiDeathScene;
    public UIShellManager uiUnlockedShell;
    public bool isSomethingOpened = false;

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
        LoadHotKey();
        LoadPlayerDeathScene();
        LoadShellUnlockedUI();

        this.LoadSingleton();
    }
    protected virtual void LoadHotKey()
    {
        if (this.uiHotKey != null) return;
        uiHotKey = GetComponent<UIInventoryHotKey>();
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
    protected virtual void LoadPlayerDeathScene()
    {
        if (this.uiDeathScene != null) return;
        uiDeathScene = GetComponent<UIDeathScene>();
    }
    protected virtual void LoadShellUnlockedUI()
    {
        if (this.uiUnlockedShell != null) return;
        uiUnlockedShell = GetComponent<UIShellManager>();
    }
    public void Reload()
    {
        playerStat.Reload();
        // uiInventory.Reload();
        uiHotKey.Reload();
    }
}