public abstract class PlayerState : IState
{
    protected PlayerState(PlayerController controller)
    {
        Controller = controller;
    }

    protected PlayerController Controller { get; }

    public virtual void Enter()
    {
    }

    public virtual void Exit()
    {
    }

    public virtual void Tick()
    {
    }
}
