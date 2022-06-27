using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class AnimatorHandler : MonoBehaviour
{
    #region VARIABLES

    public Animator anim;
    private int _vertical;
    private int _horizontal;
    public bool canRotate;

    #endregion

    public void Initialize()
    {
        anim = GetComponent<Animator>();
        _vertical = Animator.StringToHash("Vertical");
        _horizontal = Animator.StringToHash("Horizontal");
    }

    public void UpdateAnimatorValues(float verticalMovement, float horizontalMovement)
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

        anim.SetFloat(_vertical, v, 0.1f, Time.deltaTime);
        anim.SetFloat(_horizontal, h, 0.1f, Time.deltaTime);
    }

    public void CanRotate()
    {
        canRotate = true;
    }

    public void StopRotation()
    {
        canRotate = false;
    }
}
