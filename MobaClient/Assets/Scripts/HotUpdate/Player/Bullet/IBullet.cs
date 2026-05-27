using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IBullet
{
    void Initialize(BulletData data);
    void OnHit(BulletHitContext context);
    void OnUpdate(float deltaTime);
}
