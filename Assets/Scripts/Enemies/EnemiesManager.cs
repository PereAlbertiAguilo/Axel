using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemiesManager : MonoBehaviour
{
    public List<GameObject> enemiesList = new List<GameObject>();

    public bool enemiesAlive = true;

    public List<GameObject> FillEnemiesList()
    {
        enemiesList.Clear();

        foreach (EnemyController enemy in FindObjectsOfType<EnemyController>())
        {
            enemiesList.Add(enemy.gameObject);
        }

        return enemiesList;
    }

    public void AddToEnemiesList(GameObject enemy)
    {
        if (!enemiesAlive) enemiesAlive = true;

        enemiesList.Add(enemy);
    }

    public void RemoveFromEnemiesList(GameObject enemy)
    {
        enemiesList.Remove(enemy);

        if(enemiesList.Count <= 0)
        {
            enemiesAlive = false;
        }
    }

    public EnemyController GetClosestEnemyToPoint(Vector2 point)
    {
        int closestToPoint = 0;
        float currentDist = 999;

        if(enemiesList.Count <= 0)
        {
            return null;
        }

        for (int i = 0; i < enemiesList.Count; i++)
        {
            if (currentDist > Vector2.Distance(enemiesList[i].transform.position, point))
            {
                currentDist = Vector2.Distance(enemiesList[i].transform.position, point);
                closestToPoint = i;
            }
        }

        return enemiesList[closestToPoint].GetComponent<EnemyController>();
    }

    public EnemyController GetNearNextEnemy(EnemyController enemy, int dir)
    {
        EnemyController newEnemy = null;

        for (int i = 0; i < enemiesList.Count; i++)
        {
            if (enemy.gameObject == enemiesList[i])
            {
                if(dir > 0 && i + dir < enemiesList.Count)
                {
                    print("+1");
                    newEnemy = enemiesList[i + dir].GetComponent<EnemyController>();
                }
                if (dir < 0 && i - dir > 0)
                {
                    print("-1");
                    newEnemy = enemiesList[i - dir].GetComponent<EnemyController>();
                }
            }
        }

        return newEnemy;
    }
}
