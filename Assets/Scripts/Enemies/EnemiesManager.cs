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
}
