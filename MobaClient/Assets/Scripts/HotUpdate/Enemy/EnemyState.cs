using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyState : IState
{
    public EnemyBase EnemyEntity;
    
    public EnemyState(EnemyBase enemyBase)
    {
        EnemyEntity = enemyBase;
    }

    public virtual void Enter()
    {
        
    }

    public virtual void Exit()
    {
        
    }

    public virtual void Tick()
    {
        
    }
}
