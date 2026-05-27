public interface IBulletMove
{
    void Initialize(BulletRuntimeContext context);
    void OnUpdate(BulletRuntimeContext context, float deltaTime);
    void OnHit(BulletHitContext context);
}
