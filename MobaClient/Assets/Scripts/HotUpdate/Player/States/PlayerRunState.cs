public sealed class PlayerRunState : PlayerState
{
    public PlayerRunState(PlayerController controller) : base(controller)
    {
    }

    public override void Tick()
    {
        Controller.SetSpeedBlend(Controller.GetMoveBlendValue());
    }
}
