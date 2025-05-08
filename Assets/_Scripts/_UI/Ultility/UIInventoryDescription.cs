using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
public class UIInventoryDescription : MyMonobehaviour
{
    public TextMeshProUGUI itemNameText;
    public TextMeshProUGUI itemDescriptionText;
    public Image itemImage;
    public ItemData currentItemData;

    public TextMeshProUGUI itemCountText;

    [Header("Button: ")]
    public CanvasGroup buttonGroup;
    // public Button useButton;
    // public Button decreaseButton;
    // public Button increaseButton;

    // public Image backgroundImage;

    public void ShowDescription(ItemData itemData)
    {
        // backgroundImage.enabled = true;
        if (itemData == null || itemData.itemSO == null)
        {
            HideDescription();
            return;
        }
        currentItemData = itemData;
        itemImage.enabled = true;
        itemNameText.text = itemData.itemSO.itemName;
        itemDescriptionText.text = itemData.itemSO.itemDescription;
        itemImage.sprite = itemData.itemSO.itemSprite;

        ActivateButtonGroup(true);
    }
    public void HideDescription()
    {
        // backgroundImage.enabled = false;
        itemNameText.text = string.Empty;
        itemDescriptionText.text = string.Empty;
        itemImage.sprite = null;
        itemImage.enabled = false;
        ActivateButtonGroup(false);
        currentItemData = null;
    }
    protected override void Awake()
    {
        base.Awake();
        HideDescription();
    }

    #region Button Event

    public void OnClickIncreaseCount()
    {
        itemCountText.text = Mathf.Clamp((int.Parse(itemCountText.text) + 1), 1, currentItemData.amount).ToString();
    }
    public void OnClickDecreaseCount()
    {
        itemCountText.text = Mathf.Clamp((int.Parse(itemCountText.text) - 1), 1, currentItemData.amount).ToString();
    }
    public void ActivateButtonGroup(bool isActive)
    {
        // buttonGroup.DOFade(isActive ? 1 : 0, 1f);
        buttonGroup.alpha = isActive ? 1 : 0;
        buttonGroup.interactable = isActive;
        buttonGroup.blocksRaycasts = isActive;
        itemCountText.text = "1";
    }
    public void OnClickUseItem()
    {
        if (currentItemData == null) return;
        int amount = int.Parse(itemCountText.text);
        ItemData item = PlayerEntity.Instance.playerInventory.GetItem(currentItemData.itemSO, amount);
        if (item != null)
        {
            // PlayerEntity.Instance.playerInventory.AddItem(item);
            // UIEntity.Instance.uiInventory.UpdateInventoryUI(PlayerEntity.Instance.playerInventory.inventory);
            UIEntity.Instance.uiInventory.RefreshInventoryItems();

            HideDescription();
        }

        // Debug.Log($"Use item: {item.itemSO.itemName} - amount: {amount}");
    }
    #endregion


}