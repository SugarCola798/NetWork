using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoSingleton<EnemyController>
{
    [SerializeField] private List<EnemyBase> enemyPrefabs;
    public Dictionary<int, EnemyBase> Enemies = new Dictionary<int, EnemyBase>();

    protected override void Awake()
    {
        base.Awake();
        InitializeEnemy();
    }

    public void InitializeEnemy()
    {
        ClearEnemies();
        for (int i = 0; i < enemyPrefabs.Count; i++)
        {
            var index = Random.Range(0, enemyPrefabs.Count);
            var enemy = Instantiate(enemyPrefabs[index], transform);
            var randPosX = Random.Range(-3, 3);
            var randPosY = Random.Range(-3, 3);
            enemy.transform.position = new Vector3(randPosX, 0, randPosY);
            Enemies.Add(i, enemy);
        }
    }

    public void RemoveEnemy(int id)
    {
        if (Enemies.TryGetValue(id, out var e))
        {
            Destroy(e.gameObject);
        }
    }
    
    public void ClearEnemies()
    {
        for (int i = 0; i < Enemies.Count; i++)
        {
            RemoveEnemy(i);
        }
        Enemies.Clear();
    }
}
