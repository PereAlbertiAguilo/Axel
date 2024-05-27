using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour
{
    public bool[] doorsState = { false, false, false, false };

    [Space]

    public DoorsManager doorsManager;
    public EnemiesManager enemiesManager;

    private void Update()
    {
        if (!enemiesManager.enemiesAlive)
        {
            doorsManager.OpenDoors(doorsState);
        }
    }
}
