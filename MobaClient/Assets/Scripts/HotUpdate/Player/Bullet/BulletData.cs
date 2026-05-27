using System.Collections.Generic;
using UnityEngine;

public enum BulletMoveType
{
    Straight = 0,
    Homing,
    Bounce,
    Beam
}

public enum BulletEffectType
{
    Normal = 0,
    Burn,
    Slow,
    Knockback,
    Splash
}

[System.Serializable]
public class BulletData
{
    [Header("Base")]
    public float speed;
    public float damage;
    public float lifeTime = 3f;
    public float maxDistance = 50f;
    public float collisionRadius = 0.1f;
    public LayerMask hitMask = ~0;
    public BulletMoveType moveType = BulletMoveType.Straight;
    public List<BulletEffectType> effectTypes = new List<BulletEffectType>() { BulletEffectType.Normal };

    [Header("Pierce/Bounce")]
    public int pierceCount = 0;
    public int bounceCount = 0;

    [Header("Homing")]
    public float homingTurnSpeed = 360f;

    [Header("Beam")]
    public float beamLength = 60f;
    public float beamTickInterval = 0.15f;

    [Header("Effects")]
    public float burnBonusDamage = 3f;
    public float slowPercent = 0.3f;
    public float slowDuration = 1.5f;
    public float knockbackForce = 6f;
    public float splashRadius = 3f;
    public float splashDamageScale = 0.5f;
}

public class BulletHitContext
{
    public BulletRuntimeContext runtime;
    public RaycastHit hit;
    public Collider target;
    public Vector3 hitPosition;
    public Vector3 hitNormal;
    public float finalDamage;
    public bool consumeBullet = true;
}

public class BulletRuntimeContext
{
    public BulletData data;
    public Transform bulletTransform;
    public GameObject owner;
    public Transform homingTarget;
    public Vector3 direction;
    public Vector3 previousPosition;
    public Vector3 currentPosition;
    public float aliveTime;
    public float travelDistance;
    public int remainingPierce;
    public int remainingBounce;
    public float beamTickTimer;
}

public interface IDamageable
{
    void TakeDamage(float amount, GameObject source, Vector3 hitPoint);
}

public interface ISlowable
{
    void ApplySlow(float slowPercent, float duration);
}
