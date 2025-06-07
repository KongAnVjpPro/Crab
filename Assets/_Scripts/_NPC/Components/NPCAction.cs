using System.Collections.Generic;
using UnityEngine;
public class NPCAction : NPCComponent
{
    public ItemSO itemToGive;
    public virtual void ChangeDialogue(int id)
    {
        List<Dialogue> dl = npcController.dialogueTrigger.alterDialogue;
        if (id < 0) return;
        if (dl.Count == 0 || id > dl.Count - 1)
        {
            return;
        }
        npcController.dialogueTrigger.currentDialogue = dl[id];
    }
    public virtual void ChangeDialogue(string key)
    {
        List<Dialogue> dlist = new List<Dialogue>();
        foreach (var dl in npcController.dialogueTrigger.alterDialogue)
        {
            if (dl.dialogueKey == key)
            {
                dlist.Add(dl);
                // npcController.dialogueTrigger.currentDialogue = dl;
                // return;
            }
        }
        if (dlist.Count > 0)
        {
            int rand = Random.Range(0, dlist.Count);
            npcController.dialogueTrigger.currentDialogue = dlist[rand];
        }
        else
        {
            Debug.LogWarning("No dialogue found with key: " + key);
        }
    }
    // public void GiveItemToPlayer(NPCItemType itemType, int amount)
    // {
    //     ItemSO itemSO = FormItemTypeToItemSO(itemType);
    //     ItemData itemData = new ItemData { itemSO = itemSO, amount = amount };
    //     PlayerEntity.Instance.playerInventory.AddItem(itemData);

    //     //noti...

    // }
    #region  give item
    public void GiveItemToPlayer(int amount)
    {
        PlayerEntity.Instance.playerInventory.AddItem(new ItemData { itemSO = itemToGive, amount = amount });
        UIEntity.Instance.Reload();
        UIEntity.Instance.uiNotification.NoticeSomething(4f, "Receive " + itemToGive.itemName + " x" + amount, "");
    }
    #endregion
    #region give coin
    public void GiveCoinToPlayer(int amount)
    {
        PlayerEntity.Instance.playerInventory.AddCoin(amount);
    }
    public ItemSO FormItemTypeToItemSO(NPCItemType itemType)
    {
        switch (itemType)
        {
            case NPCItemType.healPotion:
                return itemToGive;
            default:
                return null;
        }
        // return null;
    }
    #endregion
    #region set appear
    public void SetAppear(bool appearValue)
    {

        npcController.SetCanAppear(appearValue);


    }
    public void SetTalk(bool talkValue)
    {
        // npcController.dialogueTrigger.enabled = talkValue;
        npcController.anim.SetBool("Talk", talkValue);
    }
    #endregion
}
public enum NPCItemType
{
    healPotion = 0,
}