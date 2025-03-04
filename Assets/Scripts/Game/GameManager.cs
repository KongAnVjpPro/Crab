using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MyMonobehaviour
{
    public string transitionedFromScene;//store prev scene
    private static GameManager instance;
    public static GameManager Instance => instance;
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
    }
    protected override void Awake()
    {
        base.Awake();
        this.LoadSingleton();
    }
}
