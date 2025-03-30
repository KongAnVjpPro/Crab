using System.IO.MemoryMappedFiles;
using UnityEngine;
public class MapManager : MyMonobehaviour
{
    [SerializeField] GameObject[] maps;
    CheckPoint checkPoint;
    void OnEnable()
    {
        checkPoint = FindObjectOfType<CheckPoint>();
        if (checkPoint != null)
        {
            if (checkPoint.interacted)
            {

                UpdateMap();
            }
        }
    }
    void UpdateMap()
    {
        var savedScenes = SaveData.Instance.sceneNames;
        Debug.Log("map");
        for (int i = 0; i < maps.Length; i++)
        {

            if (savedScenes.Contains("Cave_" + (i)))
            {
                maps[i].SetActive(true);
            }
            else
            {
                maps[i].SetActive(false);
            }
        }
    }
}