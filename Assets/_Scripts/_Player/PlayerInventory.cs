using UnityEngine;
using UnityEngine.PlayerLoop;
public class PlayerInventory : PlayerComponent
{
    public Inventory inventory;

    protected override void LoadComponents()
    {
        base.LoadComponents();
        Init();
        UIEntity.Instance.uiInventory.SetInventory(inventory);
    }
    void Init()
    {
        inventory = new Inventory();
    }
}