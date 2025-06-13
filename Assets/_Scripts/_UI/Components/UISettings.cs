using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
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
        PlayerEntity.Instance.rb.gravityScale = 0;
        FadeOut(quitWarningCanvas);
        CloseSettings();
        LevelManager.Instance.LoadScene("MenuStart", "WaveFade");
        UIEntity.Instance.ActivateCanvas(false);
        StartMenu.Instance.Activate(true);
        MusicManager.Instance.PlayMusic("MenuStart", 0.5f);




        // SceneManager.LoadScene("MenuStart");
    }
    #region ref to starmenu
    [SerializeField] CanvasGroup STARTCANVAS;
    [SerializeField] CanvasGroup mainMenuGroup;
    [SerializeField] CanvasGroup optionsGroup;
    public void OnClickSettings()
    {
        Sequence s = DOTween.Sequence();
        // s.Join(mainMenuGroup.DOFade(0f, 0.5f));
        mainMenuGroup.alpha = 0f;
        s.Join(optionsGroup.DOFade(1f, 0.5f));
        s.Join(STARTCANVAS.DOFade(1f, 0.5f));

        STARTCANVAS.blocksRaycasts = true;
        STARTCANVAS.interactable = true;
        optionsGroup.interactable = true;
        optionsGroup.blocksRaycasts = true;

        mainMenuGroup.interactable = false;
        mainMenuGroup.blocksRaycasts = false;
        CloseSettings();
        backButtonMenu.gameObject.SetActive(false);
        backButtonOptions.gameObject.SetActive(true);

    }
    [SerializeField] Button backButtonMenu;
    [SerializeField] Button backButtonOptions;
    public void OnClickBack()
    {
        // OpenSettings();

        backButtonMenu.gameObject.SetActive(true);
        backButtonOptions.gameObject.SetActive(false);

        STARTCANVAS.blocksRaycasts = false;
        STARTCANVAS.interactable = false;
        optionsGroup.interactable = false;
        optionsGroup.blocksRaycasts = false;
        mainMenuGroup.interactable = true;
        mainMenuGroup.blocksRaycasts = true;

        Sequence s = DOTween.Sequence();
        // s.Join(mainMenuGroup.DOFade(1f, 0.5f));
        s.Join(optionsGroup.DOFade(0f, 0.5f));
        s.Join(STARTCANVAS.DOFade(0f, 0.5f));
        OpenSettings();
        s.OnComplete(() =>
        {

            mainMenuGroup.alpha = 1f;
        });
    }
    #endregion
}