using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MyMonobehaviour
{
    [SerializeField] protected float followSpeed = 5f;
    [SerializeField] Vector3 defaultCam = new Vector3(0, 0, -20f);
    [SerializeField] protected GameObject target;
    void Update()
    {
        CameraMove();
    }
    public void CameraMove()
    {
        transform.position = Vector3.Lerp(transform.position, target.transform.position + defaultCam, followSpeed * Time.deltaTime);

    }
}
