using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController), typeof(PlayerInput))]
public class PlayerController : MonoBehaviour
{
    private CharacterController _controller;
    private Vector3 _playerVelocity;
    private bool _groundedPlayer;
    private PlayerInput _playerInput;
    private InputAction _moveAction;
    private InputAction _jumpAction;
    private Transform _camTransform;
    
    
    [SerializeField] private float playerSpeed = 2.0f;
    [SerializeField] private float jumpHeight = 1.0f;
    [SerializeField] private float gravityValue = -9.81f;
    [SerializeField] private float rotationSpeed = 5f;

    private void Start()
    {
        _controller = GetComponent<CharacterController>();
        _playerInput = GetComponent<PlayerInput>();
        if (Camera.main != null) _camTransform = Camera.main.transform;
        _moveAction = _playerInput.actions["Move"];
        _jumpAction = _playerInput.actions["Jump"];
    }

    void Update()
    {
        _groundedPlayer = _controller.isGrounded;
        if (_groundedPlayer && _playerVelocity.y < 0)
        {
            _playerVelocity.y = 0f;
        }

        var input = _moveAction.ReadValue<Vector2>();
        Vector3 move = new Vector3(input.x, 0, input.y);
        //Move in direction to the camera
        move = move.x * _camTransform.right.normalized + move.z * _camTransform.forward.normalized;
        move.y = 0f;
        _controller.Move(move * Time.deltaTime * playerSpeed);

        //Rotate towards camera direction
        var targetAngle = _camTransform.eulerAngles.y;
        var targetRotation = Quaternion.Euler(0, targetAngle, 0); 
        transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        
        
        if (_jumpAction.triggered && _groundedPlayer)
        {
            _playerVelocity.y += Mathf.Sqrt(jumpHeight * -3.0f * gravityValue);
        }

        _playerVelocity.y += gravityValue * Time.deltaTime;
        _controller.Move(_playerVelocity * Time.deltaTime);
    }
}