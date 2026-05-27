using UnityEngine;

public interface IBullet
{
    void Initialize(BulletData data, GameObject owner, Vector3 origin, Vector3 direction, Transform homingTarget = null);
    void OnHit(BulletHitContext context);
    void OnUpdate(float deltaTime);
}
