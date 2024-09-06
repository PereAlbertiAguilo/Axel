using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance { get; private set; }

    public int floorIndex = 0;

    public bool floorCleared = false;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        floorIndex = SceneManager.GetActiveScene().buildIndex;
    }

    public bool FloorCleared()
    {
        GameObject[] roomInstances = RoomManager.instance.rooms.ToArray();

        Room lastRoom = null;

        for (int i = 0; i < roomInstances.Length; i++)
        {
            if (roomInstances[i].name == "Last") lastRoom = roomInstances[i].GetComponent<Room>();
        }

        if (lastRoom == null) return false;

        floorCleared = lastRoom.roomCleared;

        return floorCleared;
    }
}
