using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonEnemy : EnemyBase
{
    public override void BuildStateMachine()
    {
        base.BuildStateMachine();
        StateMachines.AddState(EnemyStateType.Idle, new EnemyIdleState(this));
        StateMachines.AddState(EnemyStateType.Move, new EnemyWalkState(this));
        StateMachines.AddState(EnemyStateType.Attack, new EnemyAttackState(this));
        
        StateMachines.SetInitialState(EnemyStateType.Idle);
    }

    protected override void Update()
    {
        base.Update();
    }

    public override void ApplySlow(float slowPercent, float duration)
    {
        base.ApplySlow(slowPercent, duration);
    }

    public override void TakeDamage(float amount, GameObject source, Vector3 hitPoint)
    {
        base.TakeDamage(amount, source, hitPoint);
    }
}
