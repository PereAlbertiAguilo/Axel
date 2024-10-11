using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Door : MonoBehaviour
{
    [HideInInspector] public float offset = 8.25f;
    public Room room;

    public enum DoorDir 
    { 
        Up,
        Down, 
        Right, 
        Left 
    };

    public DoorDir dorDirection;

    public bool isLocked = false;

    private void Start()
    {
        StartCoroutine(GenerateDoorOpenerWhenGenerationComplete());
    }

    IEnumerator GenerateDoorOpenerWhenGenerationComplete()
    {
        yield return new WaitUntil(() => RoomManager.instance.generationComplete);

        if (isLocked)
        {
            GameObject doorOpenerObject = Resources.Load("Interactables/DoorOpener") as GameObject;
            GameObject instance = Instantiate(doorOpenerObject, transform);

            switch (dorDirection)
            {
                case DoorDir.Up:
                    instance.transform.position = new Vector2(transform.position.x, transform.position.y - 0.65f);
                    break;
                case DoorDir.Down:
                    instance.transform.position = new Vector2(transform.position.x, transform.position.y + 2.65f);
                    break;
                case DoorDir.Right:
                    instance.transform.position = new Vector2(transform.position.x - 1.5f, transform.position.y + 1.25f);
                    break;
                case DoorDir.Left:
                    instance.transform.position = new Vector2(transform.position.x + 1.5f, transform.position.y + 1.25f);
                    break;
            }

            DoorOpener doorOpener = instance.GetComponent<DoorOpener>();

            doorOpener.door = this;
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            PlayerController.instance.canMove = false;

            Transform playerPos = collision.transform;

            switch (dorDirection)
            {
                case DoorDir.Up:
                    playerPos.position = new Vector2(transform.position.x, playerPos.position.y + offset);
                    break;
                case DoorDir.Down:
                    playerPos.position = new Vector2(transform.position.x, playerPos.position.y - offset);
                    break;
                case DoorDir.Right:
                    playerPos.position = new Vector2(playerPos.position.x + offset, transform.position.y + 1.5f);
                    break;
                case DoorDir.Left:
                    playerPos.position = new Vector2(playerPos.position.x - offset, transform.position.y + 1.5f);
                    break;
            }

            FadeBlack.instance.FadeFromBlack();

            if (PlayerPrefs.HasKey(SceneManager.GetActiveScene().name))
            {
                DataPersistenceManager.instance.gameData.playerPos = playerPos.position;
            }
        }
    }
}
