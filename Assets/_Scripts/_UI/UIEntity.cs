using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
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
    public UINotification uiNotification;
    public UISetting uiSetting;
    public bool isSomethingOpened = false;
    public CanvasGroup mainCanvas;
    public UICinematic uiCinematic;
    public UIBoss uiBoss;
    public UIShellStation uiShellStation;
    public UIIncreaseStatController uiStatUpgrade;


    public CanvasGroup inkBullet;
    // public AudioSource audioSource;
    // public UIAudio uiAudio;
    public void InkedScreen(float duration)
    {
        Tween tweener = inkBullet.DOFade(1, 0.5f).OnComplete(() =>
        {
            DOVirtual.DelayedCall(duration, () =>
            {
                inkBullet.DOFade(0, 0.5f);
            });
        });


    }
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

        LoadMainCanvas();
        this.LoadPlayerStat();
        LoadDialog();
        this.LoadUIInventory();
        LoadSaveScreen();
        LoadHotKey();
        LoadPlayerDeathScene();
        LoadShellUnlockedUI();
        LoadSettingUI();
        LoadUIBoss();
        LoadCinematicUI();
        LoadUIShellStation();
        LoadStatUpgrade();
        // LoadAudio();
        // LoadUIAudio();


        this.LoadSingleton();
    }
    // protected virtual void LoadUIAudio()
    // {
    //     if (uiAudio != null) return;
    //     uiAudio = GetComponent<UIAudio>();
    // }
    // protected virtual void LoadAudio()
    // {
    //     if (audioSource != null) return;
    //     audioSource = GetComponent<AudioSource>();
    // }
    protected virtual void LoadStatUpgrade()
    {
        if (uiStatUpgrade != null) return;
        uiStatUpgrade = GetComponent<UIIncreaseStatController>();
    }
    protected virtual void LoadUIShellStation()
    {
        if (uiShellStation != null) return;
        uiShellStation = GetComponent<UIShellStation>();
    }
    protected virtual void LoadCinematicUI()
    {
        if (this.uiCinematic != null) return;
        this.uiCinematic = GetComponent<UICinematic>();
    }
    protected virtual void LoadUIBoss()
    {
        if (uiBoss != null) return;
        uiBoss = GetComponent<UIBoss>();
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
        if (this.uiNotification != null) return;
        uiNotification = GetComponent<UINotification>();
    }
    protected virtual void LoadSettingUI()
    {
        if (this.uiSetting != null) return;
        this.uiSetting = GetComponent<UISetting>();
    }
    protected virtual void LoadMainCanvas()
    {
        if (mainCanvas != null) return;
        mainCanvas = GetComponent<CanvasGroup>();
    }
    public void Reload()
    {
        playerStat.Reload();
        // uiInventory.Reload();
        uiHotKey.Reload();
    }
    public void CloseAllUI()
    {
        uiInventory.HideInventory();
    }
    public void ActivateCanvas(bool activeValue)
    {
        mainCanvas.interactable = activeValue;
        mainCanvas.blocksRaycasts = activeValue;
        mainCanvas.alpha = activeValue ? 1 : 0;
    }
    #region setting

    #endregion
}