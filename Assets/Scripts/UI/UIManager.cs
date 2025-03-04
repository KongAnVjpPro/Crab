using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

public class UIManager : MyMonobehaviour
{
    private static UIManager instance;
    public static UIManager Instance => instance;
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
    public SceneFader sceneFader;
    protected virtual void LoadSceneFader()
    {
        this.sceneFader = GetComponentInChildren<SceneFader>();
    }
}
