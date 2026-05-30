using UnityEngine;

[CreateAssetMenu(fileName = "EnemyMeleeAttackSkill", menuName = "Game/Enemy/Skills/Melee Attack")]
public class EnemyMeleeAttackSkill : EnemySkillDefinition
{
    [SerializeField, Min(0f)] private float damage = 15f;

    public override void Cast(EnemyBase enemy, Transform target)
    {
        if (target == null)
        {
            return;
        }

        if (target.TryGetComponent<IDamageable>(out var damageable))
        {
            damageable.TakeDamage(damage, enemy.gameObject, target.position);
        }
    }
}
