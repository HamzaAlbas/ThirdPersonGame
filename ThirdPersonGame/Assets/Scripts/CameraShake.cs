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
    
/*
    public void ShakeCamera(float intensity, float time)
    {
        var perlin = _camera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
        perlin.m_AmplitudeGain = intensity;
        _shakeTimer = time;
    }

    private void Update()
    {
        if (_shakeTimer > 0 )
        {
            _shakeTimer -= Time.deltaTime;
            if (_shakeTimer <= 0f)
            {
                var perlin = _camera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
                perlin.m_AmplitudeGain = 0.7f;
            }
        }
    }*/
}
