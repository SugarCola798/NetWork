using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoSingleton<PlayerController>
{
    [Header("References")]
    [SerializeField] private Animator animator;

    [Header("Animator Parameters")]
    [SerializeField] private string speedParameter = "Speed";
    [SerializeField, Min(0f)] private float speedDampTime = 0.1f;
    [SerializeField, Min(0f)] private float walkBlend = 0.5f;
    [SerializeField, Min(0f)] private float runBlend = 1f;
    [SerializeField, Min(0f)] private float sprintBlend = 1.5f;
    [SerializeField, Min(0f)] private float moveThreshold = 0.1f;

    private InputSystem inputSystem;
    private StateMachine<PlayerStateType> stateMachine;
    private int speedHash;

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

        if (animator == null)
        {
            Debug.LogError("PlayerController requires an Animator reference.");
        }

        inputSystem = new InputSystem();
        speedHash = Animator.StringToHash(speedParameter);

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
}
