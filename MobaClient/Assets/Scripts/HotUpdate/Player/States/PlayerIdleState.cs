public sealed class PlayerIdleState : PlayerState
{
    public PlayerIdleState(PlayerController controller) : base(controller)
    {
    }

    public override void Enter()
    {
        Controller.SetSpeedBlend(0f);
    }

    public override void Tick()
    {
        Controller.SetSpeedBlend(0f);
    }
}
