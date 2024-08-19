using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static EnemyMovement;

public struct FEnemyAssignData
{
    public EnemyMovement enemy;
    public float distance;

    public FEnemyAssignData(EnemyMovement enemyMovement, float newDist)
    {
        enemy = enemyMovement;
        distance = newDist;
    }
}

public class EnemyManager : Singleton<EnemyManager>
{
    [SerializeField]
    private int enemiesPerPosition = 3;

    [SerializeField]
    private float assignPositionDelay = 0.5f;

    [SerializeField]
    private float cycleTime = 3.0f;

    private Dictionary<EAttackDirections, List<EnemyMovement>> engagedEnemies = new Dictionary<EAttackDirections, List<EnemyMovement>>();

    private List<EnemyMovement> enemyQueue = new List<EnemyMovement>();

    int coroutineVal = 0;
    
    // Start is called before the first frame update
    protected override void Awake()
    {
        base.Awake();
        foreach (EAttackDirections attackDir in Enum.GetValues(typeof(EAttackDirections)))
        {
            if (attackDir == EAttackDirections.Null)
                continue;
            engagedEnemies.Add(attackDir, new List<EnemyMovement>());
        }
    }

    private void Start()
    {
        UpdateEnemies();
        StartCoroutine(CycleEnemies(coroutineVal));
    }

    IEnumerator CycleEnemies(int cycleVal)
    {
        yield return new WaitForSeconds(cycleTime);
        if (cycleVal == coroutineVal)
        {
            UpdateEnemies();
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
        AssignPosition(leftEnemies, false);
        AssignPosition(rightEnemies, true);
        StartCoroutine(CycleEnemies(coroutineVal));
    } 

    private void AssignPosition(List<EnemyMovement> enemies, bool isRight)
    {
        EAttackDirections meleeDir = EAttackDirections.MeleeR;
        EAttackDirections rangeDir = EAttackDirections.RangeR;
        if (isRight)
        {
            meleeDir = EAttackDirections.MeleeL;
            rangeDir = EAttackDirections.RangeL;
        }
        List<FEnemyAssignData> enemyData = new List<FEnemyAssignData>();
        float totalDist = 0.0f;
        for (int i = 0; i < enemies.Count; i++)
        {
            float dist = enemies[i].GetMeleeDistance();
            totalDist += dist;
            enemyData.Add(new FEnemyAssignData(enemies[i], dist));
        }
        float randNum = UnityEngine.Random.Range(0, 1.0f);
        float percent = 0.0f;
        for (int i = 0; i < enemyData.Count; i++)
        {
            percent += totalDist == 0.0f ? 1.0f : (1 - (enemyData[i].distance / totalDist));
            if (randNum <= percent)
            {
                enemies[i].AssignPosition(meleeDir);
                enemyData.RemoveAt(i);
                enemyQueue.Remove(enemies[i]);
                engagedEnemies[meleeDir].Add(enemies[i]);
                break;
            }
        }
        if (engagedEnemies[rangeDir].Count != enemiesPerPosition)
        {
            int randVal = UnityEngine.Random.Range(0, enemyData.Count);
            enemyData[randVal].enemy.AssignPosition(rangeDir);
            engagedEnemies[rangeDir].Add(enemyData[randVal].enemy);
            enemyQueue.Remove(enemyData[randVal].enemy);
            enemyData.RemoveAt(randVal);
        }
        for (int i = 0; i < enemyData.Count; i++)
        {
            enemyData[i].enemy.AssignPosition(EAttackDirections.Null);
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
        if (enemy.GetAttackDirection() != EAttackDirections.Null && engagedEnemies[enemy.GetAttackDirection()].Contains(enemy))
        {
            engagedEnemies[enemy.GetAttackDirection()].Remove(enemy);
        }
        else if (enemyQueue.Contains(enemy))
        {
            enemyQueue.Remove(enemy);
        }
    }
}
