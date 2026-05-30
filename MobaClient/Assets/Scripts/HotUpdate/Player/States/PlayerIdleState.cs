using UnityEngine;

public sealed class PlayerIdleState : PlayerState
{
    public PlayerIdleState(PlayerController controller) : base(controller)
    {
    }

    public override void Enter()
    {
        Controller.PlayAnimation("Idle");
    }

    public override void Tick()
    {
        //Controller.SetSpeedBlend(0f);
    }
}
