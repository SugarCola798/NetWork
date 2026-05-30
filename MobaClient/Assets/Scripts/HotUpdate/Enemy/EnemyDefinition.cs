using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EnemyDefinition", menuName = "Game/Enemy/Enemy Definition")]
public class EnemyDefinition : ScriptableObject
{
    [Header("Base Stats")]
    [SerializeField, Min(1f)] private float maxHp = 100f;
    [SerializeField, Min(0f)] private float moveSpeed = 2.5f;
    [SerializeField, Min(0f)] private float rotationSpeed = 360f;
    [SerializeField, Min(0f)] private float aggroRange = 18f;
    [SerializeField, Min(0f)] private float stopDistance = 1.8f;

    [Header("Skill Loadout")]
    [SerializeField] private List<EnemySkillDefinition> skills = new List<EnemySkillDefinition>();

    public float MaxHp => maxHp;
    public float MoveSpeed => moveSpeed;
    public float RotationSpeed => rotationSpeed;
    public float AggroRange => aggroRange;
    public float StopDistance => stopDistance;
    public IReadOnlyList<EnemySkillDefinition> Skills => skills;
}
