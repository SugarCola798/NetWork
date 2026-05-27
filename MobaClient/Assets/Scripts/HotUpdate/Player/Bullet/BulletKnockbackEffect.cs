using UnityEngine;

public class BulletKnockbackEffect : IBulletEffect
{
    public void Initialize(BulletRuntimeContext context) { }

    public void OnUpdate(BulletRuntimeContext context, float deltaTime) { }

    public void OnHit(BulletHitContext context)
    {
        if (context.target == null)
        {
            return;
        }

        Rigidbody rb = context.target.attachedRigidbody;
        if (rb == null)
        {
            return;
        }

        Vector3 pushDirection = context.runtime.direction;
        pushDirection.y = 0f;
        if (pushDirection.sqrMagnitude <= 0.0001f)
        {
            pushDirection = context.hitNormal;
        }

        rb.AddForce(pushDirection.normalized * context.runtime.data.knockbackForce, ForceMode.Impulse);
    }

    public void OnDespawn(BulletRuntimeContext context) { }
}
