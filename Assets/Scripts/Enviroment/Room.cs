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

    [HideInInspector] public bool[] doorStates;

    private void Start()
    {
        enemiesManager.gameObject.SetActive(false);
        print(transform.position + " " + name + ": " + roomGridPos);
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

        UpdateRoomMiniMapDisplay(false);
    }

    void UpdateRoomMiniMapDisplay(bool activate)
    {
        if (activate) miniMapDisplay.SetActive(true);

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

    void NextRoomsMiniMapUpdate()
    {
        if (openUp) GetRoomFromGridPos(new Vector2Int(roomGridPos.x, roomGridPos.y + 1)).UpdateRoomMiniMapDisplay(true);
        if (openDown) GetRoomFromGridPos(new Vector2Int(roomGridPos.x, roomGridPos.y - 1)).UpdateRoomMiniMapDisplay(true);
        if (openRight) GetRoomFromGridPos(new Vector2Int(roomGridPos.x + 1, roomGridPos.y)).UpdateRoomMiniMapDisplay(true);
        if (openLeft) GetRoomFromGridPos(new Vector2Int(roomGridPos.x - 1, roomGridPos.y)).UpdateRoomMiniMapDisplay(true);
    }

    Room GetRoomFromGridPos(Vector2Int newRoomGridPos)
    {
        return RoomManager.instance.rooms.Find(x => x.GetComponent<Room>().roomGridPos ==
        newRoomGridPos).GetComponent<Room>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            UpdateRoomMiniMapDisplay(true);

            if (enemiesManager.enemiesAlive) Invoke(nameof(CloseCurrentDoorsWithDelay), .45f);

            Invoke(nameof(NextRoomsMiniMapUpdate), .2f);

            CameraController.instance.ChangeCameraPos(transform);
            PlayerController.instance._playerRigidbody.velocity = Vector2.zero;
            PlayerController.instance.currentEnemiesManager = enemiesManager;

            enemiesManager.gameObject.SetActive(true);

            if (!roomCleared) StartCoroutine(PlaySoundDelayed(.45f));

        }
    }
}
