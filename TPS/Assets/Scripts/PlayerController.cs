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
    private InputAction _shootAction;
    private Transform _camTransform;

    private Animator _animator;
    private int _moveXAnimationParameterId;
    private int _moveZAnimationParameterId;

    private Vector2 _currentAnimationBlendVector;
    private Vector2 _animationVelocity;
    [SerializeField] private float animationSmoothTime;
    
    [SerializeField] private float playerSpeed = 2.0f;
    [SerializeField] private float jumpHeight = 1.0f;
    [SerializeField] private float gravityValue = -9.81f;
    [SerializeField] private float rotationSpeed = 5f;
    [SerializeField] private float bulletHitMissDistance = 25f;

    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private Transform barrelTransform;
    [SerializeField] private Transform bulletParent;
    
    private void Awake()
    {
        Cursor.lockState = CursorLockMode.Locked;

        _controller = GetComponent<CharacterController>();
        _playerInput = GetComponent<PlayerInput>();
        if (Camera.main != null) _camTransform = Camera.main.transform;
        _moveAction = _playerInput.actions["Move"];
        _jumpAction = _playerInput.actions["Jump"];
        _shootAction = _playerInput.actions["Shoot"];
        _animator = GetComponent<Animator>();
        _moveXAnimationParameterId = Animator.StringToHash("MoveX");
        _moveZAnimationParameterId = Animator.StringToHash("MoveZ");
    }

    private void OnEnable()
    {
        _shootAction.performed += _ => ShootGun();
    }

    private void OnDisable()
    {
        _shootAction.performed -= _ => ShootGun();
    }

    void Update()
    {
        _groundedPlayer = _controller.isGrounded;
        if (_groundedPlayer && _playerVelocity.y < 0)
        {
            _playerVelocity.y = 0f;
        }

        var input = _moveAction.ReadValue<Vector2>();
        _currentAnimationBlendVector = Vector2.SmoothDamp(_currentAnimationBlendVector, input, ref _animationVelocity, animationSmoothTime);
        
        Vector3 move = new Vector3(input.x, 0, input.y);
        //Move in direction to the camera
        move = move.x * _camTransform.right.normalized + move.z * _camTransform.forward.normalized;
        move.y = 0f;
        _controller.Move(move * Time.deltaTime * playerSpeed);
        //Blending animations
        _animator.SetFloat(_moveXAnimationParameterId, _currentAnimationBlendVector.x);
        _animator.SetFloat(_moveZAnimationParameterId, _currentAnimationBlendVector.y);

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

    private void ShootGun()
    {
        RaycastHit hit;
        var bullet = Instantiate(bulletPrefab, barrelTransform.position, Quaternion.identity, bulletParent);
        var bulletController = bullet.GetComponent<BulletController>();

        
        if (Physics.Raycast(_camTransform.position, _camTransform.forward, out hit, Mathf.Infinity))
        {
            bulletController.target = hit.point;
            bulletController.hit = true;
        }
        else
        {
            bulletController.target = _camTransform.position + _camTransform.forward * bulletHitMissDistance;
            bulletController.hit = false;
        }
    }
    
    
}