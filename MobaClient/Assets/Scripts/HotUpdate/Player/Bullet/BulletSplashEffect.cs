using UnityEngine;

public class BulletSplashEffect : IBulletEffect
{
    private static readonly Collider[] HitBuffer = new Collider[32];

    public void Initialize(BulletRuntimeContext context) { }

    public void OnUpdate(BulletRuntimeContext context, float deltaTime) { }

    public void OnHit(BulletHitContext context)
    {
        BulletData data = context.runtime.data;
        if (data.splashRadius <= 0f)
        {
            return;
        }

        int count = Physics.OverlapSphereNonAlloc(context.hitPosition, data.splashRadius, HitBuffer, data.hitMask);
        for (int i = 0; i < count; i++)
        {
            Collider collider = HitBuffer[i];
            if (collider == null || collider == context.target)
            {
                continue;
            }

            if (context.runtime.owner != null && collider.transform.root == context.runtime.owner.transform.root)
            {
                continue;
            }

            if (collider.TryGetComponent<IDamageable>(out var damageable))
            {
                float splashDamage = context.runtime.data.damage * data.splashDamageScale;
                damageable.TakeDamage(splashDamage, context.runtime.owner, context.hitPosition);
            }
        }
    }

    public void OnDespawn(BulletRuntimeContext context) { }
}
