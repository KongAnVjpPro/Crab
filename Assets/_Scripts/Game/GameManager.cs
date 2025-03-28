using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
        }
        else if (instance == null)
        {
            instance = this;
        }
        DontDestroyOnLoad(gameObject);
        checkPoint = GameObject.Find("CheckPoint").GetComponent<CheckPoint>();
    }
    protected override void Awake()
    {
        base.Awake();
        this.LoadSingleton();
    }
    public void RespawnPlayer()
    {
        if (checkPoint != null)
        {
            if (checkPoint.interacted)
            {
                respawnPoint = checkPoint.transform.position;
            }

        }
        else
        {
            respawnPoint = platformingRespawnPoint;
        }

        PlayerController.Instance.transform.position = respawnPoint;
        StartCoroutine(UIManager.Instance.DeactivateDeathScreen());
        PlayerController.Instance.Respawned();
    }
}
