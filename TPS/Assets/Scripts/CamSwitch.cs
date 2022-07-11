using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;

public class CamSwitch : MonoBehaviour
{
    [SerializeField] private PlayerInput playerInput;
    private InputAction _aimAction;
    private CinemachineVirtualCamera _virtualCamera;

    private void Awake()
    {
        _virtualCamera = GetComponent<CinemachineVirtualCamera>();
        _aimAction = playerInput.actions["Aim"];
    }

    private void OnEnable()
    {
        _aimAction.performed += _ => StartAim();
        _aimAction.canceled += _ => CancelAim();
    }

    private void OnDisable()
    {
        _aimAction.performed -= _ => StartAim();
        _aimAction.canceled -= _ => CancelAim(); 
    }

    private void StartAim()
    {
        _virtualCamera.Priority += 10;
    }

    private void CancelAim()
    {
        _virtualCamera.Priority -= 10;
    }
}
