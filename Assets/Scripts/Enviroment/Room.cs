using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour
{
    public bool openUp = false; 
    public bool openDown = false;
    public bool openRight = false;
    public bool openLeft = false;

    public DoorsManager doorsManager;
    public EnemiesManager enemiesManager;

    public Vector2Int roomIndex {  get; set; }

    public void OpenDoors()
    {
        doorsManager.OpenDoor("Up", openUp);
        doorsManager.OpenDoor("Down", openDown);
        doorsManager.OpenDoor("Right", openRight);
        doorsManager.OpenDoor("Left", openLeft);
    }

    private void Update()
    {
        if(!enemiesManager.enemiesAlive)
        {
            OpenDoors();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            //CameraController.instance.StartCoroutine(CameraController.instance.ChangeCameraPos(transform));
            CameraController.instance.ChangeCameraPos(transform);
            enemiesManager.gameObject.SetActive(true);
        }
    }
}
