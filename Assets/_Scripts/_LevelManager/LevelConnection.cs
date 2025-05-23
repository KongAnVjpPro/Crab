using UnityEngine;
[CreateAssetMenu(fileName = "", menuName = "Levels/Connection")]
public class LevelConnection : ScriptableObject
{
    public static LevelConnection ActiveConnection { get; set; }
}