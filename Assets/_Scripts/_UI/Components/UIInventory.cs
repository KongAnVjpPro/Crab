using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
public class UIInventory : UIComponent
{
    [SerializeField] private Inventory inventory;
    [SerializeField] private RectTransform itemSlotContainer;
    [SerializeField] private UIInventorySlot itemSlotTemplate;
    [SerializeField] private UIInventoryDescription descriptionPanel;
    [SerializeField] private List<UIInventorySlot> itemSlots = new List<UIInventorySlot>();

    public CanvasGroup inventoryCanvasGroup;
    public Image blurBackground;

    public bool isInventoryOpen = false;
    [SerializeField] private float inventoryOpenTime = 0.5f;
    [SerializeField] private float lastToggleTime = -999f;


    [Header("Handle Click Slot: ")]
    [SerializeField] public UIInventorySlot currentSlot;
    [SerializeField] public UIInventorySlot previousSlot;
    protected override void Awake()
    {
        base.Awake();
    }
    protected override void LoadComponents()
    {
        base.LoadComponents();
        LoadItemSlot();
        SetSlotRef();
    }

    void Update()
    {
        if (PlayerEntity.Instance.playerInput.inventory && Time.time - lastToggleTime > inventoryOpenTime)
        {
            lastToggleTime = Time.time;

            isInventoryOpen = !isInventoryOpen;
            if (isInventoryOpen)
            {
                ShowInventory();
                GameController.Instance.isBlockPlayerControl = true;
            }
            else
            {
                HideInventory();
                GameController.Instance.isBlockPlayerControl = false;
            }
        }
    }
    public void ShowInventory()
    {
        // if (isInventoryOpen) return;
        isInventoryOpen = true;
        Sequence seq = DOTween.Sequence();
        seq.Join(inventoryCanvasGroup.DOFade(1f, 0.25f));
        seq.Join(blurBackground.DOFade(0.5f, 0.25f));

        inventoryCanvasGroup.interactable = true;
        inventoryCanvasGroup.blocksRaycasts = true;
        RefreshInventoryItems();

        currentSlot?.EndClickHandle();
        currentSlot = null;
        previousSlot?.EndClickHandle();
        previousSlot = null;
    }
    public void HideInventory()
    {
        // if (!isInventoryOpen) return;
        isInventoryOpen = false;
        Sequence seq = DOTween.Sequence();
        seq.Join(inventoryCanvasGroup.DOFade(0f, 0.25f));
        seq.Join(blurBackground.DOFade(0f, 0.25f));

        inventoryCanvasGroup.interactable = false;
        inventoryCanvasGroup.blocksRaycasts = false;
        RefreshInventoryItems();

        currentSlot?.EndClickHandle();
        currentSlot = null;
        previousSlot?.EndClickHandle();
        previousSlot = null;

        HideItemDescription();
    }
    protected virtual void LoadItemSlot()
    {
        if (itemSlots.Count > 0) return;
        foreach (UIInventorySlot slot in itemSlotContainer.GetComponentsInChildren<UIInventorySlot>())
        {
            itemSlots.Add(slot);
            // slot.gameObject.SetActive(false);
        }
    }

    public void SetInventory(Inventory inventory)
    {
        this.inventory = inventory;
        RefreshInventoryItems();
    }
    public void SetSlotRef()
    {
        foreach (UIInventorySlot slot in itemSlots)
        {
            slot.SetUIInventory(this);
        }
    }

    public void RefreshInventoryItems()
    {
        List<ItemData> items = inventory.GetItems();
        for (int i = 0; i < itemSlots.Count; i++)
        {
            // if (itemSlots[i] == null)
            // {
            //     itemSlots[i] = Instantiate(itemSlotTemplate, itemSlotContainer);
            //     itemSlots[i].gameObject.SetActive(true);
            // }
            if (i > items.Count - 1)
            {
                itemSlots[i].ClearItemUI();
                itemSlots[i].State = UIInventorySlot.SlotState.empty;
                continue;
            }

            itemSlots[i].State = UIInventorySlot.SlotState.occupied;
            itemSlots[i].UpdateItemUI(items[i]);

        }

    }
    public void ShowItemDescription(ItemData itemData)
    {
        descriptionPanel.ShowDescription(itemData);
    }
    public void HideItemDescription()
    {
        descriptionPanel.HideDescription();
    }
}