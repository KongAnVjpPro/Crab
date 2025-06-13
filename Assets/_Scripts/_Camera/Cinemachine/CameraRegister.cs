using Cinemachine;
using UnityEngine;
partial class CameraRegister : MyMonobehaviour
{
    private void OnEnable()
    {
        CameraController.Register(GetComponent<CinemachineVirtualCamera>());
    }
    private void OnDisable()
    {
        CameraController.Unregister(GetComponent<CinemachineVirtualCamera>());
    }
}