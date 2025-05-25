using System;
using System.Collections;
using UnityEngine;
using UnityEngine.PlayerLoop;
public class PlayerInventory : PlayerComponent
{
    public Inventory inventory = new Inventory();
    public StatComponent targetUseItem;
    // public bool isFull;
    public int maxSlots = 12;
    [Header("Coin: ")]
    [SerializeField] int currentCoin = 0;
    public Action OnCoinChange;
    protected override void LoadComponents()
    {
        base.LoadComponents();
        Init();
        // UIEntity.Instance.uiInventory.SetInventory(inventory);
        StartCoroutine(WaitForInventory());
        targetUseItem = playerController.playerStat;
    }
    IEnumerator WaitForInventory()
    {
        while (UIEntity.Instance == null || UIEntity.Instance.uiInventory == null)
        {
            yield return null;
        }

        // while (UIEntity.Instance.uiInventory == null)
        // {
        //     // Debug.Log("null");
        //     yield return null;
        // }
        // if (inventory == null) Debug.Log("Null");
        UIEntity.Instance.uiInventory.SetInventory(inventory);
        Debug.Log("Inventory");

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
        ItemData current = inventory.FindItem(itemSO);//return ref
        if (current == null)
        {
            return null;
        }
        // if (itemSO.itemEffectSO.GetIUsable() == null) return null;
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
    public int ItemCount(ItemData item)
    {
        if (!IsContainItem(item)) return -1;
        return inventory.FindItem(item.itemSO).amount;
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
    void Update()
    {
        //test
        // if (Input.GetKeyDown(KeyCode.Space))
        // {
        //     AddCoin(1);
        // }
        // if (Input.GetKeyDown(KeyCode.X))
        // {
        //     UseCoin(1);
        // }

    }
    #region useItem
    [Header("Heal Potion Config (Kelp): ")]
    [SerializeField] public float healPotionCoolDown = 4f;
    // [SerializeField] public float healPotionTimer = 0;
    [SerializeField] public ItemSO healPotionPreb;
    public void UseItem(ItemData itemData)
    {
        itemData.Use(playerController, itemData.amount);
    }
    public void HotKey_UseHealPotion()
    {
        ItemData healPotion = GetItem(healPotionPreb, 1);
        healPotion?.Use(playerController);
    }

    #endregion
    #region Coin
    public void UseCoin(int useAmout)
    {
        if (!CanUseCoin(useAmout)) return;

        currentCoin -= useAmout;
        OnCoinChange?.Invoke();

    }

    public void AddCoin(int addAmount)
    {
        if (!CanAddCoin(addAmount)) return;
        currentCoin += addAmount;
        OnCoinChange?.Invoke();
    }

    bool CanUseCoin(int useAmout)
    {
        return currentCoin >= useAmout;
    }
    bool CanAddCoin(int addAmount)
    {
        return addAmount >= 0;
    }
    public int GetCurrentCoin()
    {
        return currentCoin;
    }

    #endregion
}