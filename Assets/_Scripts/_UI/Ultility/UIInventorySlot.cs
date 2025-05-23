using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
public class UIInventorySlot : MyMonobehaviour
{
    public enum SlotState
    {
        empty,
        occupied,
        full,
        none,
    }
    [Header("UI Props:")]
    [SerializeField] private CanvasGroup slotClickedImage;
    [SerializeField] private Image itemImage;
    [SerializeField] private TextMeshProUGUI itemCountText;
    private SlotState pState = SlotState.none;
    public ItemData currentItem;

    [Header("Ref:")]
    public UIInventory uiInventory;

    public void SetUIInventory(UIInventory uiInventory)
    {
        this.uiInventory = uiInventory;
    }



    public SlotState State
    {
        get => pState;
        set
        {
            if (pState == value) return;
            pState = value;
            DoOnChangeState();
        }
    }
    private void DoOnChangeState()
    {
        switch (pState)
        {
            case SlotState.empty:
                itemImage.enabled = false;
                itemCountText.text = string.Empty;
                break;
            case SlotState.occupied:
                itemImage.enabled = true;
                // itemCountText.enabled = true;
                break;
            case SlotState.full:
                itemImage.enabled = true;
                // itemCountText.enabled = true;
                break;
        }
    }

    // [SerializeField] private UIInventory uiInventory;
    protected override void LoadComponents()
    {
        base.LoadComponents();
        this.LoadImage();
        this.LoadTextCount();



        State = SlotState.empty;
    }
    protected virtual void LoadImage()
    {
        if (itemImage != null) return;
        itemImage = GetComponentInChildren<Image>();
    }
    protected virtual void LoadTextCount()
    {
        if (itemCountText != null) return;
        itemCountText = GetComponentInChildren<TextMeshProUGUI>();
    }
    public void UpdateItemUI(ItemData itemData)
    {

        itemImage.sprite = itemData.itemSO.itemSprite;
        itemCountText.text = itemData.amount.ToString();
        currentItem = itemData;
        // uiInventory.ShowItemDescription(itemData);
    }

    // xử lý update riêng lẻ chỗ này thay vì cập nhật cả Invent, chưa tích hợp tại hết tgian
    public void ClearItemUI()
    {
        itemImage.sprite = null;
        itemCountText.text = string.Empty;
        State = SlotState.empty;
        currentItem = null;
    }
    public void EndClickHandle()
    {
        slotClickedImage.DOFade(0, 0.5f);
        // uiInventory.HideItemDescription();
    }
    public void OnClickHandle()
    {

        slotClickedImage.DOFade(1f, 0.5f);
        uiInventory.previousSlot?.EndClickHandle();
        uiInventory.currentSlot = this;
        uiInventory.previousSlot = this;


        uiInventory.ShowItemDescription(currentItem);
    }

}