using UnityEngine;
using UnityEngine.PlayerLoop;
public class PlayerInventory : PlayerComponent
{
    public Inventory inventory;

    protected override void LoadComponents()
    {
        base.LoadComponents();
        Init();
    }
    void Init()
    {
        inventory = new Inventory();
    }
}