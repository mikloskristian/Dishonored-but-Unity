using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationStateController : MonoBehaviour
{
    Animator Animator;
    int IsWalkingHash;
    int IsRunningHash;

    void Start()
    {
        Animator = GetComponent<Animator>();
        IsWalkingHash = Animator.StringToHash("IsWalking");
        IsRunningHash = Animator.StringToHash("IsRunning");
    }

    void Update()
    {
        bool IsRunning = Animator.GetBool(IsRunningHash);
        bool IsWalking = Animator.GetBool(IsWalkingHash);
        bool PressedForward = Input.GetKey("w");
        bool RunPressed = Input.GetKey("left shift");

        if (!IsWalking && PressedForward)
        {
            Animator.SetBool(IsWalkingHash, true);
                
        }
        if (IsWalking && !PressedForward)
        {
            Animator.SetBool(IsWalkingHash, false);

        }

        if(!IsRunning && PressedForward && RunPressed)
        {
            Animator.SetBool(IsRunningHash, true);
        }

        if(IsRunning && !PressedForward || !RunPressed)
        {
            Animator.SetBool(IsRunningHash, false);
        }
    }
}
