using UnityEngine;


public class InputHandler : MonoBehaviour
{
    #region VARIABLES

    [HideInInspector] public float horizontal;
    [HideInInspector] public float vertical;
    [HideInInspector] public float moveAmount;
    [HideInInspector] public float mouseX;
    [HideInInspector] public float mouseY;
    [HideInInspector] public float rollInputTimer;
    [HideInInspector] public bool bInput;
    [HideInInspector] public bool rollFlag;
    [HideInInspector] public bool sprintFlag;
    
    private PlayerControls _inputs;
    private CameraHandler _cameraHandler;

    private Vector2 _movementInput;
    private Vector2 _cameraInput;

    #endregion

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
        bInput = _inputs.PlayerActions.Roll.phase == UnityEngine.InputSystem.InputActionPhase.Started;
        
        if (bInput)
        {
            rollInputTimer += delta;
            sprintFlag = true;
        }
        else
        {
            if (rollInputTimer > 0 && rollInputTimer <0.5f)
            {
                sprintFlag = false;
                rollFlag = true;
            }

            rollInputTimer = 0;
        }
    }
}