using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyIdleState : EnemyState
{
    private EnemyBase enemyEntity;
    
    public EnemyIdleState(EnemyBase enemy) : base(enemy)
    {
        this.enemyEntity = enemy;
    }
    
    public override void Enter()
    {
        enemyEntity.PlayAnimation("Idle");
    }

    public override void Tick()
    {
        
    }
    
    public override void Exit()
    {
        
    }
}
