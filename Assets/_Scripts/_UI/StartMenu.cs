using DG.Tweening;
using TMPro;
using TMPro.EditorUtilities;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
public class StartMenu : MyMonobehaviour
{
    public static StartMenu Instance;
    public CanvasGroup cv;
    public CanvasGroup mainMenuGroup;
    [Header("Options: ")]
    public CanvasGroup optionsGroup;
    public Slider bgmSlider;
    public Slider sfxSlider;
    public AudioMixer audioMixer;
    public Toggle toggleFullScreen;
    public TMP_Dropdown qualityDropdown;
    public CanvasGroup howToPlayGroup;

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
    bool init = false;
    public void LoadSettings()
    {

        bgmSlider.value = PlayerPrefs.GetFloat("BGM");
        sfxSlider.value = PlayerPrefs.GetFloat("SFX");


    }
    public void SaveSettings()
    {
        PlayerPrefs.SetFloat("BGM", bgmSlider.value);
        PlayerPrefs.SetFloat("SFX", sfxSlider.value);
        PlayerPrefs.Save();

    }
    public void SetSettings()
    {

        SaveSettings();
    }
    #region UI Events
    public void OnClickOptions()
    {
        LoadSettings();
        // optionsGroup.DOFade(1f, 0.5f);
        optionsGroup.interactable = true;
        optionsGroup.blocksRaycasts = true;
        // cv.DOFade(0f, 0.5f);
        Sequence s = DOTween.Sequence();
        s.Join(mainMenuGroup.DOFade(0f, 0.5f));
        s.Join(optionsGroup.DOFade(1f, 0.5f));
    }
    public void OnClickBack()
    {
        // optionsGroup.DOFade(0f, 0.5f);
        optionsGroup.interactable = false;
        optionsGroup.blocksRaycasts = false;
        // cv.DOFade(1f, 0.5f);
        Sequence s = DOTween.Sequence();
        s.Join(mainMenuGroup.DOFade(1f, 0.5f));
        s.Join(optionsGroup.DOFade(0f, 0.5f));
    }
    public void SetQuality(int qualityIndex)
    {
        QualitySettings.SetQualityLevel(qualityIndex);
        PlayerPrefs.SetInt("Quality", qualityIndex);
        PlayerPrefs.Save();
    }
    public void SetBGMVolume()
    {
        float volume = bgmSlider.value;
        audioMixer.SetFloat("BGM", Mathf.Log10(volume) * 20);

    }
    public void SetSFXVolume()
    {
        float volume = sfxSlider.value;
        audioMixer.SetFloat("SFX", Mathf.Log10(volume) * 20);
    }
    public void SetFullScreen(bool isFullScreen)
    {
        Screen.fullScreen = isFullScreen;
        PlayerPrefs.SetInt("FullScreen", isFullScreen ? 1 : 0);
        PlayerPrefs.Save();
    }
    #endregion
    protected override void Awake()
    {
        base.Awake();
        Screen.fullScreen = PlayerPrefs.GetInt("FullScreen", 1) == 1;
        int qualityIndex = PlayerPrefs.GetInt("Quality", 2);
        QualitySettings.SetQualityLevel(qualityIndex);


        toggleFullScreen.isOn = Screen.fullScreen;
        qualityDropdown.value = qualityIndex;
        bgmSlider.value = PlayerPrefs.GetFloat("BGM", 1f);
        sfxSlider.value = PlayerPrefs.GetFloat("SFX", 1f);
        audioMixer.SetFloat("BGM", Mathf.Log10(bgmSlider.value) * 20);
        audioMixer.SetFloat("SFX", Mathf.Log10(sfxSlider.value) * 20);

        Debug.Log("StartMenu Awake called. FullScreen: " + Screen.fullScreen + ", Quality: " + qualityIndex + ", BGM: " + bgmSlider.value + ", SFX: " + sfxSlider.value);




    }
    public void OnClickHowToPlay()
    {
        howToPlayGroup.DOFade(1f, 0.5f);
        howToPlayGroup.interactable = true;
        howToPlayGroup.blocksRaycasts = true;
        // mainMenuGroup.DOFade(0f, 0.5f);
        // optionsGroup.DOFade(0f, 0.5f);
        // optionsGroup.alpha = 0f;
    }
    public void OnClickHowToPlayBack()
    {
        howToPlayGroup.DOFade(0f, 0.5f).OnComplete(() =>
        {
            howToPlayGroup.interactable = false;
            howToPlayGroup.blocksRaycasts = false;
        });

        // mainMenuGroup.DOFade(1f, 0.5f);
        // optionsGroup.DOFade(1f, 0.5f);
        // optionsGroup.alpha = 1f;
    }
}