using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoSingleton<PlayerController>
{
    [Header("References")]
    [SerializeField] private Animator animator;
    [SerializeField] private CharacterController characterController;
    [SerializeField] private Transform cameraTransform;
    [SerializeField] private bool preferMainCameraForDirection = true;

    [Header("Movement")]
    [SerializeField, Min(0f)] private float moveSpeed = 3.5f;
    [SerializeField, Min(1f)] private float sprintMultiplier = 1.5f;
    [SerializeField, Min(0f)] private float rotationSpeed = 720f;

    [Header("Animator Parameters")]
    [SerializeField] private string speedParameter = "MoveBlend";
    [SerializeField] private bool useDirectionalBlend = true;
    [SerializeField] private string forwardParameter = "MoveForward";
    [SerializeField] private string strafeParameter = "MoveRight";
    [SerializeField, Min(0f)] private float speedDampTime;
    [SerializeField, Min(0f)] private float walkBlend;
    [SerializeField, Min(0f)] private float runBlend;
    [SerializeField, Min(0f)] private float sprintBlend;
    [SerializeField, Min(0f)] private float moveThreshold;

    private InputSystem inputSystem;
    private StateMachine<PlayerStateType> stateMachine;
    private int speedHash;
    private int forwardHash;
    private int strafeHash;

    public Vector2 MoveInput { get; private set; }
    public bool IsJumping { get; private set; }
    public bool IsAiming { get; private set; }
    public bool IsSprint { get; private set; }
    public bool HasMoveInput => MoveInput.sqrMagnitude > moveThreshold * moveThreshold;

    protected override void Awake()
    {
        base.Awake();

        if (animator == null)
        {
            animator = GetComponentInChildren<Animator>();
        }
        
        if (characterController == null)
        {
            characterController = GetComponent<CharacterController>();
        }

        if (cameraTransform == null && Camera.main != null)
        {
            cameraTransform = Camera.main.transform;
        }

        if (animator == null)
        {
            Debug.LogError("PlayerController requires an Animator reference.");
        }

        inputSystem = new InputSystem();
        speedHash = Animator.StringToHash(speedParameter);
        forwardHash = Animator.StringToHash(forwardParameter);
        strafeHash = Animator.StringToHash(strafeParameter);

        BuildStateMachine();
    }

    void Update()
    {
        #region 玩家输入相关
        MoveInput = inputSystem.Player.Move.ReadValue<Vector2>();
        IsJumping = inputSystem.Player.Jump.IsPressed();
        IsAiming = inputSystem.Player.Aim.IsPressed();
        IsSprint = inputSystem.Player.Sprint.IsPressed();
        #endregion

        HandleMovement();
        stateMachine?.Tick();
    }

    private void OnEnable()
    {
        inputSystem.Enable();
        stateMachine?.SetInitialState(PlayerStateType.Idle);
    }

    private void OnDisable()
    {
        inputSystem.Disable();
    }

    private void OnDestroy()
    {
        inputSystem?.Dispose();
    }

    private void BuildStateMachine()
    {
        stateMachine = new StateMachine<PlayerStateType>();

        var idleState = new PlayerIdleState(this);
        var runState = new PlayerRunState(this);

        stateMachine.AddState(PlayerStateType.Idle, idleState);
        stateMachine.AddState(PlayerStateType.Run, runState);

        stateMachine.AddTransition(PlayerStateType.Idle, PlayerStateType.Run, () => HasMoveInput);
        stateMachine.AddTransition(PlayerStateType.Run, PlayerStateType.Idle, () => !HasMoveInput);
    }

    public float GetMoveBlendValue()
    {
        var moveMagnitude = Mathf.Clamp01(MoveInput.magnitude);
        var maxBlend = IsSprint ? sprintBlend : runBlend;
        return moveMagnitude * Mathf.Max(walkBlend, maxBlend);
    }

    public void SetSpeedBlend(float targetBlend)
    {
        if (animator == null)
        {
            return;
        }
        
        animator.SetFloat(speedHash, targetBlend, speedDampTime, Time.deltaTime);
    }

    public void PlayAnimation(string animationName, float transitionTime = 0.25f, int layer = 0)
    {
        animator.CrossFadeInFixedTime(animationName, transitionTime);
    }

    private void HandleMovement()
    {
        float deltaTime = Time.deltaTime;
        Vector2 input = MoveInput;

        if (input.sqrMagnitude <= moveThreshold * moveThreshold)
        {
            UpdateDirectionalBlend(Vector3.zero, deltaTime);
            return;
        }

        Transform currentCamera = ResolveCameraTransform();
        Vector3 cameraForward = currentCamera != null ? currentCamera.forward : Vector3.forward;
        Vector3 cameraRight = currentCamera != null ? currentCamera.right : Vector3.right;

        cameraForward.y = 0f;
        cameraRight.y = 0f;
        cameraForward.Normalize();
        cameraRight.Normalize();

        Vector3 moveDirection = cameraForward * input.y + cameraRight * input.x;
        moveDirection.Normalize();
        UpdateDirectionalBlend(moveDirection, deltaTime);

        Quaternion targetRotation = Quaternion.LookRotation(moveDirection, Vector3.up);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed * deltaTime);

        float currentSpeed = IsSprint ? moveSpeed * sprintMultiplier : moveSpeed;
        Vector3 velocity = moveDirection * currentSpeed;

        if (characterController != null)
        {
            characterController.Move(velocity * deltaTime);
            return;
        }

        transform.position += velocity * deltaTime;
    }

    private void UpdateDirectionalBlend(Vector3 moveDirection, float deltaTime)
    {
        if (!useDirectionalBlend || animator == null)
        {
            return;
        }

        Vector3 localDirection = transform.InverseTransformDirection(moveDirection);
        animator.SetFloat(forwardHash, localDirection.z, speedDampTime, deltaTime);
        animator.SetFloat(strafeHash, localDirection.x, speedDampTime, deltaTime);
    }

    private Transform ResolveCameraTransform()
    {
        if (preferMainCameraForDirection && Camera.main != null)
        {
            return Camera.main.transform;
        }

        if (cameraTransform != null)
        {
            return cameraTransform;
        }

        if (Camera.main != null)
        {
            cameraTransform = Camera.main.transform;
            return cameraTransform;
        }

        return null;
    }
}
