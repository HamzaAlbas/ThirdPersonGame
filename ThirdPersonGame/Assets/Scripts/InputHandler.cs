using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.LowLevel;

public class InputHandler : MonoBehaviour
{
    #region VARIABLES

    [HideInInspector] public float horizontal;
    [HideInInspector] public float vertical;
    [HideInInspector] public float moveAmount;
    [HideInInspector] public float mouseX;
    [HideInInspector] public float mouseY;
    [HideInInspector] public bool b_Input;
    [HideInInspector] public bool rollFlag;
    [HideInInspector] public bool isInteracting;
    
    private PlayerControls _inputs;
    private CameraHandler _cameraHandler;

    private Vector2 _movementInput;
    private Vector2 _cameraInput;

    #endregion

    private void Awake()
    {
        _cameraHandler = CameraHandler.Singleton;
    }

    private void FixedUpdate()
    {
        var delta = Time.fixedDeltaTime;

        if (_cameraHandler != null)
        {
            _cameraHandler.FollowTarget(delta);
            _cameraHandler.HandleCameraRotation(delta, mouseX, mouseY);
        }
    }

    public void OnEnable()
    {
        if (_inputs == null)
        {
            _inputs = new PlayerControls();
            _inputs.PlayerMovement.Movement.performed += input => _movementInput = input.ReadValue<Vector2>();
            _inputs.PlayerMovement.Camera.performed += i => _cameraInput = i.ReadValue<Vector2>();
        }
        _inputs.Enable();
    }

    private void OnDisable()
    {
        _inputs.Disable();
    }


    public void TickInput(float delta)
    {
        MoveInput(delta);
        HandleRollInput(delta);
    }

    private void MoveInput(float delta)
    {
        horizontal = _movementInput.x;
        vertical = _movementInput.y;
        moveAmount = Mathf.Clamp01(Mathf.Abs(horizontal) + Mathf.Abs(vertical));
        mouseX = _cameraInput.x;
        mouseY = _cameraInput.y;
    }

    private void HandleRollInput(float delta)
    {
        b_Input = _inputs.PlayerActions.Roll.phase == UnityEngine.InputSystem.InputActionPhase.Started;
        
        if (b_Input)
        {
            rollFlag = true;
        }
    }
}