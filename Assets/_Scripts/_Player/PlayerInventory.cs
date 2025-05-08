using UnityEngine;
using UnityEngine.PlayerLoop;
public class PlayerInventory : PlayerComponent
{
    public Inventory inventory;
    public StatComponent targetUseItem;
    // public bool isFull;
    public int maxSlots = 12;
    protected override void LoadComponents()
    {
        base.LoadComponents();
        Init();
        UIEntity.Instance.uiInventory.SetInventory(inventory);
        targetUseItem = playerController.playerStat;
    }
    void Init()
    {
        // inventory = new Inventory();
    }

    public void AddItem(ItemData item)
    {


        inventory.AddItem(item);
        UIEntity.Instance.uiInventory.SetInventory(inventory);
        // UIEntity.Instance.uiInventory.UpdateInventoryUI(inventory);z
    }
    public ItemData GetItem(ItemSO itemSO, int amount)
    {
        ItemData current = inventory.FindItem(itemSO);
        if (current == null)
        {
            return null;
        }
        int newAmount = Mathf.Clamp(current.amount - amount, 0, current.itemSO.maxStack);
        int amountGet = newAmount == 0 ? current.amount : amount;
        current.amount = newAmount;
        if (current.amount == 0)
        {
            inventory.GetItems().Remove(current);
        }
        UIEntity.Instance.uiInventory.SetInventory(inventory);
        return new ItemData { itemSO = current.itemSO, amount = amountGet };
    }
    public bool IsFullStack(ItemData item)
    {
        foreach (var i in inventory.GetItems())
        {
            if (i.itemSO == item.itemSO && i.amount >= item.itemSO.maxStack)
            {
                return true;
            }
        }
        return false;
    }
    public bool IsFullSlot()
    {
        if (inventory.GetItems().Count >= maxSlots)
        {
            // isFull = true;
            return true;
        }
        else
        {
            // isFull = false;
            return false;
        }
    }
    public bool IsContainItem(ItemData item)
    {
        foreach (var i in inventory.GetItems())
        {
            if (i.itemSO == item.itemSO)
            {
                return true;
            }
        }
        return false;
    }
    public bool CanAddItem(ItemData item)
    {
        //co item 
        if (IsContainItem(item))
        {
            if (IsFullStack(item))
            {
                return false;
            }
            else
            {
                return true;
            }
        }
        if (IsFullSlot())
        {
            return false;
        }

        return true;
    }
}