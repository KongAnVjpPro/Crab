using UnityEngine;
using UnityEngine.UI;
public class SoundManager : MyMonobehaviour
{
    public static SoundManager Instance;
    [SerializeField] SoundLibrary sfxLibrary;
    [SerializeField] AudioSource sfx2DSource;
    [SerializeField] Slider sliderVolume;
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
    protected override void LoadComponents()
    {
        base.LoadComponents();
        LoadSingleton();
    }
    public void PlaySound3D(AudioClip clip, Vector3 pos)
    {
        if (clip != null)
        {
            AudioSource.PlayClipAtPoint(clip, pos);
        }
    }
    public void PlaySound3D(string soundName, Vector3 pos)
    {
        PlaySound3D(sfxLibrary.GetClipFromName(soundName), pos);
    }
    public void PlaySound2D(string soundName)
    {
        sfx2DSource.PlayOneShot(sfxLibrary.GetClipFromName(soundName));
    }
    public void SetVolume()
    {
        if (sliderVolume != null)
        {
            sfx2DSource.volume = sliderVolume.value;
        }
    }
    public void SetSlider(Slider slider)
    {
        sliderVolume = slider;
        SetVolume();
    }
    public float GetVolume()
    {
        return sfx2DSource.volume;
    }
}