using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public InputHandler inputHandler;
    public Animator animator;

    private void Start()
    {
        inputHandler = GetComponent<InputHandler>();
        animator = GetComponentInChildren<Animator>();
    }

    private void Update()
    {
        inputHandler.isInteracting = animator.GetBool("isInteracting");
        inputHandler.rollFlag = false;
    }
}
