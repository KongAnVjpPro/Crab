using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class CameraTrigger : MyMonobehaviour
{
    [SerializeField] CinemachineVirtualCamera newCamera;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
    void OnTriggerEnter2D(Collider2D _other)
    {
        // Debug.Log("trigger");
        if (_other.CompareTag("Player"))
        {
            CameraManager.Instance.SwapCamera(newCamera);
        }
    }
}
