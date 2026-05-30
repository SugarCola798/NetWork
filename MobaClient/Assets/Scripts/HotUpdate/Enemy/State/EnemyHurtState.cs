using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHurtState : EnemyState
{
    private EnemyBase enemyEntity;
    public EnemyHurtState(EnemyBase enemy) : base(enemy)
    {
        this.enemyEntity = enemy;
    }
    
    public override void Enter()
    {
        
    }

    public override void Tick()
    {
        base.Tick();
    }

    public override void Exit()
    {
        base.Exit();
    }
}
