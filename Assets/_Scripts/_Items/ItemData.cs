using System;
using UnityEngine;
using UnityEngine.Events;
[Serializable]
public class ItemData
{
    public ItemSO itemSO;
    public int amount;
    private IUsableItem usableLogic;
    public void SetUsableLogic(IUsableItem logic)
    {
        this.usableLogic = logic;
    }
    public void Use(EntityController targetUser, int amount = 1)
    {
        usableLogic?.Use(targetUser, amount);
    }
}