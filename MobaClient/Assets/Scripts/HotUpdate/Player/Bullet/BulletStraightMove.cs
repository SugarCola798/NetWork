public class BulletStraightMove : IBulletMove
{
    public void Initialize(BulletRuntimeContext context)
    {
    }

    public void OnUpdate(BulletRuntimeContext context, float deltaTime)
    {
        float distance = context.data.speed * deltaTime;
        context.previousPosition = context.bulletTransform.position;
        context.bulletTransform.position += context.direction * distance;
        context.currentPosition = context.bulletTransform.position;
        context.travelDistance += distance;
    }

    public void OnHit(BulletHitContext context) {
        PlayerController.Instance.PlayHitEffect(context.hitPosition);
     }
}
