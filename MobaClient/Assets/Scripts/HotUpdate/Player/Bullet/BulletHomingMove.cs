using UnityEngine;

public class BulletHomingMove : IBulletMove
{
    public void Initialize(BulletRuntimeContext context) { }

    public void OnUpdate(BulletRuntimeContext context, float deltaTime)
    {
        if (context.homingTarget != null)
        {
            Vector3 toTarget = context.homingTarget.position - context.bulletTransform.position;
            if (toTarget.sqrMagnitude > 0.0001f)
            {
                Vector3 targetDir = toTarget.normalized;
                float maxRadians = context.data.homingTurnSpeed * Mathf.Deg2Rad * deltaTime;
                context.direction = Vector3.RotateTowards(context.direction, targetDir, maxRadians, 0f).normalized;
            }
        }

        float distance = context.data.speed * deltaTime;
        context.previousPosition = context.bulletTransform.position;
        context.bulletTransform.position += context.direction * distance;
        context.currentPosition = context.bulletTransform.position;
        context.travelDistance += distance;
        context.bulletTransform.forward = context.direction;
    }

    public void OnHit(BulletHitContext context) { }
}
