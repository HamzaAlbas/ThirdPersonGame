using System.Diagnostics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem.HID;
using Debug = UnityEngine.Debug;

public class PlayerController : MonoBehaviour
{
    #region VARIABLES

    private Transform _cameraObject;
    private InputHandler _inputHandler;
    private PlayerManager _playerManager;
    public Vector3 moveDirection;
    [HideInInspector] public Transform myTransform;
    [HideInInspector] public AnimatorHandler animatorHandler;

    [Header("References")]
    public new Rigidbody rigidbody;
    public GameObject normalCamera;

    [Header("Stats")] 
    [SerializeField] private float movementSpeed = 5;
    [SerializeField] private float rotationSpeed;
    [SerializeField] private float sprintSpeed = 7;
    [SerializeField] private float fallingSpeed = 45;

    [Header("Ground & Air Detection")] 
    [SerializeField] private float groundDetectionRayStartPoint = 0.5f;

    [SerializeField] private float minimumDistanceNeededToBeginFall = 1f;
    [SerializeField] private float groundDirectionRayDistance = 0.2f;
    private LayerMask _ignoreForGroundCheck;
    [HideInInspector] public float inAirTimer;
    
    //Movement
    private Vector3 _normalVector;
    private Vector3 _targetPosition;
    private static readonly int IsInteracting = Animator.StringToHash("isInteracting");

    #endregion
    
    private void Start()
    {
        GetReferences();   
    }

    #region MOVEMENT

    private void HandleRotation(float delta)
    {
        var moveOverride = _inputHandler.moveAmount;

        var targetDir = _cameraObject.forward * _inputHandler.vertical;
        targetDir += _cameraObject.right * _inputHandler.horizontal;
        
        targetDir.Normalize();
        targetDir.y = 0;

        if (targetDir == Vector3.zero)
        {
            targetDir = myTransform.forward;
        }

        var rs = rotationSpeed;

        var tr = Quaternion.LookRotation(targetDir);
        var targetRotation = Quaternion.Slerp(myTransform.rotation, tr, rs * delta);

        myTransform.rotation = targetRotation;

    }

    public void HandleMovement(float delta)
    {
        if (_inputHandler.rollFlag)
        {
            return;
        }

        if (_playerManager.isInteracting)
        {
            return;
        }
        
        moveDirection = _cameraObject.forward * _inputHandler.vertical;
        moveDirection += _cameraObject.right * _inputHandler.horizontal;
        moveDirection.Normalize();
        moveDirection.y = 0;

        var speed = movementSpeed;

        if (_inputHandler.sprintFlag)
        {
            speed = sprintSpeed;
            _playerManager.isSprinting = true;
            moveDirection *= speed;
        }
        else
        {
            moveDirection *= speed;
        }

        var projectedVelocity = Vector3.ProjectOnPlane(moveDirection, _normalVector);
        rigidbody.velocity = projectedVelocity;

        animatorHandler.UpdateAnimatorValues(_inputHandler.moveAmount, 0, _playerManager.isSprinting);
        if (animatorHandler.canRotate)
        {
            HandleRotation(delta);
        }
    }

    public void HandleRollingAndSprinting(float delta)
    {
        if (animatorHandler.anim.GetBool(IsInteracting))
            return;
        if (_inputHandler.rollFlag)
        {
            moveDirection = _cameraObject.forward * _inputHandler.vertical;
            moveDirection += _cameraObject.right * _inputHandler.horizontal;

            if (_inputHandler.moveAmount > 0)
            {
                animatorHandler.PlayTargetAnimation("Rolling", true);
                moveDirection.y = 0;
                var rollRotation = Quaternion.LookRotation(moveDirection);
                myTransform.rotation = rollRotation;
            }
            else
            {
                animatorHandler.PlayTargetAnimation("Backstep", true);
            }
        }
    }

    public void HandleFalling(float delta, Vector3 moveDirection)
    {
        _playerManager.isGrounded = false;
        RaycastHit hit;
        var origin = myTransform.position;
        origin.y += groundDetectionRayStartPoint;

        if (Physics.Raycast(origin, myTransform.forward, out hit, 0.4f))
        {
            moveDirection = Vector3.zero;
        }

        if (_playerManager.isInAir)
        {
            rigidbody.AddForce(-Vector3.up * fallingSpeed);
            rigidbody.AddForce(moveDirection * fallingSpeed / 10f);
        }

        var dir = moveDirection;
        dir.Normalize();
        origin = origin + dir * groundDirectionRayDistance;

        _targetPosition = myTransform.position;
        
        Debug.DrawRay(origin, -Vector3.up * minimumDistanceNeededToBeginFall, Color.red, 0.1f, false);
        if (Physics.Raycast(origin, -Vector3.up, out hit, minimumDistanceNeededToBeginFall, _ignoreForGroundCheck))
        {
            _normalVector = hit.normal;
            var tp = hit.point;
            _playerManager.isGrounded = true;
            _targetPosition.y = tp.y;

            if (_playerManager.isInAir)
            {
                if (inAirTimer > 0.5f)
                {
                    animatorHandler.PlayTargetAnimation("Land", true);
                    inAirTimer = 0;
                }
                else
                {
                    animatorHandler.PlayTargetAnimation("Locomotion", false);
                    inAirTimer = 0;
                }
                _playerManager.isInAir = false;
            }
        }
        else
        {
            if (_playerManager.isGrounded)
            {
                _playerManager.isGrounded = false;
            }

            if (_playerManager.isInAir == false)
            {
                if (_playerManager.isInteracting == false)
                {
                    animatorHandler.PlayTargetAnimation("Falling", true);
                }

                var vel = rigidbody.velocity;
                vel.Normalize();
                rigidbody.velocity = vel * ( movementSpeed /2);
                _playerManager.isInAir = true;
            }
        }

        if (_playerManager.isGrounded)
        {
            if (_playerManager.isInteracting || _inputHandler.moveAmount > 0)
            {
                myTransform.position = Vector3.Lerp(myTransform.position, _targetPosition, Time.deltaTime);
            }
            else
            {
                myTransform.position = _targetPosition;
            }
        }
    }
    
    #endregion
    
    private void GetReferences()
    {
        _playerManager = GetComponent<PlayerManager>();
        rigidbody = GetComponent<Rigidbody>();
        _inputHandler = GetComponent<InputHandler>();
        if (Camera.main != null) _cameraObject = Camera.main.transform;
        myTransform = transform;
        animatorHandler = GetComponentInChildren<AnimatorHandler>();
        animatorHandler.Initialize();

        _playerManager.isGrounded = true;
        _ignoreForGroundCheck = ~(1 << 8 | 1 << 11);
    }
}
