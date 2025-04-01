using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.SceneManagement;
using Unity.VisualScripting;
using Unity.Mathematics;


[System.Serializable]
public struct SaveData
{
    public static SaveData Instance;

    //map stuff
    public HashSet<string> sceneNames;
    //checkpoint stuff
    public string checkPointName;
    public Vector2 checkPointPosition;
    //player stuff
    public int playerHealth;
    public float playerMana;
    public bool playerHalfMana;
    public Vector2 playerPosition;
    public string lastScene;

    public bool playerUnlockedWallJump;
    public bool playerUnlockedDash;
    public bool playerUnlockedVarJump;


    //enemy stuff
    //shade
    public Vector2 shadePos;
    public string sceneWithShade;
    public Quaternion shadeRotation;
    public void Initialize()
    {
        if (!File.Exists(Application.persistentDataPath + "/save.checkpoint.data"))
        {
            BinaryWriter writer = new BinaryWriter(File.Create(Application.persistentDataPath + "/save.checkpoint.data"));
        }

        if (!File.Exists(Application.persistentDataPath + "/save.player.data"))
        {
            BinaryWriter writer = new BinaryWriter(File.Create(Application.persistentDataPath + "/save.player.data"));
        }
        if (sceneNames == null)
        {
            sceneNames = new HashSet<string>();

        }
    }
    public void SaveCheckPoint()
    {
        using (BinaryWriter writer = new BinaryWriter(File.OpenWrite(Application.persistentDataPath + "/save.checkpoint.data")))
        {
            writer.Write(checkPointName);
            writer.Write(checkPointPosition.x);
            writer.Write(checkPointPosition.y);
        }
    }
    public void LoadCheckPoint()
    {
        if (File.Exists(Application.persistentDataPath + "/save.checkpoint.data"))
        {
            using (BinaryReader reader = new BinaryReader(File.OpenRead(Application.persistentDataPath + "/save.checkpoint.data")))
            {
                checkPointName = reader.ReadString();
                checkPointPosition.x = reader.ReadSingle();
                checkPointPosition.y = reader.ReadSingle();
            }
        }
    }
    public void SavePlayerData()
    {
        using (BinaryWriter writer = new BinaryWriter(File.OpenWrite(Application.persistentDataPath + "/save.player.data")))
        {
            playerHealth = PlayerController.Instance.Health;
            writer.Write(playerHealth);
            playerMana = PlayerController.Instance.Mana;
            writer.Write(playerMana);
            playerHalfMana = PlayerController.Instance.halfMana;
            writer.Write(playerHalfMana);

            playerUnlockedWallJump = PlayerController.Instance.unlockedWallJump;
            writer.Write(playerUnlockedWallJump);
            playerUnlockedDash = PlayerController.Instance.unlockedDash;
            writer.Write(playerUnlockedDash);
            playerUnlockedVarJump = PlayerController.Instance.unlockedVarJump;
            writer.Write(playerUnlockedVarJump);

            playerPosition = PlayerController.Instance.transform.position;
            writer.Write(playerPosition.x);
            writer.Write(playerPosition.y);

            lastScene = SceneManager.GetActiveScene().name;
            writer.Write(lastScene);
        }
    }
    public void LoadPlayerData()
    {
        if (File.Exists(Application.persistentDataPath + "/save.player.data"))
        {
            using (BinaryReader reader = new BinaryReader(File.OpenRead(Application.persistentDataPath + "/save.player.data")))
            {
                playerHealth = reader.ReadInt32();
                playerMana = reader.ReadSingle();
                playerHalfMana = reader.ReadBoolean();

                playerUnlockedWallJump = reader.ReadBoolean();
                playerUnlockedDash = reader.ReadBoolean();
                playerUnlockedVarJump = reader.ReadBoolean();

                playerPosition.x = reader.ReadSingle();
                playerPosition.y = reader.ReadSingle();

                lastScene = reader.ReadString();

                SceneManager.LoadScene(lastScene);
                PlayerController.Instance.transform.position = playerPosition;
                PlayerController.Instance.halfMana = playerHalfMana;
                PlayerController.Instance.Health = playerHealth;
                PlayerController.Instance.Mana = playerMana;

                PlayerController.Instance.unlockedWallJump = playerUnlockedWallJump;
                PlayerController.Instance.unlockedDash = playerUnlockedDash;
                PlayerController.Instance.unlockedVarJump = playerUnlockedVarJump;
            }
        }
        else
        {
            Debug.Log("file doesnt exist");
            PlayerController.Instance.Health = PlayerController.Instance.maxHealth;
            PlayerController.Instance.halfMana = false;
            PlayerController.Instance.Mana = 0.5f;

            PlayerController.Instance.unlockedWallJump = false;
            PlayerController.Instance.unlockedDash = false;
            PlayerController.Instance.unlockedVarJump = false;
        }

    }
    public void SaveShadeData()
    {
        using (BinaryWriter writer = new BinaryWriter(File.OpenWrite(Application.persistentDataPath + "/save.shade.data")))
        {
            sceneWithShade = SceneManager.GetActiveScene().name;
            shadePos = Shade.Instance.transform.position;
            shadeRotation = Shade.Instance.transform.rotation;

            writer.Write(sceneWithShade);

            writer.Write(shadePos.x);
            writer.Write(shadePos.y);

            writer.Write(shadeRotation.x);
            writer.Write(shadeRotation.y);
            writer.Write(shadeRotation.z);
            writer.Write(shadeRotation.w);
        }
    }
    public void LoadShadeData()
    {
        if (File.Exists(Application.persistentDataPath + "/save.shade.data"))
        {
            using (BinaryReader reader = new BinaryReader(File.OpenRead(Application.persistentDataPath + "/save.shade.data")))
            {
                sceneWithShade = reader.ReadString();
                shadePos.x = reader.ReadSingle();
                shadePos.y = reader.ReadSingle();

                float rotX = reader.ReadSingle();
                float rotY = reader.ReadSingle();
                float rotZ = reader.ReadSingle();
                float rotW = reader.ReadSingle();
                shadeRotation = new Quaternion(rotX, rotY, rotZ, rotW);
            }
        }
        else
        {
            Debug.Log("shade doesnt exist");
        }
    }
}
