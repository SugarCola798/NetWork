using UnityEngine;

public sealed class PlayerRunState : PlayerState
{
    public PlayerRunState(PlayerController controller) : base(controller)
    {
    }

    public override void Enter()
    {
        base.Enter();
        Controller.PlayAnimation("Move");
    }

    public override void Tick()
    {
        var blendValue = Controller.GetMoveBlendValue();
        Controller.SetSpeedBlend(blendValue);
    }
}
