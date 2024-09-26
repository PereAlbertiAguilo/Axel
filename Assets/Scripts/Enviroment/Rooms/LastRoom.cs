using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LastRoom : Room
{
    public GameObject portal;

    public override void Start()
    {
        base.Start();

        if (!roomCleared) portal.SetActive(false);
    }

    public override void RoomCleared()
    {
        base.RoomCleared();

        GameManager.instance.FloorCleared();

        portal.SetActive(true);
    }
}
