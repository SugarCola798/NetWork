using UnityEngine;

public sealed class PlayerIdleState : PlayerState
{
    public PlayerIdleState(PlayerController controller) : base(controller)
    {
    }

    public override void Enter()
    {
        Debug.Log("Entering PlayerIdleState");
        Controller.PlayAnimation("Idle");
    }

    public override void Tick()
    {
        //Controller.SetSpeedBlend(0f);
    }
}
