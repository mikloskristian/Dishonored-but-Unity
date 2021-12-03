using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMovement : MonoBehaviour
{
    //underrated legenda https://www.youtube.com/channel/UC26kmK523wCy9RziFrvhp7g

    public bool CanMove { get; private set; } = true;
    private bool _isSprinting => _canSprint && Input.GetKey(_sprintKey);
    private bool _shouldJump => Input.GetKeyDown(_jumpKey) && _characterController.isGrounded;
    private bool _shouldCrouch => Input.GetKeyDown(_crouchKey) && !_duringCrouchAnimation && _characterController.isGrounded;

    [Header("Functional Options")]
    [SerializeField] private bool _canSprint = true;
    [SerializeField] private bool _canJump = true;
    [SerializeField] private bool _canCrouch = true;
    [SerializeField] private bool _willSlideOnSlopes = true;

    [Header("Controls")]
    [SerializeField] private KeyCode _sprintKey = KeyCode.LeftShift;
    [SerializeField] private KeyCode _jumpKey = KeyCode.Space;
    [SerializeField] private KeyCode _crouchKey = KeyCode.LeftControl;

    [Header("Movement Parameters")]
    [SerializeField] private float _walkSpeed = 7.0f;
    [SerializeField] private float _sprintSpeed = 12.0f;
    [SerializeField] private float _crouchSpeed = 3.5f;
    [SerializeField] private float _slopeSpeed = 8.0f;

    [Header("Look Parameters")]
    [SerializeField, Range(1, 10)] private float _lookSpeedX = 2.0f;
    [SerializeField, Range(1, 10)] private float _lookSpeedY = 2.0f;
    [SerializeField, Range(1, 180)] private float _upperLockLimit = 80.0f;
    [SerializeField, Range(1, 180)] private float _lowerLockLimit = 80.0f;

    [Header("Jumping Parameters")]
    [SerializeField] private float _jumpForce = 8.0f;
    [SerializeField] private float _gravity = 30.0f;

    [Header("Crouching Parameters")]
    [SerializeField] private float _crouchingHeight = 0.5f;
    [SerializeField] private float _standingHeight = 2.0f;
    [SerializeField] private float _timeToCrouch = 0.25f;
    [SerializeField] private Vector3 _crouchingCenter = new Vector3(0, 0.5f, 0);
    [SerializeField] private Vector3 _standingCenter= new Vector3(0, 0, 0);
    private bool _isCrouching;
    private bool _duringCrouchAnimation;

    //SLAJDANJE MADAFAKA
    private Vector3 _hitPointNormal;
    private bool _isSliding
    {
        get
        {
            if(_characterController.isGrounded && Physics.Raycast(transform.position, Vector3.down, out RaycastHit SlopeHit, 2.0f))
            {
                _hitPointNormal = SlopeHit.normal;
                return Vector3.Angle(_hitPointNormal, Vector3.up) > _characterController.slopeLimit;
            }
            else
            {
                return false;
            }
        }
    }


    private Camera _playerCamera;
    private CharacterController _characterController;

    private Vector3 _moveDirection;
    private Vector2 _currentInput;

    private float rotationX = 0;

    void Start()
    {
        _playerCamera = GetComponentInChildren<Camera>();
        _characterController = GetComponent<CharacterController>();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        if (CanMove)
        {
            HandleMovementInput();
            HandleMouseLook();

            if (_canJump)
            {
                HandleJump();
            }

            if (_canCrouch)
            {
                HandleCrouch();
            }

            ApplyFinalMovement();
        }
    }

    private void HandleMovementInput()
    {
        _currentInput = new Vector2((_isCrouching ? _crouchSpeed : _isSprinting ? _sprintSpeed :  _walkSpeed) * Input.GetAxis("Vertical"), (_isCrouching ? _crouchSpeed : _isSprinting ? _sprintSpeed :  _walkSpeed) * Input.GetAxis("Horizontal"));

        float MoveDirectionY = _moveDirection.y;
        _moveDirection = (transform.TransformDirection(Vector3.forward) * _currentInput.x) + (transform.TransformDirection(Vector3.right) * _currentInput.y);
        _moveDirection.y = MoveDirectionY;
    }

    private void HandleMouseLook()
    {
        rotationX -= Input.GetAxis("Mouse Y") * _lookSpeedY;
        rotationX = Mathf.Clamp(rotationX, -_upperLockLimit, _lowerLockLimit);
        _playerCamera.transform.localRotation = Quaternion.Euler(rotationX, 0, 0);
        transform.localRotation *= Quaternion.Euler(0, Input.GetAxis("Mouse X") * _lookSpeedX, 0);
    }

    private void HandleJump()
    {
        if (_shouldJump)
        {
            _moveDirection.y = _jumpForce;
        }
    }

    private void HandleCrouch()
    {
        if (_shouldCrouch)
        {
            StartCoroutine(CrouchStand());
        }
    }

    private void ApplyFinalMovement()
    {
        if (!_characterController.isGrounded)
        {
            _moveDirection.y -= _gravity * Time.deltaTime;
        }

        if (_willSlideOnSlopes && _isSliding)
        {
            _moveDirection += new Vector3(_hitPointNormal.x, -_hitPointNormal.x, _hitPointNormal.z) * _slopeSpeed;
        }

        _characterController.Move(_moveDirection * Time.deltaTime);
    }

    private IEnumerator CrouchStand()
    {
        if(_isCrouching && Physics.Raycast(_playerCamera.transform.position, Vector3.up, 1.0f))
        {
            yield break;
        }

        _duringCrouchAnimation = true;

        float TimeElapsed = 0;
        float TargetHeight = _isCrouching ? _standingHeight : _crouchingHeight;
        float CurrentHeight = _characterController.height;
        Vector3 TargetCenter = _isCrouching ? _standingCenter : _crouchingCenter;
        Vector3 CurrentCenter = _characterController.center;

        while (TimeElapsed < _timeToCrouch)
        {
            _characterController.height = Mathf.Lerp(CurrentHeight, TargetHeight, TimeElapsed / _timeToCrouch);
            _characterController.center = Vector3.Lerp(CurrentCenter, TargetCenter, TimeElapsed / _timeToCrouch);
            TimeElapsed += Time.deltaTime;
            yield return null;
        }

        _characterController.height = TargetHeight;
        _characterController.center = TargetCenter;

        _isCrouching = !_isCrouching;

        _duringCrouchAnimation = false;
    }
}
