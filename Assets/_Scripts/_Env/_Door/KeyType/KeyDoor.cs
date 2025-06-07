using UnityEngine;
public class KeyDoor : DoorActivate
{
    // [Header("Key Settings: {Key = ItemSO name}")]
    [SerializeField] protected ItemSO keyItem;
    [SerializeField] string keyItemName = "";
    protected override void Awake()
    {
        base.Awake();
        keyItemName = keyItem.itemName;
        if (keyItem == null)
        {
            Debug.LogError("Key item is not set for the door: " + doorKey);
        }
    }
    public override bool CheckCondition(bool activateValue)
    {
        if (isOpened) return false;
        if (!activateValue) return false;
        if (keyItem == null)
        {
            // Debug.LogWarning("Key item is not set for the door.");
            return false;
        }
        if (!PlayerEntity.Instance.playerInventory.IsContainItem(new ItemData { itemSO = keyItem, amount = 1 }))
        {
            UIEntity.Instance.uiNotification.NoticeSomething(3f, "You need " + keyItemName + " to open this door.", "");
            return false;
        }
        PlayerEntity.Instance.playerInventory.GetItem(keyItem, 1);
        return true;

    }
    public bool playerInZone = false;
    public void SetPlayerInzone(bool value)
    {
        playerInZone = value;
    }
    void Update()
    {
        if (!playerInZone) return;
        if (isOpened) return;
        if (PlayerEntity.Instance.playerInput.interact)
        {
            SetDoorState(true);
        }
    }
}