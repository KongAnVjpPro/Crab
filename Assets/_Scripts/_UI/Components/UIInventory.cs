using UnityEngine;
public class UIInventory : UIComponent
{
    [SerializeField] private Inventory inventory;
    [SerializeField] private RectTransform itemSlotContainer;
    [SerializeField] private RectTransform itemSlotTemplate;
    protected override void Awake()
    {
        base.Awake();
    }


    public void SetInventory(Inventory inventory)
    {
        this.inventory = inventory;
        RefreshInventoryItems();
    }

    private void RefreshInventoryItems()
    {
        foreach (Item item in inventory.GetItems())
        {
            RectTransform itemSlotTransform = Instantiate(itemSlotTemplate, itemSlotContainer);
            itemSlotTransform.gameObject.SetActive(true);

        }
    }
}