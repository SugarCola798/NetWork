using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoSingleton<PlayerController>
{
    private InputSystem inputSystem;
    
    public Vector2 moveInput; //移动输入
    public bool isJumping = false;
    public bool isAniming = false;
    public bool isSprint = false;

    protected override void Awake()
    {
        base.Awake();
        inputSystem = new InputSystem();
    }

    void Update()
    {
        #region 玩家输入相关
        moveInput = inputSystem.Player.Move.ReadValue<Vector2>();
        isJumping = inputSystem.Player.Jump.IsPressed();
        isAniming = inputSystem.Player.Aim.IsPressed();
        isSprint = inputSystem.Player.Sprint.IsPressed();
        #endregion
    }

    private void OnEnable()
    {
        inputSystem.Enable();
    }

    private void OnDisable()
    {
        inputSystem.Disable();
    }
}
