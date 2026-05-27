using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour, IBullet
{
    [SerializeField] 
    private BulletData defaultData;
    private readonly RaycastHit[] raycastBuffer = new RaycastHit[8];
    private readonly HashSet<Collider> ignoredColliders = new HashSet<Collider>();

    private BulletRuntimeContext runtime;
    private IBulletMove moveStrategy;
    private List<IBulletEffect> effects;
    private bool isInitialized;

    public BulletData BulletData => defaultData;
    public void Initialize(BulletData data, GameObject owner, Vector3 origin, Vector3 direction, Transform homingTarget = null)
    {
        BulletData bulletData = data ?? defaultData;
        if(data != null)
        {
            bulletData = data;
        }

        if (bulletData == null)
        {
            Debug.LogError("BulletData is missing.");
            enabled = false;
            return;
        }

        transform.position = origin;
        transform.forward = direction.normalized;

        runtime = new BulletRuntimeContext
        {
            data = bulletData,
            bulletTransform = transform,
            owner = owner,
            homingTarget = homingTarget,
            direction = direction.normalized,
            previousPosition = origin,
            currentPosition = origin,
            remainingPierce = bulletData.pierceCount,
            remainingBounce = bulletData.bounceCount,
            aliveTime = 0f,
            travelDistance = 0f,
            beamTickTimer = 0f
        };

        moveStrategy = BulletStrategyFactory.CreateMove(bulletData.moveType);
        moveStrategy.Initialize(runtime);

        effects = BulletStrategyFactory.CreateEffects(bulletData.effectTypes);
        for (int i = 0; i < effects.Count; i++)
        {
            effects[i].Initialize(runtime);
        }

        ignoredColliders.Clear();
        if (owner != null)
        {
            Collider[] ownerColliders = owner.GetComponentsInChildren<Collider>();
            for (int i = 0; i < ownerColliders.Length; i++)
            {
                ignoredColliders.Add(ownerColliders[i]);
            }
        }

        isInitialized = true;
    }

    private void OnEnable()
    {
        if (defaultData != null && !isInitialized)
        {
            Initialize(defaultData, null, transform.position, transform.forward, null);
        }
    }

    private void Update()
    {
        OnUpdate(Time.deltaTime);
    }

    public void OnUpdate(float deltaTime)
    {
        if (!isInitialized || runtime == null)
        {
            return;
        }

        runtime.aliveTime += deltaTime;
        if (runtime.aliveTime >= runtime.data.lifeTime || runtime.travelDistance >= runtime.data.maxDistance)
        {
            Despawn();
            return;
        }

        moveStrategy.OnUpdate(runtime, deltaTime);

        for (int i = 0; i < effects.Count; i++)
        {
            effects[i].OnUpdate(runtime, deltaTime);
        }

        if (runtime.data.moveType == BulletMoveType.Beam)
        {
            if (runtime.beamTickTimer >= runtime.data.beamTickInterval)
            {
                runtime.beamTickTimer = 0f;
                TryProcessBeamHit();
            }
            return;
        }

        TryProcessProjectileHit();
    }

    public void OnHit(BulletHitContext context)
    {
        context.finalDamage = runtime.data.damage;

        for (int i = 0; i < effects.Count; i++)
        {
            effects[i].OnHit(context);
        }

        if (context.target != null && context.target.TryGetComponent<IDamageable>(out var damageable))
        {
            damageable.TakeDamage(context.finalDamage, runtime.owner, context.hitPosition);
        }

        moveStrategy.OnHit(context);

        if (!context.consumeBullet)
        {
            return;
        }

        if (runtime.remainingPierce > 0)
        {
            runtime.remainingPierce--;
            return;
        }

        Despawn();
    }

    private void TryProcessProjectileHit()
    {
        Vector3 castDir = runtime.currentPosition - runtime.previousPosition;
        float castDistance = castDir.magnitude;
        if (castDistance <= 0.0001f)
        {
            return;
        }

        castDir /= castDistance;
        int hitCount = Physics.SphereCastNonAlloc(
            runtime.previousPosition,
            runtime.data.collisionRadius,
            castDir,
            raycastBuffer,
            castDistance,
            runtime.data.hitMask,
            QueryTriggerInteraction.Collide);

        for (int i = 0; i < hitCount; i++)
        {
            RaycastHit hit = raycastBuffer[i];
            if (hit.collider == null || ignoredColliders.Contains(hit.collider))
            {
                continue;
            }

            var context = new BulletHitContext
            {
                runtime = runtime,
                hit = hit,
                target = hit.collider,
                hitPosition = hit.point,
                hitNormal = hit.normal
            };

            OnHit(context);
            return;
        }
    }

    private void TryProcessBeamHit()
    {
        if (!Physics.Raycast(
                runtime.bulletTransform.position,
                runtime.direction,
                out RaycastHit hit,
                runtime.data.beamLength,
                runtime.data.hitMask,
                QueryTriggerInteraction.Collide))
        {
            return;
        }

        if (hit.collider == null || ignoredColliders.Contains(hit.collider))
        {
            return;
        }

        var context = new BulletHitContext
        {
            runtime = runtime,
            hit = hit,
            target = hit.collider,
            hitPosition = hit.point,
            hitNormal = hit.normal
        };

        OnHit(context);
    }

    private void Despawn()
    {
        for (int i = 0; i < effects.Count; i++)
        {
            effects[i].OnDespawn(runtime);
        }

        isInitialized = false;
        Destroy(gameObject);
    }
}
