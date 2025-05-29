using DG.Tweening;
using UnityEngine;
public class StartMenu : MyMonobehaviour
{
    public static StartMenu Instance;
    public CanvasGroup cv;
    protected override void LoadComponents()
    {
        base.LoadComponents();
        LoadSingleton();
    }
    protected virtual void LoadSingleton()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            if (Instance != this)
            {
                Destroy(gameObject);
            }
        }
        DontDestroyOnLoad(gameObject);
    }
    public void Activate(bool activeValue)
    {
        cv.DOFade(activeValue ? 1 : 0, 1f);
        cv.interactable = activeValue;
        cv.blocksRaycasts = activeValue;
    }
}