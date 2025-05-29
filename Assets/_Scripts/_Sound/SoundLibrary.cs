using UnityEngine;
[System.Serializable]
public struct SoundEffect
{
    public string groupID;
    public AudioClip[] clips;
}
public class SoundLibrary : MyMonobehaviour
{
    public SoundEffect[] soundEffects;
    public AudioClip GetClipFromName(string name)
    {
        foreach (var SoundEffect in soundEffects)
        {
            if (SoundEffect.groupID == name)
            {
                return SoundEffect.clips[Random.Range(0, SoundEffect.clips.Length)];
            }
        }
        return null;
    }
}