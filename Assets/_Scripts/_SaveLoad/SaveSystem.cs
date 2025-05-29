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


    //boss defeated
    public bool isSeawWeedDefeated = false;
    public bool isJellyFishDefeated = false;
    public bool isCrabDefeated = false;
    public bool isFinalBossDefeated = false;





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
        if (!File.Exists(Application.persistentDataPath + "/save.boss.data"))
        {
            BinaryWriter writer = new BinaryWriter(File.Create(Application.persistentDataPath + "/save.boss.data"));
        }
    }

    public void ClearData()
    {
        string[] files = {
        "/save.shell.data",
        "/save.playerStat.data",
        "/save.npcAppear.data",
        "/save.boss.data"
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
        playerPosition = new Vector2(-43.41f, -3.77f);
        lastScene = "Cave_Tutorial";

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
                PlayerEntity.Instance.rb.gravityScale = 3;
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
                PlayerEntity.Instance.rb.gravityScale = 3;
            }
        }
        else
        {
            Debug.Log("File doesnt exist");
            SceneManager.LoadScene(lastScene);
            GameController.Instance.DeactiveStartCanvas();
            UIEntity.Instance.ActivateCanvas(true);
            PlayerEntity.Instance.transform.position = playerPosition;
            PlayerEntity.Instance.rb.gravityScale = 3;
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
    #region boss defeated
    public void SaveBossDefeated()
    {
        using (BinaryWriter writer = new BinaryWriter(File.OpenWrite(Application.persistentDataPath + "/save.boss.data")))
        {
            writer.Write(isSeawWeedDefeated);
            writer.Write(isJellyFishDefeated);
            writer.Write(isCrabDefeated);
            writer.Write(isFinalBossDefeated);
        }
    }
    public void LoadBossDefeated()
    {
        if (File.Exists(Application.persistentDataPath + "/save.boss.data"))
        {
            FileInfo fileInfo = new FileInfo(Application.persistentDataPath + "/save.boss.data");
            if (fileInfo.Length == 0)
            {

                return;
            }
            using (BinaryReader reader = new BinaryReader(File.OpenRead(Application.persistentDataPath + "/save.boss.data")))
            {
                isSeawWeedDefeated = reader.ReadBoolean();
                isJellyFishDefeated = reader.ReadBoolean();
                isCrabDefeated = reader.ReadBoolean();
                isFinalBossDefeated = reader.ReadBoolean();
            }
        }
        else
        {
            Debug.LogError("file ko ton tai");
        }
    }
    public bool GetBossDefeated(BossType bossType)
    {
        switch (bossType)
        {
            case BossType.SeaWeed:
                return isSeawWeedDefeated;

            case BossType.JellyFish:
                return isJellyFishDefeated;

            case BossType.Crab:
                return isCrabDefeated;

            case BossType.Final:
                return isFinalBossDefeated;

        }
        return false;
    }
    public void SetBossDefeated(BossType bossType, bool isDefeated)
    {
        switch (bossType)
        {
            case BossType.SeaWeed:
                isSeawWeedDefeated = isDefeated;
                break;
            case BossType.JellyFish:
                isJellyFishDefeated = isDefeated;
                break;
            case BossType.Crab:
                isCrabDefeated = isDefeated;
                break;
            case BossType.Final:
                isFinalBossDefeated = isDefeated;
                break;
        }
    }
    #endregion
}
public enum NPNCCanAppear
{
    //bione seaweed
    OldLobster = 0,
}