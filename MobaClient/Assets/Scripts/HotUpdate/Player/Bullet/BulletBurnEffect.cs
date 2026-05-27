public class BulletBurnEffect : IBulletEffect
{
    public void Initialize(BulletRuntimeContext context) { }

    public void OnUpdate(BulletRuntimeContext context, float deltaTime) { }

    public void OnHit(BulletHitContext context)
    {
        context.finalDamage += context.runtime.data.burnBonusDamage;
    }

    public void OnDespawn(BulletRuntimeContext context) { }
}
