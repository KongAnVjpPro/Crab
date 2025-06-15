using UnityEngine;
using System.Collections.Generic;
using System.IO;
using UnityEngine.SceneManagement;
using UnityEngine.Experimental.GlobalIllumination;
using UnityEngine.Events;
using System;

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
        LoadItemDict();

    }
    void LoadItemDict()
    {
        ItemSO[] effs = Resources.LoadAll<ItemSO>("ScriptableObject/");
        foreach (var eff in effs)
        {
            itemDict.Add(eff.itemName, eff);
            Debug.Log("1");
        }
    }
    //map




    //shell station // check point
    public string shellSceneName;
    public Vector2 shellStationPos;

    //shell ancient
    Dictionary<string, bool> shellAncientData = new Dictionary<string, bool>();

    //player
    public float playerTotalHP = 5;
    public float playerTotalMana = 5;
    public float playerTotalStamina = 5;
    public Vector2 playerPosition;
    public string lastScene = "Cave_Tutorial";


    //NPC Appear
    public bool canOldLobsterAppear = true;
    Dictionary<string, bool> npcAppearData = new Dictionary<string, bool>();


    //boss defeated
    public bool isSeawWeedDefeated = false;
    public bool isJellyFishDefeated = false;
    public bool isCrabDefeated = false;
    public bool isFinalBossDefeated = false;

    //map data
    Dictionary<string, bool> doorData = new Dictionary<string, bool>();
    public Action<string, bool> OnDoorDataChanged;

    //shell owned data

    public List<string> shellSaveKey = new List<string>();
    public string lastShellEquippedKey = "";


    //inventory and coin
    public int coinAmount = 0;
    public List<ItemData> itemData = new List<ItemData>();
    public Dictionary<string, ItemSO> itemDict = new Dictionary<string, ItemSO>();


    //player skill

    public bool isTideBurstUnlocked = false;
    public bool isCrushingWaveUnlocked = false;
    public bool isAbyssalPulseUnlocked = false;


    public void Initialize()
    {
        if (!File.Exists(Application.persistentDataPath + "/save.shell.data"))//check point
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
        if (!File.Exists(Application.persistentDataPath + "/save.door.data"))
        {
            BinaryWriter writer = new BinaryWriter(File.Create(Application.persistentDataPath + "/save.door.data"));
        }
        if (!File.Exists(Application.persistentDataPath + "/save.shellAncient.data")) // ancient shell
        {
            BinaryWriter writer = new BinaryWriter(File.Create(Application.persistentDataPath + "/save.shellAncient.data"));
        }
        if (!File.Exists(Application.persistentDataPath + "/save.shellOwned.data")) // shell to equip
        {
            BinaryWriter writer = new BinaryWriter(File.Create(Application.persistentDataPath + "/save.shellOwned.data"));
        }
        if (!File.Exists(Application.persistentDataPath + "/save.inventory.data")) // invent
        {
            BinaryWriter writer = new BinaryWriter(File.Create(Application.persistentDataPath + "/save.inventory.data"));
        }

    }

    public void ClearData()
    {
        string[] files = {
        "/save.shell.data",
        "/save.playerStat.data",
        "/save.npcAppear.data",
        "/save.boss.data",
        "/save.door.data",
        "/save.shellAncient.data",
        "/save.shellOwned.data",
        "/save.inventory.data"
    };

        foreach (string file in files)
        {
            string path = Application.persistentDataPath + file;
            if (File.Exists(path))
            {
                File.Delete(path);
                Debug.Log($"File {file} cleared.");
            }
        }
        GameController.Instance.isGameStarted = false;

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
    public void SaveGlobalData()
    {
        SaveShellStation();
        SavePlayerData();
        SaveNPCAppear();
        SaveDoorData();
        SaveBossDefeated();
        SaveShellAcient();
        SaveShellOwnedData();
    }
    #region Shell Owned Data
    public List<string> GetShellSaveKey()
    {
        return shellSaveKey;
    }
    public void SetShellSaveKey(List<string> keys)
    {
        shellSaveKey = keys;
    }
    public string GetLastShellEquippedKey()
    {
        return lastShellEquippedKey;
    }
    public void SetLastShellEquippedKey(string key)
    {
        lastShellEquippedKey = key;
    }
    public void SaveShellOwnedData()
    {
        shellSaveKey = PlayerEntity.Instance.playerShell.shellSaveKey;
        lastShellEquippedKey = PlayerEntity.Instance.playerShell.lastShellEquippedKey;
        using (BinaryWriter writer = new BinaryWriter(File.OpenWrite(Application.persistentDataPath + "/save.shellOwned.data")))
        {
            writer.Write(shellSaveKey.Count);
            foreach (var key in shellSaveKey)
            {
                writer.Write(key);
            }
            writer.Write(lastShellEquippedKey);
        }
    }

    public void LoadShellOwnedData()
    {
        if (File.Exists(Application.persistentDataPath + "/save.shellOwned.data"))
        {
            FileInfo fileInfo = new FileInfo(Application.persistentDataPath + "/save.shellOwned.data");
            if (fileInfo.Length == 0)
            {

                return;
            }
            using (BinaryReader reader = new BinaryReader(File.OpenRead(Application.persistentDataPath + "/save.shellOwned.data")))
            {
                int count = reader.ReadInt32();
                // shellSaveKey.Clear();
                for (int i = 0; i < count; i++)
                {
                    string key = reader.ReadString();
                    shellSaveKey.Add(key);
                }
                lastShellEquippedKey = reader.ReadString();
            }
        }
        else
        {
            Debug.LogError("file ko ton tai");
        }
    }
    #endregion
    #region Ancient Shell Station

    public void SaveShellAcient()
    {
        using (BinaryWriter writer = new BinaryWriter(File.OpenWrite(Application.persistentDataPath + "/save.shellAncient.data")))
        {
            writer.Write(shellAncientData.Count);
            foreach (var pair in shellAncientData)
            {
                writer.Write(pair.Key);
                writer.Write(pair.Value);
            }
        }
    }
    public void LoadShellAcient()
    {
        if (File.Exists(Application.persistentDataPath + "/save.shellAncient.data"))
        {
            FileInfo fileInfo = new FileInfo(Application.persistentDataPath + "/save.shellAncient.data");
            if (fileInfo.Length == 0)
            {

                return;
            }
            using (BinaryReader reader = new BinaryReader(File.OpenRead(Application.persistentDataPath + "/save.shellAncient.data")))
            {
                int count = reader.ReadInt32();
                for (int i = 0; i < count; i++)
                {
                    string key = reader.ReadString();
                    bool value = reader.ReadBoolean();
                    shellAncientData[key] = value;
                }
            }
        }
        else
        {
            Debug.LogError("file ko ton tai");
        }
    }
    public bool GetShellAncientData(string key)
    {
        if (shellAncientData.ContainsKey(key))
        {
            return shellAncientData[key];
        }
        return false;
    }
    public void SetShellAncientData(string key, bool value)
    {
        if (shellAncientData.ContainsKey(key))
        {
            shellAncientData[key] = value;
        }
        else
        {
            shellAncientData.Add(key, value);
        }
    }
    #endregion
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

            isTideBurstUnlocked = PlayerEntity.Instance.pState.unlockedTideBurst;
            writer.Write(isTideBurstUnlocked);
            isCrushingWaveUnlocked = PlayerEntity.Instance.pState.unlockedCurshingWave;
            writer.Write(isCrushingWaveUnlocked);
            isAbyssalPulseUnlocked = PlayerEntity.Instance.pState.unlockedAbyssalPulse;
            writer.Write(isAbyssalPulseUnlocked);

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
                PlayerEntity.Instance.transform.position = playerPosition;
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

                isTideBurstUnlocked = reader.ReadBoolean();
                isCrushingWaveUnlocked = reader.ReadBoolean();
                isAbyssalPulseUnlocked = reader.ReadBoolean();




                // LevelManager.Instance.LoadScene(lastScene, "WaveFade");


                //change stats that ra la cong hoac tru stats
                PlayerEntity.Instance.playerStat.ChangeTotalStats(StatComponent.StatType.Health, -200);
                PlayerEntity.Instance.playerStat.ChangeTotalStats(StatComponent.StatType.Mana, -200);
                PlayerEntity.Instance.playerStat.ChangeTotalStats(StatComponent.StatType.Stamina, -200);

                PlayerEntity.Instance.playerStat.ChangeTotalStats(StatComponent.StatType.Health, playerTotalHP);
                PlayerEntity.Instance.playerStat.ChangeTotalStats(StatComponent.StatType.Mana, playerTotalMana);
                PlayerEntity.Instance.playerStat.ChangeTotalStats(StatComponent.StatType.Stamina, playerTotalStamina);
                PlayerEntity.Instance.playerStat.RestoreStat();
                PlayerEntity.Instance.transform.position = playerPosition;

                PlayerEntity.Instance.pState.unlockedTideBurst = isTideBurstUnlocked;
                PlayerEntity.Instance.pState.unlockedCurshingWave = isCrushingWaveUnlocked;
                PlayerEntity.Instance.pState.unlockedAbyssalPulse = isAbyssalPulseUnlocked;

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
    #region  Inventory
    public void SaveInventory()
    {
        // if (!File.Exists(Application.persistentDataPath + "/save.inventory.data")) // invent
        // {
        //     BinaryWriter writer = new BinaryWriter(File.Create(Application.persistentDataPath + "/save.inventory.data"));
        // }
        using (BinaryWriter writer = new BinaryWriter(File.OpenWrite(Application.persistentDataPath + "/save.inventory.data")))
        {
            // writer.Write(isSeawWeedDefeated);
            // writer.Write(isJellyFishDefeated);
            // writer.Write(isCrabDefeated);
            // writer.Write(isFinalBossDefeated);
            itemData = PlayerEntity.Instance.playerInventory.inventory;
            coinAmount = PlayerEntity.Instance.playerInventory.GetCurrentCoin();
            writer.Write(coinAmount);
            writer.Write(itemData.GetItems().Count);
            foreach (ItemData item in itemData)
            {
                writer.Write(item.itemSO.itemName);
                writer.
            }

        }

    }
    public void LoadInventory()
    {

    }

    #endregion

    #region npc



    public bool GetNPCAppear(string npcKey)
    {
        // switch (npcEnum)
        // {
        //     case NPNCCanAppear.OldLobster:
        //         return canOldLobsterAppear;
        // }
        // return true;
        if (npcAppearData.ContainsKey(npcKey))
        {
            return npcAppearData[npcKey];
        }
        else
        {
            // Default value if the NPC key does not exist
            return true;
        }

    }
    public void SetNPCAppear(string npcKey, bool val)
    {
        // switch (npcEnum)
        // {
        //     case NPNCCanAppear.OldLobster:
        //         canOldLobsterAppear = val;
        //         break;
        // }
        if (npcAppearData.ContainsKey(npcKey))
        {
            npcAppearData[npcKey] = val;
        }
        else
        {
            npcAppearData.Add(npcKey, val);
        }

    }

    public void SaveNPCAppear()
    {
        using (BinaryWriter writer = new BinaryWriter(File.OpenWrite(Application.persistentDataPath + "/save.npcAppear.data")))
        {
            // writer.Write(canOldLobsterAppear);
            writer.Write(npcAppearData.Count);
            foreach (var pair in npcAppearData)
            {
                writer.Write(pair.Key);
                writer.Write(pair.Value);
            }
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
                // canOldLobsterAppear = reader.ReadBoolean();
                int npcCount = reader.ReadInt32();
                for (int i = 0; i < npcCount; i++)
                {
                    string npcKey = reader.ReadString();
                    bool npcValue = reader.ReadBoolean();
                    npcAppearData.Add(npcKey, npcValue);
                }
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
    #region Door
    public bool GetDoorState(string doorKey)
    {
        if (doorData.ContainsKey(doorKey))
        {
            return doorData[doorKey];
        }
        return false;
    }
    public void SetDoorState(string doorKey, bool isOpened)
    {
        if (doorData.ContainsKey(doorKey))
        {
            doorData[doorKey] = isOpened;
        }
        else
        {
            doorData.Add(doorKey, isOpened);
        }
        OnDoorDataChanged?.Invoke(doorKey, isOpened);
    }
    public void LoadDoorData()
    {
        if (File.Exists(Application.persistentDataPath + "/save.door.data"))
        {
            FileInfo fileInfo = new FileInfo(Application.persistentDataPath + "/save.door.data");
            if (fileInfo.Length == 0)
            {

                return;
            }
            using (BinaryReader reader = new BinaryReader(File.OpenRead(Application.persistentDataPath + "/save.door.data")))
            {
                // isSeawWeedDefeated = reader.ReadBoolean();
                // isJellyFishDefeated = reader.ReadBoolean();
                // isCrabDefeated = reader.ReadBoolean();
                // isFinalBossDefeated = reader.ReadBoolean();
                int doorCount = reader.ReadInt32();
                for (int i = 0; i < doorCount; i++)
                {
                    string doorKey = reader.ReadString();
                    bool doorValue = reader.ReadBoolean();
                    doorData.Add(doorKey, doorValue);
                }

            }
        }
        else
        {
            Debug.LogError("file ko ton tai");
        }
    }
    public void SaveDoorData()
    {
        using (BinaryWriter writer = new BinaryWriter(File.OpenWrite(Application.persistentDataPath + "/save.door.data")))
        {
            // writer.Write(isSeawWeedDefeated);
            // writer.Write(isJellyFishDefeated);
            // writer.Write(isCrabDefeated);
            // writer.Write(isFinalBossDefeated);
            writer.Write(doorData.Count);
            foreach (var pair in doorData)
            {
                writer.Write(pair.Key);
                writer.Write(pair.Value);
            }
        }
    }
    #endregion

}
// public enum NPNCCanAppear
// {
//     //bione seaweed
//     OldLobster = 0,
// }