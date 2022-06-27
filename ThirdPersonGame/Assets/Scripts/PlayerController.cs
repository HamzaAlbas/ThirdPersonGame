using UnityEngine;

public class PlayerController : MonoBehaviour
{
    #region VARIABLES

    private Transform _cameraObject;
    private InputHandler _inputHandler;
    private Vector3 _moveDirection;
    [HideInInspector] public Transform myTransform;
    [HideInInspector] public AnimatorHandler animatorHandler;

    [Header("References")]
    public new Rigidbody rigidbody;
    public GameObject normalCamera;

    [Header("Stats")] 
    [SerializeField] private float movementSpeed = 5;
    [SerializeField] private float rotationSpeed;
    
    //Movement
    private Vector3 _normalVector;
    private Vector3 _targetPosition;
    #endregion
    
    private void Start()
    {
        GetReferences();   
    }

    private void Update()
    {
        var delta = Time.deltaTime;
        
        _inputHandler.TickInput(delta);

        _moveDirection = _cameraObject.forward * _inputHandler.vertical;
        _moveDirection += _cameraObject.right * _inputHandler.horizontal;
        _moveDirection.Normalize();
        _moveDirection.y = 0;

        var speed = movementSpeed;
        _moveDirection *= speed;

        var projectedVelocity = Vector3.ProjectOnPlane(_moveDirection, _normalVector);
        rigidbody.velocity = projectedVelocity;

        animatorHandler.UpdateAnimatorValues(_inputHandler.moveAmount, 0);
        if (animatorHandler.canRotate)
        {
            HandleRotation(delta);
        }
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
    
    #endregion
    
    private void GetReferences()
    {
        rigidbody = GetComponent<Rigidbody>();
        _inputHandler = GetComponent<InputHandler>();
        if (Camera.main != null) _cameraObject = Camera.main.transform;
        myTransform = transform;
        animatorHandler = GetComponentInChildren<AnimatorHandler>();
        animatorHandler.Initialize();
    }
}
