using System.Collections;
using UnityEngine;
using UnityEngine.UI;
public class MusicManager : MyMonobehaviour
{
    public static MusicManager Instance;
    [SerializeField] MusicLibrary musicLibrary;
    [SerializeField] AudioSource musicSource;
    [SerializeField] Slider sliderVolume;
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
    IEnumerator AnimateMusicCrossFade(AudioClip nextTrack, float fadeDuration = 0.5f)
    {
        float percent = 0f;
        while (percent < 1f)
        {
            percent += Time.deltaTime / fadeDuration;
            musicSource.volume = Mathf.Lerp(1f, 0f, percent);
            yield return null;
        }
        musicSource.clip = nextTrack;
        musicSource.Play();

        percent = 0f;
        while (percent < 1f)
        {
            percent += Time.deltaTime / fadeDuration;
            musicSource.volume = Mathf.Lerp(0, 1f, percent);
            yield return null;
        }
    }
    public void PlayMusic(string trackName, float fadeDuration = 0.5f)
    {
        AudioClip nextTrack = musicLibrary.GetTrackFromName(trackName);
        if (nextTrack != null)
        {
            StartCoroutine(AnimateMusicCrossFade(nextTrack, fadeDuration));
        }
    }

    public void SetVolume()
    {
        if (sliderVolume != null)
        {
            musicSource.volume = sliderVolume.value;
        }
    }
    public void SetSlider(Slider slider)
    {
        sliderVolume = slider;
        SetVolume();
    }
    public float GetVolume()
    {
        return musicSource.volume;

    }
}