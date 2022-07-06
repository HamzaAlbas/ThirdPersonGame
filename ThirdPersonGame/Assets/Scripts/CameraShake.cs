using System;
using Cinemachine;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    public static CameraShake Instance { get; private set;}
    private CinemachineVirtualCamera _camera;
    private CinemachineImpulseSource _impulseSource;
    private float _shakeTimer;

    private void Awake()
    {
        Instance = this;
        _camera = GetComponent<CinemachineVirtualCamera>();
        _impulseSource = GetComponent<CinemachineImpulseSource>();
    }
    
    public void ShakeCamera()
    {
        _impulseSource.GenerateImpulse();
    }
}
