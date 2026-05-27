public class BulletSlowEffect : IBulletEffect
{
    public void Initialize(BulletRuntimeContext context) { }

    public void OnUpdate(BulletRuntimeContext context, float deltaTime) { }

    public void OnHit(BulletHitContext context)
    {
        if (context.target != null && context.target.TryGetComponent<ISlowable>(out var slowable))
        {
            slowable.ApplySlow(context.runtime.data.slowPercent, context.runtime.data.slowDuration);
        }
    }

    public void OnDespawn(BulletRuntimeContext context) { }
}
