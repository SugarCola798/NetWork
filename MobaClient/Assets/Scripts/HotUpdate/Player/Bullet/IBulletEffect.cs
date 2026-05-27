using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IBulletEffect
{
    void Initialize(BulletData data);
    void OnUpdate(float deltaTime);
}
