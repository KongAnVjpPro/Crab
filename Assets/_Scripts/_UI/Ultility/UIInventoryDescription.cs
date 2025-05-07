using TMPro;
using UnityEngine;
using UnityEngine.UI;
public class UIInventoryDescription : MyMonobehaviour
{
    public TextMeshProUGUI itemNameText;
    public TextMeshProUGUI itemDescriptionText;
    public Image itemImage;
    // public Image backgroundImage;

    public void ShowDescription(ItemData itemData)
    {
        // backgroundImage.enabled = true;
        if (itemData == null || itemData.itemSO == null)
        {
            HideDescription();
            return;
        }
        itemImage.enabled = true;
        itemNameText.text = itemData.itemSO.itemName;
        itemDescriptionText.text = itemData.itemSO.itemDescription;
        itemImage.sprite = itemData.itemSO.itemSprite;
    }
    public void HideDescription()
    {
        // backgroundImage.enabled = false;
        itemNameText.text = string.Empty;
        itemDescriptionText.text = string.Empty;
        itemImage.sprite = null;
        itemImage.enabled = false;
    }
    protected override void Awake()
    {
        base.Awake();
        HideDescription();
    }
}