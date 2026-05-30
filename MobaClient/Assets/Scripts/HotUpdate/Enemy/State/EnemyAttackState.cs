using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttackState : EnemyState
{
    private EnemyBase enemyEntity;
    public EnemyAttackState(EnemyBase enemy) : base(enemy)
    {
        enemyEntity = enemy;
    }
        
    public override void Enter()
    {
        
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Tick()
    {
        base.Tick();
    }
}
