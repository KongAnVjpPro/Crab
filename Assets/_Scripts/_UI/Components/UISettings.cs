using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;
public class UISetting : UIComponent
{

    [SerializeField] CanvasGroup settingCanvas;
    [SerializeField] CanvasGroup mainButtonsCanvas;
    [SerializeField] CanvasGroup quitWarningCanvas;
    [SerializeField] CanvasGroup settingsCanvas;
    [SerializeField] float fadeDuration = 1f;
    [SerializeField] bool isOpen = false;
    Sequence s;
    [SerializeField] float coolDownOpen = 0.5f;
    [SerializeField] float coolDownTimer = 0;
    public void FadeIn(CanvasGroup cv)
    {
        UIController.CloseAllUI();

        if (s != null && s.active)
        {
            s.Kill();
        }
        isOpen = true;
        s = DOTween.Sequence();
        cv.DOFade(1f, fadeDuration);
        cv.interactable = true;
        cv.blocksRaycasts = true;
    }
    public void CloseSettings()
    {
        GameController.Instance.isBlockPlayerControl = false;
        UIController.isSomethingOpened = false;
        FadeOut(settingCanvas);

    }
    public void OpenSettings()
    {
        GameController.Instance.isBlockPlayerControl = true;
        UIController.isSomethingOpened = true;
        FadeIn(settingCanvas);
    }
    public void FadeOut(CanvasGroup cv)
    {


        if (s != null && s.active)
        {
            s.Kill();
        }
        isOpen = false;
        s = DOTween.Sequence();
        cv.DOFade(0f, fadeDuration);
        cv.interactable = false;
        cv.blocksRaycasts = false;
    }
    void Update()
    {
        coolDownTimer += Time.deltaTime;
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (!isOpen && coolDownTimer >= coolDownOpen)
            {
                // FadeIn(settingsCanvas);
                OpenSettings();
                coolDownTimer = 0;
            }
            else
            {
                // FadeOut(settingCanvas);
                CloseSettings();
            }
        }
    }
    public void OnClickQuit()
    {
        LevelManager.Instance.LoadScene("MenuStart", "WaveFade");
        UIEntity.Instance.ActivateCanvas(false);
        // SceneManager.LoadScene("MenuStart");
    }
}