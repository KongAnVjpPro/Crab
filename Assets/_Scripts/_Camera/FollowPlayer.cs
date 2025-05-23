using Cinemachine;
using UnityEngine;
public class FollowPlayer : MyMonobehaviour
{
    [SerializeField] CinemachineVirtualCamera cam;
    protected override void LoadComponents()
    {
        base.LoadComponents();
        if (this.cam != null) return;
        this.cam = GetComponent<CinemachineVirtualCamera>();
    }
    void FindPlayer()
    {
        Transform player = PlayerEntity.Instance.transform;
        cam.Follow = player;
    }
    void Start()
    {
        FindPlayer();
    }
}