using UnityEngine;

[System.Serializable]
public struct MusicTrack
{
    public string trackName;
    public AudioClip clip;
}
public class MusicLibrary : MyMonobehaviour
{
    [SerializeField] MusicTrack[] tracks;

    public AudioClip GetTrackFromName(string name)
    {
        foreach (var track in tracks)
        {
            if (track.trackName == name)
            {
                return track.clip;
            }
        }
        return null;
    }

    protected override void LoadComponents()
    {
        base.LoadComponents();
        // Additional initialization if needed
    }
}