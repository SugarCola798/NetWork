using UnityEngine;

public class BulletBeamMove : IBulletMove
{
    public void Initialize(BulletRuntimeContext context)
    {
        context.beamTickTimer = 0f;
    }

    public void OnUpdate(BulletRuntimeContext context, float deltaTime)
    {
        context.previousPosition = context.bulletTransform.position;
        context.currentPosition = context.bulletTransform.position;
        context.beamTickTimer += deltaTime;
        context.travelDistance += context.data.speed * deltaTime;
    }

    public void OnHit(BulletHitContext context)
    {
        // Beam keeps ticking until lifetime/maxDistance condition ends it.
        context.consumeBullet = false;
    }
}
