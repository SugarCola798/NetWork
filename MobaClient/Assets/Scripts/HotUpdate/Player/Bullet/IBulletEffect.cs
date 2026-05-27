public interface IBulletEffect
{
    void Initialize(BulletRuntimeContext context);
    void OnUpdate(BulletRuntimeContext context, float deltaTime);
    void OnHit(BulletHitContext context);
    void OnDespawn(BulletRuntimeContext context);
}
