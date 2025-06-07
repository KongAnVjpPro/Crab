using UnityEngine;
public class MoneyDoor : DoorActivate
{
    [SerializeField] private int moneyCost = 20;

    public override bool CheckCondition(bool activateValue)
    {
        if (isOpened) return false;
        // if (PlayerEntity.Instance.playerData.money < moneyCost) return false; // không đủ tiền
        if (activateValue == false) return true;
        if (!PlayerEntity.Instance.playerInventory.CanUseCoin(moneyCost))
        {
            // Debug.Log("Not enough money to open the door.");
            UIEntity.Instance.uiNotification.NoticeSomething(3f, "Not enough Shell.", "");
            return false;
        }
        PlayerEntity.Instance.playerInventory.UseCoin(moneyCost);
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