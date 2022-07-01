using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public InputHandler inputHandler;
    public Animator animator;
    private CameraHandler _cameraHandler;
    private PlayerController _playerController;
    [HideInInspector] public bool isInteracting;
    [HideInInspector] public bool isSprinting;
    private static readonly int IsInteracting = Animator.StringToHash("isInteracting");


    private void Awake()
    {
        _cameraHandler = CameraHandler.singleton;
    }
    
    private void Start()
    {
        inputHandler = GetComponent<InputHandler>();
        animator = GetComponentInChildren<Animator>();
        _playerController = GetComponent<PlayerController>();
    }

    private void Update()
    {
        var delta = Time.deltaTime;

        isInteracting = animator.GetBool(IsInteracting);

        inputHandler.TickInput(delta);
        _playerController.HandleMovement(delta);
        _playerController.HandleRollingAndSprinting(delta);

    }
    
    private void FixedUpdate()
    {
        var delta = Time.fixedDeltaTime;

        if (_cameraHandler != null)
        {
            _cameraHandler.FollowTarget(delta);
            _cameraHandler.HandleCameraRotation(delta, inputHandler.mouseX, inputHandler.mouseY);
        }
    }

    private void LateUpdate()
    {
        inputHandler.rollFlag = false;
        inputHandler.sprintFlag = false;
        isSprinting = inputHandler.bInput;
 
    }
}
