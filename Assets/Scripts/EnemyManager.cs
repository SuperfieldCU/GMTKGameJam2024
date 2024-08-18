using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static EnemyMovement;

public class EnemyManager : Singleton<EnemyManager>
{
    [SerializeField]
    private int enemiesPerPosition = 3;

    [SerializeField]
    private float assignPositionDelay = 0.5f;

    private Dictionary<EAttackDirections, List<EnemyMovement>> engagedEnemies = new Dictionary<EAttackDirections, List<EnemyMovement>>();

    private List<EnemyMovement> enemyQueue = new List<EnemyMovement>();
    
    // Start is called before the first frame update
    protected override void Awake()
    {
        base.Awake();
        foreach (EAttackDirections attackDir in Enum.GetValues(typeof(EAttackDirections)))
        {
            if (attackDir != EAttackDirections.Null)
                continue;
            engagedEnemies.Add(attackDir, new List<EnemyMovement>());
        }
    }

    public void AddEnemy(EnemyMovement enemy)
    {
        enemyQueue.Add(enemy);
    }

    public void UpdateEnemies()
    {
        EmptyEngaged(EAttackDirections.MeleeR);
        EmptyEngaged(EAttackDirections.MeleeL);
        List<EnemyMovement> leftEnemies = new List<EnemyMovement>();
        List<EnemyMovement> rightEnemies = new List<EnemyMovement>();
        for (int i = 0; i < enemyQueue.Count; i++)
        {
            if (!enemyQueue[i].IsWithinRange())
            {
                continue;
            }
            if (enemyQueue[i].IsRightOfTarget())
            {
                rightEnemies.Add(enemyQueue[i]);
            }
            else
            {
                leftEnemies.Add(enemyQueue[i]);
            }
        }
    } 

    private void EmptyEngaged(EAttackDirections attackDir)
    {
        for (int i = 0; i < engagedEnemies[attackDir].Count; i++)
        {
            enemyQueue.Add(engagedEnemies[attackDir][i]);
            engagedEnemies[attackDir].RemoveAt(i);
            i--;
        }
    }

    public void RemoveEnemy(EnemyMovement enemy)
    {
        if (engagedEnemies[enemy.GetAttackDirection()].Contains(enemy))
        {
            engagedEnemies[enemy.GetAttackDirection()].Remove(enemy);
        }
        else if (enemyQueue.Contains(enemy))
        {
            enemyQueue.Remove(enemy);
        }
    }
}
