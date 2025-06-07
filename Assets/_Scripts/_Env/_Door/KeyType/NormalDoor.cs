using UnityEngine;

public class NormalDoor : DoorActivate
{
    public override bool CheckCondition(bool activateValue)
    {
        // return base.CheckCondition(activateValue);
        if (isOpened) return false;// cửa 1 chiều
        return true;
    }
}