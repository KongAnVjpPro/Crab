using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MyMonobehaviour
{
    [SerializeField] protected float followSpeed = 5f;
    [SerializeField] Vector3 defaultCam = new Vector3(0, 0, -10f);
    void Update()
    {
        CameraMove();
    }
    public void CameraMove()
    {
        transform.position = Vector3.Lerp(transform.position, PlayerController.Instance.transform.position + defaultCam, followSpeed * Time.deltaTime);

    }
}
