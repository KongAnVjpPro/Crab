using UnityEngine;
public class TrapRespawnPoint : MyMonobehaviour
{
    public void SetSpawnPoint()
    {
        GameController.Instance.trapRespawnPoint = transform.position;
    }
}