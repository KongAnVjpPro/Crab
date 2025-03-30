using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

public class UIManager : MyMonobehaviour
{
    private static UIManager instance;
    public static UIManager Instance => instance;
    [SerializeField] GameObject deathScreen;
    public GameObject mapHandler;
    [SerializeField] GameObject halfMana, fullMana;
    public enum ManaState
    {
        FullMana,
        HalfMana
    }
    public ManaState manaState;
    public void SwitchMana(ManaState _manaState)
    {
        switch (_manaState)
        {
            case ManaState.FullMana:
                halfMana.SetActive(false);
                fullMana.SetActive(true);
                break;
            case ManaState.HalfMana:
                halfMana.SetActive(true);
                fullMana.SetActive(false);
                break;
        }
        manaState = _manaState;
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
    public SceneFader sceneFader;
    protected virtual void LoadSceneFader()
    {
        this.sceneFader = GetComponentInChildren<SceneFader>();
    }
    public IEnumerator ActivateDeathScreen()
    {
        yield return new WaitForSeconds(0.8f);
        StartCoroutine(sceneFader.Fade(SceneFader.FadeDirection.In));

        yield return new WaitForSeconds(0.8f);
        deathScreen.SetActive(true);
        // StartCoroutine(sceneFader.Fade(SceneFader.FadeDirection.Out));
    }
    public IEnumerator DeactivateDeathScreen()
    {
        yield return new WaitForSeconds(0.5f);
        deathScreen.SetActive(false);
        StartCoroutine(sceneFader.Fade(SceneFader.FadeDirection.Out));

    }
}
