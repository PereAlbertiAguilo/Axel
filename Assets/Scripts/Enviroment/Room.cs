using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour
{
    public bool openUp = false; 
    public bool openDown = false;
    public bool openRight = false;
    public bool openLeft = false;

    public GameObject miniMapUp;
    public GameObject miniMapDown;
    public GameObject miniMapRight;
    public GameObject miniMapLeft;

    public DoorsManager doorsManager;
    public EnemiesManager enemiesManager;

    public GameObject miniMapDisplay;

    public Vector2Int roomIndex {  get; set; }

    public void OpenDoors()
    {
        doorsManager.OpenDoor("Up", openUp);
        doorsManager.OpenDoor("Down", openDown);
        doorsManager.OpenDoor("Right", openRight);
        doorsManager.OpenDoor("Left", openLeft);

        UpdateMiniMapDoors();
    }

    private void Update()
    {
        if(!enemiesManager.enemiesAlive)
        {
            OpenDoors();
        }
    }

    void UpdateMiniMapDoors()
    {
        miniMapUp.SetActive(openDown);
        miniMapDown.SetActive(openUp);
        miniMapRight.SetActive(openRight);
        miniMapLeft.SetActive(openLeft);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            UpdateMiniMapDoors();

            CameraController.instance.ChangeCameraPos(transform);
            miniMapDisplay.SetActive(true);
            enemiesManager.gameObject.SetActive(true);
        }
    }
}
