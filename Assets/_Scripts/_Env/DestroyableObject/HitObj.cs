using UnityEngine;
public class HitObject : MyMonobehaviour
{
    public DamagedAbleObject obj;
    public void Hit()
    {
        obj.HitObject();
    }
}