using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MyMonobehaviour
{
    public string transitionedFromScene;//store prev scene
    public Vector2 platformingRespawnPoint;
    public Vector2 respawnPoint;
    [SerializeField] CheckPoint checkPoint;
    private static GameManager instance;
    public static GameManager Instance => instance;
    public GameObject shade;
    protected override void LoadComponents()
    {
        base.LoadComponents();

    }
    protected virtual void LoadSingleton()
    {
        // Debug.Log("l");
        SaveData.Instance.Initialize();

        if (instance != null && instance != this)
        {
            Destroy(gameObject);
        }
        else if (instance == null)
        {
            instance = this;
        }
        SaveScene();
        DontDestroyOnLoad(gameObject);
        checkPoint = GameObject.Find("CheckPoint").GetComponent<CheckPoint>();
    }
    protected override void Awake()
    {
        base.Awake();

        this.LoadSingleton();
        if (PlayerController.Instance != null)
        {
            if (PlayerController.Instance.halfMana)
            {
                SaveData.Instance.LoadShadeData();
                UIManager.Instance.SwitchMana(UIManager.ManaState.HalfMana);

                if (SaveData.Instance.sceneWithShade == SceneManager.GetActiveScene().name || SaveData.Instance.sceneWithShade == "")
                {
                    Instantiate(shade, SaveData.Instance.shadePos, SaveData.Instance.shadeRotation);
                }
            }
        }

    }

    public void RespawnPlayer()
    {

        SaveData.Instance.LoadCheckPoint();
        if (SaveData.Instance.checkPointName != null)
        {
            if (SaveData.Instance.checkPointName != SceneManager.GetActiveScene().name)
                SceneManager.LoadScene(SaveData.Instance.checkPointName);
        }
        if (SaveData.Instance.checkPointPosition != null)
        {
            respawnPoint = SaveData.Instance.checkPointPosition;
        }
        else
        {
            respawnPoint = platformingRespawnPoint;
        }

        PlayerController.Instance.transform.position = respawnPoint;
        StartCoroutine(UIManager.Instance.DeactivateDeathScreen());
        PlayerController.Instance.Respawned();
    }
    public void SaveScene()
    {
        string currentSceneName = SceneManager.GetActiveScene().name;
        Debug.Log(currentSceneName);
        SaveData.Instance.sceneNames.Add(currentSceneName);
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            SaveData.Instance.SavePlayerData();
        }
    }
}
