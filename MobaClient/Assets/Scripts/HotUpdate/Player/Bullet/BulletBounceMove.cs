using UnityEngine;

public class BulletBounceMove : IBulletMove
{
    public void Initialize(BulletRuntimeContext context) { }

    public void OnUpdate(BulletRuntimeContext context, float deltaTime)
    {
        float distance = context.data.speed * deltaTime;
        context.previousPosition = context.bulletTransform.position;
        context.bulletTransform.position += context.direction * distance;
        context.currentPosition = context.bulletTransform.position;
        context.travelDistance += distance;
        context.bulletTransform.forward = context.direction;
    }

    public void OnHit(BulletHitContext context)
    {
        var runtime = context.runtime;
        if (runtime.remainingBounce <= 0)
        {
            return;
        }

        runtime.remainingBounce--;
        runtime.direction = Vector3.Reflect(runtime.direction, context.hitNormal).normalized;
        runtime.bulletTransform.forward = runtime.direction;
        runtime.bulletTransform.position = context.hitPosition + context.hitNormal * runtime.data.collisionRadius;
        context.consumeBullet = false;
    }
}
