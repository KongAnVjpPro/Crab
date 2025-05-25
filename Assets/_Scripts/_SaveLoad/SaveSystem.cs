using UnityEngine;
using System.Collections.Generic;
using System.IO;
using UnityEngine.SceneManagement;
using UnityEngine.Experimental.GlobalIllumination;

[System.Serializable]
public class SaveSystem : MyMonobehaviour
{
    public static SaveSystem Instance;


    protected override void Awake()
    {
        base.Awake();
        if (Instance == null) Instance = this;
        else
        {
            if (Instance != this)
            {
                Destroy(gameObject);
            }
        }
        DontDestroyOnLoad(gameObject);

    }
    //map




    //shell station
    public string shellSceneName;
    public Vector2 shellStationPos;

    //player
    public float playerTotalHP = 5;
    public float playerTotalMana = 5;
    public float playerTotalStamina = 5;
    public Vector2 playerPosition;
    public string lastScene = "Cave_Tutorial";


    //NPC Appear
    public bool canOldLobsterAppear = true;






    public void Initialize()
    {
        if (!File.Exists(Application.persistentDataPath + "/save.shell.data"))
        {
            BinaryWriter writer = new BinaryWriter(File.Create(Application.persistentDataPath + "/save.shell.data"));
        }

        if (!File.Exists(Application.persistentDataPath + "/save.playerStat.data"))
        {
            BinaryWriter writer = new BinaryWriter(File.Create(Application.persistentDataPath + "/save.playerStat.data"));
        }
        if (!File.Exists(Application.persistentDataPath + "/save.npcAppear.data"))
        {
            BinaryWriter writer = new BinaryWriter(File.Create(Application.persistentDataPath + "/save.npcAppear.data"));
        }
    }

    public void ClearData()
    {
        string[] files = {
        "/save.shell.data",
        "/save.playerStat.data",
        "/save.npcAppear.data"
    };

        foreach (string file in files)
        {
            string path = Application.persistentDataPath + file;
            if (File.Exists(path))
            {
                File.Delete(path);
            }
        }

        //reset field data
        // shellSceneName = "";
        // shellStationPos = Vector2.zero;

        // playerTotalHP = 0;
        // playerTotalMana = 0;
        // playerTotalStamina = 0;
        // playerPosition = Vector2.zero;
        // lastScene = "";

        // canOldLobsterAppear = false;

        Debug.Log("Save data cleared.");
    }
    #region check point data
    public void SaveShellStation()
    {
        using (BinaryWriter writer = new BinaryWriter(File.OpenWrite(Application.persistentDataPath + "/save.shell.data")))
        {
            writer.Write(shellSceneName);
            writer.Write(shellStationPos.x);
            writer.Write(shellStationPos.y);
        }
    }
    public void LoadShellStation()
    {
        if (File.Exists(Application.persistentDataPath + "/save.shell.data"))
        {
            FileInfo fileInfo = new FileInfo(Application.persistentDataPath + "/save.shell.data");
            if (fileInfo.Length == 0)
            {

                return;
            }
            using (BinaryReader reader = new BinaryReader(File.OpenRead(Application.persistentDataPath + "/save.shell.data")))
            {
                shellSceneName = reader.ReadString();
                shellStationPos.x = reader.ReadSingle();
                shellStationPos.y = reader.ReadSingle();

            }

        }
    }
    #endregion
    #region  player data
    public void SavePlayerData()
    {
        using (BinaryWriter writer = new BinaryWriter(File.OpenWrite(Application.persistentDataPath + "/save.playerStat.data")))
        {
            playerTotalHP = PlayerEntity.Instance.playerStat.TotalHealth;
            writer.Write(playerTotalHP);
            playerTotalMana = PlayerEntity.Instance.playerStat.TotalMana;
            writer.Write(playerTotalMana);
            playerTotalStamina = PlayerEntity.Instance.playerStat.TotalStamina;
            writer.Write(playerTotalStamina);


            playerPosition = PlayerEntity.Instance.transform.position;
            writer.Write(playerPosition.x);
            writer.Write(playerPosition.y);

            lastScene = SceneManager.GetActiveScene().name;
            writer.Write(lastScene);
        }
    }
    public void LoadPlayerData()
    {
        if (File.Exists(Application.persistentDataPath + "/save.playerStat.data"))
        {
            FileInfo fileInfo = new FileInfo(Application.persistentDataPath + "/save.playerStat.data");
            if (fileInfo.Length == 0)
            {


                SceneManager.LoadScene(lastScene);
                GameController.Instance.DeactiveStartCanvas();
                UIEntity.Instance.ActivateCanvas(true);
                // PlayerEntity.Instance.transform.position = playerPosition;
                return;
            }
            using (BinaryReader reader = new BinaryReader(File.OpenRead(Application.persistentDataPath + "/save.playerStat.data")))
            {
                playerTotalHP = reader.ReadSingle();
                playerTotalMana = reader.ReadSingle();
                playerTotalStamina = reader.ReadSingle();

                playerPosition.x = reader.ReadSingle();
                playerPosition.y = reader.ReadSingle();

                lastScene = reader.ReadString();


                // LevelManager.Instance.LoadScene(lastScene, "WaveFade");


                PlayerEntity.Instance.playerStat.ChangeTotalStats(StatComponent.StatType.Health, playerTotalHP);
                PlayerEntity.Instance.playerStat.ChangeTotalStats(StatComponent.StatType.Mana, playerTotalMana);
                PlayerEntity.Instance.playerStat.ChangeTotalStats(StatComponent.StatType.Stamina, playerTotalStamina);
                PlayerEntity.Instance.playerStat.RestoreStat();
                PlayerEntity.Instance.transform.position = playerPosition;

                SceneManager.LoadScene(lastScene);
                GameController.Instance.DeactiveStartCanvas();
                UIEntity.Instance.ActivateCanvas(true);

            }
        }
        else
        {
            Debug.Log("File doesnt exist");
            SceneManager.LoadScene(lastScene);
            GameController.Instance.DeactiveStartCanvas();
            UIEntity.Instance.ActivateCanvas(true);
            // PlayerController
        }
    }
    #endregion

    #region npc



    public bool GetNPCAppear(NPNCCanAppear npcEnum)
    {
        switch (npcEnum)
        {
            case NPNCCanAppear.OldLobster:
                return canOldLobsterAppear;
        }
        return true;
    }
    public void SetNPCAppear(NPNCCanAppear npcEnum, bool val)
    {
        switch (npcEnum)
        {
            case NPNCCanAppear.OldLobster:
                canOldLobsterAppear = val;
                break;
        }
    }

    public void SaveNPCAppear()
    {
        using (BinaryWriter writer = new BinaryWriter(File.OpenWrite(Application.persistentDataPath + "/save.npcAppear.data")))
        {
            writer.Write(canOldLobsterAppear);
        }
    }
    public void LoadNPCAppear()
    {
        if (File.Exists(Application.persistentDataPath + "/save.npcAppear.data"))
        {
            FileInfo fileInfo = new FileInfo(Application.persistentDataPath + "/save.npcAppear.data");
            if (fileInfo.Length == 0)
            {

                return;
            }
            using (BinaryReader reader = new BinaryReader(File.OpenRead(Application.persistentDataPath + "/save.npcAppear.data")))
            {
                canOldLobsterAppear = reader.ReadBoolean();
            }
        }
        else
        {
            Debug.LogError("file ko ton tai");
        }
    }
    #endregion
}
public enum NPNCCanAppear
{
    //bione seaweed
    OldLobster = 0,
}