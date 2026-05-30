using UnityEngine;
using System.Collections.Generic;

public class EnemyBase : MonoBehaviour, IDamageable, ISlowable
{
    [Header("Config")]
    [SerializeField] private EnemyDefinition definition;

    [Header("References")]
    [SerializeField] private Transform attackOrigin;
    [SerializeField]
    private Animator animator;
    [SerializeField]
    private UnityEngine.AI.NavMeshAgent navMeshAgent;
    
    private readonly List<EnemySkillRuntime> skillRuntimes = new List<EnemySkillRuntime>();
    private Transform currentTarget;
    private float currentHp;
    private float slowMultiplier = 1f;
    private float slowEndTime;

    public EnemyDefinition Definition => definition;
    public Transform AttackOrigin => attackOrigin != null ? attackOrigin : transform;
    public Transform CurrentTarget => currentTarget;

    public StateMachine<EnemyStateType> StateMachines;
    private void Awake()
    {
        if (definition == null)
        {
            Debug.LogError($"{name} is missing EnemyDefinition.");
            enabled = false;
            return;
        }
        
        currentHp = definition.MaxHp;
        BuildSkillRuntime();
        BuildStateMachine();
    }

    public virtual void BuildStateMachine()
    {
        StateMachines = new StateMachine<EnemyStateType>();
    }

    protected virtual void Update()
    {
        UpdateSlowState();
        AcquireTarget();
    }

    public virtual void TakeDamage(float amount, GameObject source, Vector3 hitPoint)
    {
        currentHp -= amount;
        if (currentHp <= 0f)
        {
            Die();
        }
    }

    public virtual void ChangeState(EnemyStateType stateId)
    {
        StateMachines.ChangeState(stateId);
    }

    public void PlayAnimation(string animationName)
    {
        animator.Play(animationName);
    }

    public virtual void ApplySlow(float slowPercent, float duration)
    {
        float clamped = Mathf.Clamp01(1f - Mathf.Clamp01(slowPercent));
        slowMultiplier = Mathf.Min(slowMultiplier, clamped);
        slowEndTime = Mathf.Max(slowEndTime, Time.time + Mathf.Max(0f, duration));
    }

    public bool IsSkillReady(EnemySkillRuntime runtime)
    {
        return true;
        //return runtime.NextReadyTime <= Time.time;
    }

    public void MarkSkillUsed(EnemySkillRuntime runtime)
    {
        runtime.NextReadyTime = Time.time + runtime.Skill.Cooldown;
    }

    private void BuildSkillRuntime()
    {
        skillRuntimes.Clear();
        if (definition.Skills == null)
        {
            return;
        }

        for (int i = 0; i < definition.Skills.Count; i++)
        {
            EnemySkillDefinition skill = definition.Skills[i];
            if (skill == null)
            {
                continue;
            }

            skillRuntimes.Add(new EnemySkillRuntime(skill));
        }

        skillRuntimes.Sort((a, b) => b.Skill.Priority.CompareTo(a.Skill.Priority));
    }

    private void AcquireTarget()
    {
        if (PlayerController.Instance == null)
        {
            currentTarget = null;
            return;
        }
        
        Transform player = PlayerController.Instance.curControlPlayer;
        currentTarget = player;
        Debug.Log($"Looking at {currentTarget.name}");
        float sqrDistance = (player.position - transform.position).sqrMagnitude;
        float aggroSqr = definition.AggroRange * definition.AggroRange;
        if (sqrDistance <= aggroSqr)
        {
            TryUseSkill();
        }
        else
        {
            MoveToTarget();
        }
    }

    private bool TryUseSkill()
    {
        for (int i = 0; i < skillRuntimes.Count; i++)
        {
            EnemySkillRuntime runtime = skillRuntimes[i];
            if (!IsSkillReady(runtime))
            {
                continue;
            }
            
            ChangeState(EnemyStateType.Attack);
            this.PlaySkillAnimation(runtime);
            runtime.Skill.Cast(this, currentTarget);
            MarkSkillUsed(runtime);
            return true;
        }

        return false;
    }

    public virtual void PlaySkillAnimation(EnemySkillRuntime skillRuntime)
    {
        var animName = skillRuntime.Skill.SkillAnimName;
        if (!string.IsNullOrEmpty(animName))
        {
            animator.Play(animName);
        }
    }

    public virtual void PlayAnimation(EnemyStateType state)
    {
        var anim = "";
        switch (state)
        {
            case EnemyStateType.Idle:
                anim = "idle";
                break;
            case EnemyStateType.Move:
                anim = "MoveBlendTree";
                break;
        }

        if (!string.IsNullOrEmpty(anim))
        {
            animator.Play(anim);
        }
    }

    private void MoveToTarget()
    {
        ChangeState(EnemyStateType.Move);
        Vector3 direction = (currentTarget.position - transform.position);
        direction.y = 0f;
        if (direction.sqrMagnitude <= 0.0001f)
        {
            return;
        }

        direction.Normalize();

        Quaternion targetRotation = Quaternion.LookRotation(direction, Vector3.up);
        transform.rotation = Quaternion.RotateTowards(
            transform.rotation,
            targetRotation,
            definition.RotationSpeed * Time.deltaTime);

        float moveSpeed = definition.MoveSpeed * slowMultiplier;
        navMeshAgent.speed = moveSpeed;
        navMeshAgent.SetDestination(currentTarget.position);
       // transform.position += direction * moveSpeed * Time.deltaTime;
    }

    private void UpdateSlowState()
    {
        if (slowMultiplier < 1f && Time.time >= slowEndTime)
        {
            slowMultiplier = 1f;
        }
    }

    private void Die()
    {
        Destroy(gameObject);
    }
}

public class EnemySkillRuntime
{
    public EnemySkillRuntime(EnemySkillDefinition skill)
    {
        Skill = skill;
    }

    public EnemySkillDefinition Skill { get; }
    public float NextReadyTime { get; set; }
}
