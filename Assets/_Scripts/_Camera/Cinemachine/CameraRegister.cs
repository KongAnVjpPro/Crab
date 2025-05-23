using Cinemachine;
using UnityEngine;
partial class CameraRegister : MyMonobehaviour
{
    private void OnEnable()
    {
        CameraController.Register(GetComponent<CinemachineVirtualCamera>());
    }
    private void Osable()
    {
        CameraController.Unregister(GetComponent<CinemachineVirtualCamera>());
    }
}