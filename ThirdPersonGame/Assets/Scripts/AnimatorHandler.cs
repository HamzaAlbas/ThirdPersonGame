using UnityEngine;

public class AnimatorHandler : MonoBehaviour
{
    #region VARIABLES

    [HideInInspector] public Animator anim;
    private int _vertical;
    private int _horizontal;
    public bool canRotate;
    private InputHandler _inputHandler;
    private PlayerController _playerController;
    private PlayerManager _playerManager;
    private static readonly int IsInteracting = Animator.StringToHash("isInteracting");

    #endregion

    public void Initialize()
    {
        anim = GetComponent<Animator>();
        _inputHandler = GetComponentInParent<InputHandler>();
        _playerController = GetComponentInParent<PlayerController>();
        _vertical = Animator.StringToHash("Vertical");
        _horizontal = Animator.StringToHash("Horizontal");
        _playerManager = GetComponentInParent<PlayerManager>();
    }

    public void UpdateAnimatorValues(float verticalMovement, float horizontalMovement, bool isSprinting)
    {
        #region Vertical

        var v = verticalMovement switch
        {
            > 0 and < 0.55f => 0.5f,
            > 0.55f => 1,
            < 0 and > -0.55f => -0.5f,
            < -0.55f => -1f,
            _ => 0
        };

        #endregion

        #region Horizontal

        var h = horizontalMovement switch
        {
            > 0 and < 0.55f => 0.5f,
            > 0.55f => 1f,
            < 0 and > -0.55f => -0.55f,
            < -0.55f => -1f,
            _ => 0f
        };

        #endregion

        if (isSprinting)
        {
            v = 2;
            h = horizontalMovement;
        }

        anim.SetFloat(_vertical, v, 0.1f, Time.deltaTime);
        anim.SetFloat(_horizontal, h, 0.1f, Time.deltaTime);
    }

    public void PlayTargetAnimation(string targetAnim, bool isInteracting)
    {
        anim.applyRootMotion = isInteracting;
        anim.SetBool(IsInteracting, isInteracting);
        anim.CrossFade(targetAnim, 0.2f);
    }
    
    public void CanRotate()
    {
        canRotate = true;
    }

    public void StopRotation()
    {
        canRotate = false;
    }

    private void OnAnimatorMove()
    {
        if (_playerManager.isInteracting == false)
        {
            return;
        }

        var delta = Time.deltaTime;
        _playerController.rigidbody.drag = 0;
        var deltaPosition = anim.deltaPosition;
        deltaPosition.y = 0;
        var velocity = deltaPosition / delta;
        _playerController.rigidbody.velocity = velocity;
    }
}
