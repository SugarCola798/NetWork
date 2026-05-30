using UnityEngine;

public abstract class EnemySkillDefinition : ScriptableObject
{
    [SerializeField, Min(0f)] private float cooldown = 1f;
    [SerializeField, Min(0f)] private float castRange = 2f;
    [SerializeField] private int priority = 0;
    [SerializeField] private string animationName = "";
    public float Cooldown => cooldown;
    public float CastRange => castRange;
    public int Priority => priority;
    
    public string SkillAnimName => animationName;

    public virtual bool CanCast(EnemyBase enemy, Transform target, float distanceToTarget)
    {
        if (enemy == null || target == null)
        {
            return false;
        }

        return distanceToTarget <= castRange;
    }

    public abstract void Cast(EnemyBase enemy, Transform target);
}
