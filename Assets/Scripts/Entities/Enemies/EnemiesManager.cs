using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemiesManager : MonoBehaviour
{
    public List<GameObject> enemiesList = new List<GameObject>();

    public bool enemiesAlive = true;

    private void Start()
    {
        foreach (Transform enemy in transform)
        {
            enemiesList.Add(enemy.gameObject);
        }
    }

    public void AddToEnemiesList(GameObject enemy)
    {
        enemiesAlive = true;

        if(!enemiesList.Contains(enemy)) enemiesList.Add(enemy);
    }

    public void RemoveFromEnemiesList(GameObject enemy)
    {
        enemiesList.Remove(enemy);

        if(enemiesList.Count <= 0)
        {
            enemiesAlive = false;
        }
    }

    public void UpdateEnemies(bool activate)
    {
        foreach (Transform enemy in transform)
        {
            enemy.gameObject.SetActive(activate);
        }
    }

    public Enemy GetClosestEnemyToPoint(Vector2 point)
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

        return enemiesList[closestToPoint].GetComponent<Enemy>();
    }

    public Enemy GetNearNextEnemy(Enemy enemy, int dir)
    {
        Enemy newEnemy = null;

        for (int i = 0; i < enemiesList.Count; i++)
        {
            if (enemy.gameObject == enemiesList[i])
            {
                if(dir > 0 && i + dir < enemiesList.Count)
                {
                    print("+1");
                    newEnemy = enemiesList[i + dir].GetComponent<Enemy>();
                }
                if (dir < 0 && i - dir > 0)
                {
                    print("-1");
                    newEnemy = enemiesList[i - dir].GetComponent<Enemy>();
                }
            }
        }

        return newEnemy;
    }
}
