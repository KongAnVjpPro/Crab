using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Cinemachine;
using UnityEngine;

public class CameraManager : MyMonobehaviour
{
    private static CameraManager instance;
    public static CameraManager Instance => instance;
    [SerializeField] CinemachineVirtualCamera[] allVirtualCameras;
    private CinemachineVirtualCamera currentCamera;
    private CinemachineFramingTransposer framingTransposer;
    [Header("Y Damping Settings: ")]
    [SerializeField] private float panAmount = 0.1f;
    [SerializeField] private float panTime = 0.2f;
    public float playerSpeedThreshold = -10;
    public bool isLerpingYDamping;
    public bool hasLerpedYDamping;
    private float normalYDamp;



    protected override void Awake()
    {
        base.Awake();
        LoadCamera();
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            if (instance != this)
            {
                Destroy(gameObject);
            }

        }
        for (int i = 0; i < allVirtualCameras.Length; i++)
        {
            if (allVirtualCameras[i].enabled)
            {
                currentCamera = allVirtualCameras[i];
                framingTransposer = currentCamera.GetCinemachineComponent<CinemachineFramingTransposer>();
            }
        }
        normalYDamp = framingTransposer.m_YDamping;
        // DontDestroyOnLoad(gameObject); 1 scene, 1 object
    }
    private void Start()
    {
        for (int i = 0; i < allVirtualCameras.Length; i++)
        {
            allVirtualCameras[i].Follow = PlayerController.Instance.transform;
        }
    }
    public void SwapCamera(CinemachineVirtualCamera newCam)
    {
        currentCamera.enabled = false;

        currentCamera = newCam;
        currentCamera.enabled = true;
        framingTransposer = newCam.GetCinemachineComponent<CinemachineFramingTransposer>();
        newCam.Follow = PlayerController.Instance.transform;
    }
    public void LoadCamera()
    {
        GameObject camHolder = GameObject.Find("Cameras");
        allVirtualCameras = camHolder.GetComponentsInChildren<CinemachineVirtualCamera>();
    }
    public IEnumerator LerpYDamping(bool _isPlayerFalling)
    {
        isLerpingYDamping = true;
        float _startYDamp = framingTransposer.m_YDamping;
        float _endYDamp = 0;

        if (_isPlayerFalling)
        {
            _endYDamp = panAmount;
            hasLerpedYDamping = true;
        }
        else
        {
            _endYDamp = normalYDamp;
        }


        float _timer = 0;
        while (_timer < panTime)
        {
            _timer += Time.deltaTime;
            float _lerpedPanAmount = Mathf.Lerp(_startYDamp, _endYDamp, (_timer / panTime));
            framingTransposer.m_YDamping = _lerpedPanAmount;
            yield return null;
        }
        isLerpingYDamping = false;
    }
}
