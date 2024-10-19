using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
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

        HudManager.instance.timer = 0;
        CollectiblesManager.instance.UpdateCollectable(true);
        PlayerController.instance.ResetStats();
        PlayerPrefs.SetInt("Continue", 1);

        DataPersistenceManager.instance.RemovePlayerPrefsData();
        DataPersistenceManager.instance.gameData.playerPos = new Vector2(0, 0.5f);
        DataPersistenceManager.instance.NewGame();
    }

    public void ClearDataForNextFloor()
    {
        DataPersistenceManager.instance.gameData.playerPos = new Vector2(0, 0.5f);
        PlayerController.instance.canMove = false;
    }

    public void RestartGame()
    {
        ResetGameData();

        MenusManager.instance.ChangeScene(SceneNameByBuildIndex(1));
    }

    public static string SceneNameByBuildIndex(int index)
    {
        string scenePath = SceneUtility.GetScenePathByBuildIndex(index);
        string sceneName = System.IO.Path.GetFileNameWithoutExtension(scenePath);

        return sceneName;
    }

    public IEnumerator SlowMotionAnimation(float duration)
    {
        PauseMenu.instance.canPause = false;

        float currentTime = 0;
        Time.timeScale = 0;

        while (currentTime < duration)
        {
            currentTime += Time.unscaledDeltaTime;

            Time.timeScale = Mathf.Clamp(Mathf.Lerp(0f, 1f, currentTime / duration), 0f, 1f);

            yield return null;
        }

        Time.timeScale = 1;

        PauseMenu.instance.canPause = true;
    }

    public void SlowMo(float duration)
    {
        StartCoroutine(SlowMotionAnimation(duration));
    }
}
