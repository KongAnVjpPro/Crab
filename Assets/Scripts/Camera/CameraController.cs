using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MyMonobehaviour
{
    [SerializeField] protected CameraFollow cameraFollow;
    public CameraFollow CameraFollow => cameraFollow;
    // private CameraController instance;
    // public CameraController Instance => instance;
    protected override void LoadComponents()
    {
        base.LoadComponents();
        this.LoadCameraFollow();
    }
    void LoadCameraFollow()
    {
        if (this.cameraFollow != null) return;
        this.cameraFollow = GetComponent<CameraFollow>();
    }
    protected override void Awake()
    {
        // if (this.instance == null)
        // {
        //     instance = this;
        // }
        // else
        // {
        //     if (instance != this)
        //     {
        //         Destroy(gameObject);
        //     }

        // }
        base.Awake();

    }
}
