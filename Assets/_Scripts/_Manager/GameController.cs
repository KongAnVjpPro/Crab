using System;
using System.Collections;
using DG.Tweening;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
public class GameController : MyMonobehaviour
{
    private static GameController instance;
    public static GameController Instance => instance;
    public bool isBlockPlayerControl = false;
    public ItemSpawner itemSpawner;
    public Transform effectHolder;
    public bool isGameStarted = false;
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

        // SaveSystem.Instance.Initialize();


        this.LoadSingleton();
        StartCoroutine(WaitForSaveSystem());
        // this.LoadPlayerEntity();
        this.LoadItemSpawner();
        SaveScene();
        // this.LoadAbilityManager();

    }
    IEnumerator WaitForSaveSystem()
    {
        while (SaveSystem.Instance == null)
        {
            yield return null;
        }
        SaveSystem.Instance.Initialize();
    }
    // void LoadD
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
    void Start()
    {


    }
    #region  main menu
    public void StartGame()
    {
        StartCoroutine(StartAnimation());
        MusicManager.Instance.PlayMusic("SeaWeed", 5f);
    }
    public void OnClickQuitGame()
    {
        // UIEntity.Instance.QuitGameAnimationHandle();
        // Application.Quit();
        // StartCoroutine(QuitGameAnimationHandle());
        StartCoroutine(QuitAnimation());
    }
    [SerializeField] CanvasGroup quitCanvas;
    [SerializeField] CanvasGroup startCanvas;
    [SerializeField] float quitTime = 5f;
    [SerializeField] float fadeTime = 3f;
    void QuitGameAnimationHandle()
    {
        // StartCoroutine(QuitAnimation)
        quitCanvas.blocksRaycasts = true;
        quitCanvas.interactable = true;
        quitCanvas.DOFade(1, quitTime).OnComplete(() =>
        {

            Application.Quit();
        });
    }
    IEnumerator QuitAnimation()
    {
        quitCanvas.DOFade(1, fadeTime);
        quitCanvas.blocksRaycasts = true;
        yield return new WaitForSeconds(quitTime);

        Application.Quit();

    }
    IEnumerator StartAnimation()
    {
        startCanvas.DOFade(1, fadeTime);
        startCanvas.blocksRaycasts = true;
        yield return new WaitForSeconds(quitTime);
        if (!isGameStarted)
        {
            isGameStarted = true;
            // LoadGame();

        }
        else
        {
            SaveSystem.Instance.LoadPlayerData();
            StartMenu.Instance.Activate(false);
            yield break;
        }
        SaveSystem.Instance.LoadNPCAppear();
        SaveSystem.Instance.LoadBossDefeated();

        SaveSystem.Instance.LoadPlayerData();
        SaveSystem.Instance.LoadDoorData();
        SaveSystem.Instance.LoadShellStation();
        SaveSystem.Instance.LoadShellAcient();
        SaveSystem.Instance.LoadShellOwnedData();


        LoadPlayerData();
        StartMenu.Instance.Activate(false);

    }
    public void DeactiveStartCanvas()
    {
        startCanvas.DOFade(0, 2f);
        startCanvas.blocksRaycasts = false;
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
    #endregion
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
        SaveSystem.Instance.LoadShellStation();
        if (SaveSystem.Instance.shellSceneName != null)
        {
            // SceneManager.LoadScene(SaveSystem.Instance.shellSceneName);
            PlayerEntity.Instance.rb.gravityScale = 0;
            LevelManager.Instance.LoadScene(SaveSystem.Instance.shellSceneName, "WaveFade");
        }
        if (SaveSystem.Instance.shellStationPos != null)
        {
            respawnPoint = SaveSystem.Instance.shellStationPos;
        }
        PlayerEntity.Instance.playerStat.RespawnPlayer();
        if (respawnPoint != null) PlayerEntity.Instance.transform.position = respawnPoint;
    }
    #endregion
    #region  save
    public void SaveScene()//for map
    {
        string currentSceneName = SceneManager.GetActiveScene().name;

    }
    #endregion

    #region LoadPlayerData

    void LoadPlayerData()
    {
        //shell owned
        PlayerEntity.Instance.playerShell.GetData();
        // PlayerEntity.Instance.playerShell.GetData();
    }
    #endregion
    #region Settings
    // public void OnClickSettings()
    // {
    //     UIEntity.Instance.uiSettings.ShowSettings();
    // }
    #endregion
}