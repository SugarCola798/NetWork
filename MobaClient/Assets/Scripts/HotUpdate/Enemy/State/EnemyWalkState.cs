using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyWalkState : EnemyState
{

    private EnemyBase enemy;
    public EnemyWalkState(EnemyBase enemyBase) : base(enemyBase)
    {
        this.enemy = enemyBase;
    }
    
    public override void Enter()
    {
        enemy.PlayAnimation(EnemyStateType.Move);
    }

    public override void Tick()
    {
        base.Tick();
    }

    public override void Exit()
    {
        
    }
}
