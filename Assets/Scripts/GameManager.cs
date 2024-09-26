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

    public bool isGameOver = false;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        DataPersistenceManager.instance.gameData.currentFloor = SceneManager.GetActiveScene().name;

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

    public void ResetGameData()
    {
        PlayerController.instance.canMove = false;

        for (int i = 0; i < SceneManager.sceneCountInBuildSettings; i++)
        {
            string key = System.IO.Path.GetFileNameWithoutExtension(SceneUtility.GetScenePathByBuildIndex(i));

            if (PlayerPrefs.HasKey(key)) PlayerPrefs.DeleteKey(key);
        }

        PlayerController.instance.ResetStats();
        PlayerPrefs.SetInt("Continue", 1);
        DataPersistenceManager.instance.NewGame();
    }

    public void RestartGame()
    {
        ResetGameData();

        MenusManager.instance.ChangeScene(SceneManager.GetSceneByBuildIndex(1).name);
    }
}
