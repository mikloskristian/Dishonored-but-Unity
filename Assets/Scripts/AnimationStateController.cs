using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationStateController : MonoBehaviour
{
    Animator Animator;
    [SerializeField] private GameObject _playerModel;
    [SerializeField] private Camera _camera;
    int IsWalkingHash;
    int IsRunningHash;
    int IsCrouchingHash;
    int IsCrouchWalkingHash;

    void Start()
    {
        Animator = GetComponent<Animator>();
        IsWalkingHash = Animator.StringToHash("IsWalking");
        IsRunningHash = Animator.StringToHash("IsRunning");
        IsCrouchingHash = Animator.StringToHash("IsCrouching");
        IsCrouchWalkingHash = Animator.StringToHash("IsCrouchWalking");
    }

    void Update()
    {
        bool IsRunning = Animator.GetBool(IsRunningHash);
        bool IsWalking = Animator.GetBool(IsWalkingHash);
        bool IsCrouching = Animator.GetBool(IsCrouchingHash);
        bool IsCrouchWalking = Animator.GetBool(IsCrouchWalkingHash);
        bool PressedForward = Input.GetKey("w");
        bool RunPressed = Input.GetKey("left shift");
        bool CrouchPressed = Input.GetKeyUp("left ctrl");

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
        
        if (!IsCrouching && CrouchPressed)
        {
            StartCoroutine(CrouchDown());
        }

        if(IsCrouching && !IsCrouchWalking && PressedForward)
        {
            StartCoroutine(CrouchWalking());
        }
        if(IsCrouching && IsCrouchWalking && !PressedForward)
        {
            StartCoroutine(CrouchWalkingStop());
        }

        if (IsCrouching && CrouchPressed)
        {
            StartCoroutine(CrouchUp());
        }
        
        IEnumerator CrouchDown()
        {
            Animator.SetBool(IsCrouchingHash, true);
            _playerModel.transform.position += transform.TransformDirection(Vector3.back) * 0.2f + transform.TransformDirection(Vector3.up) * 0.7f;
            yield return new WaitForSeconds(2);
        }
       
        IEnumerator CrouchUp()
        {
            Animator.SetBool(IsCrouchingHash, false);
            _playerModel.transform.position -= transform.TransformDirection(Vector3.back) * 0.2f + transform.TransformDirection(Vector3.up) * 0.7f;
            yield return new WaitForSeconds(2);
        }

        IEnumerator CrouchWalking()
        {
            Animator.SetBool(IsCrouchWalkingHash, true);
            _camera.transform.position += new Vector3(0, 0.17f, 0);
            yield return 0;
        }
        IEnumerator CrouchWalkingStop()
        {
            Animator.SetBool(IsCrouchWalkingHash, false);
            _camera.transform.position -= new Vector3(0, 0.17f, 0);
            yield return 0;
        }
    }
}
