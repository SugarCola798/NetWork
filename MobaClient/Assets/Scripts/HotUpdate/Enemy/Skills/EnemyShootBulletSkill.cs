using UnityEngine;

[CreateAssetMenu(fileName = "EnemyShootBulletSkill", menuName = "Game/Enemy/Skills/Shoot Bullet")]
public class EnemyShootBulletSkill : EnemySkillDefinition
{
    [SerializeField] private Bullet bulletPrefab;
    [SerializeField] private BulletData bulletDataOverride;
    [SerializeField, Min(0f)] private float muzzleForwardOffset = 0.5f;
    [SerializeField] private bool useTargetAsHomingTarget = true;

    public override bool CanCast(EnemyBase enemy, Transform target, float distanceToTarget)
    {
        if (!base.CanCast(enemy, target, distanceToTarget))
        {
            return false;
        }

        return bulletPrefab != null;
    }

    public override void Cast(EnemyBase enemy, Transform target)
    {
        if (enemy == null || target == null || bulletPrefab == null)
        {
            return;
        }

        Transform origin = enemy.AttackOrigin;
        Vector3 spawnPosition = origin.position + origin.forward * muzzleForwardOffset;
        Vector3 direction = (target.position - spawnPosition).normalized;

        Bullet bullet = Object.Instantiate(bulletPrefab, spawnPosition, Quaternion.LookRotation(direction, Vector3.up));
        Transform homingTarget = useTargetAsHomingTarget ? target : null;
        bullet.Initialize(bulletDataOverride, enemy.gameObject, spawnPosition, direction, homingTarget);
    }
}
