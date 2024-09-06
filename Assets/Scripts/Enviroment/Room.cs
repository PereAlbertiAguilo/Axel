using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour
{
    public GameObject miniMapUp;
    public GameObject miniMapDown;
    public GameObject miniMapRight;
    public GameObject miniMapLeft;

    public DoorsManager doorsManager;
    public EnemiesManager enemiesManager;

    public GameObject miniMapDisplay;

    public Vector2Int roomGridPos {  get; set; }

    [HideInInspector] public int roomIndex = 0;

    [HideInInspector] public bool openUp = false;
    [HideInInspector] public bool openDown = false;
    [HideInInspector] public bool openRight = false;
    [HideInInspector] public bool openLeft = false;

    [HideInInspector] public bool roomCleared = false;

    private void Start()
    {
        enemiesManager.gameObject.SetActive(false);
    }

    private void Update()
    {
        if(!enemiesManager.enemiesAlive && !roomCleared)
        {
            roomCleared = true;

            OpenDoors();

            GameManager.instance.FloorCleared();

            StartCoroutine(PlaySoundDelayed(.45f));
        }
    }

    IEnumerator PlaySoundDelayed(float delay)
    {
        yield return new WaitForSeconds(delay);

        Audio.instance.PlayOneShot(Audio.Sound.changeRoom, .01f, true);
    }

    public void OpenDoors()
    {
        doorsManager.OpenDoor("Up", openUp);
        doorsManager.OpenDoor("Down", openDown);
        doorsManager.OpenDoor("Right", openRight);
        doorsManager.OpenDoor("Left", openLeft);

        UpdateMiniMapDoors();
    }

    void UpdateMiniMapDoors()
    {
        miniMapUp.SetActive(openDown);
        miniMapDown.SetActive(openUp);
        miniMapRight.SetActive(openRight);
        miniMapLeft.SetActive(openLeft);
    }

    void CloseCurrentDoorsWithDelay()
    {
        if (openUp) doorsManager.animatorUp.Play("CloseDoors");
        if (openDown) doorsManager.animatorDown.Play("CloseDoors");
        if (openRight) doorsManager.animatorRight.Play("CloseDoors");
        if (openLeft) doorsManager.animatorLeft.Play("CloseDoors");
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            UpdateMiniMapDoors();

            if (enemiesManager.enemiesAlive) Invoke(nameof(CloseCurrentDoorsWithDelay), .45f);

            CameraController.instance.ChangeCameraPos(transform);
            miniMapDisplay.SetActive(true);
            enemiesManager.gameObject.SetActive(true);
            PlayerController.instance._playerRigidbody.velocity = Vector2.zero;
            PlayerController.instance.currentEnemiesManager = enemiesManager;

            if (!roomCleared) StartCoroutine(PlaySoundDelayed(.45f));
        }
    }
}
