using UnityEngine;
public class DoorActivate : MyMonobehaviour
{
    [SerializeField] protected bool isOpened = false;
    [SerializeField] protected Animator anim;
    [Header("Door Settings: {Door key = scene name}")]
    [SerializeField] protected string doorKey = "";

    public void GetDoorState()
    {
        isOpened = SaveSystem.Instance.GetDoorState(doorKey);
    }
    public void SetDoorState(bool state)
    {
        if (isOpened) return;
        if (CheckCondition(state) == false)
        {
            Debug.Log("S00");
            return;
        }
        Debug.Log("Door");
        isOpened = state;
        SaveSystem.Instance.SetDoorState(doorKey, state);
        anim?.SetBool("IsOpened", isOpened);
        // SaveSystem.Instance.SaveGlobalData();
    }
    // protected virtual void DoorActivateVisual()
    // {

    // }
    // protected virtual void DoorDeactivateVisual()
    // {

    // }
    public virtual bool CheckCondition(bool activateValue)
    {
        if (!activateValue) return true;
        return true;
    }
}