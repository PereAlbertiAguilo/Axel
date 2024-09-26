using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Room : MonoBehaviour
{
    [HideInInspector] public string prefabPath;

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

    public bool roomCleared = false;
    public bool roomEntered = false;

    [HideInInspector] public bool[] doorStates;

    public virtual void Start()
    {
    }

    private void Update()
    {
        if(!enemiesManager.enemiesAlive && !roomCleared && enemiesManager.enemiesList.Count <= 0)
        {
            RoomCleared();
        }
    }

    public virtual void RoomCleared()
    {
        roomCleared = true;

        RoomManager.instance.SaveRoomData(roomIndex);

        OpenDoors();

        StartCoroutine(PlaySoundDelayed(.45f));
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

        UpdateRoomMiniMapDisplay(false, false);
    }

    public void UpdateRoomMiniMapDisplay(bool activate, bool current)
    {
        if (activate) miniMapDisplay.SetActive(true);

        miniMapUp.SetActive(openDown);
        miniMapDown.SetActive(openUp);
        miniMapRight.SetActive(openRight);
        miniMapLeft.SetActive(openLeft);

        if (current)
        {
            SpriteRenderer miniMapDisplayRenderer = miniMapDisplay.GetComponent<SpriteRenderer>();
            Color startColor = miniMapDisplayRenderer.color;

            miniMapDisplayRenderer.color = new Color(startColor.r, startColor.g, startColor.b, 1);
        }
    }

    void CloseCurrentDoorsWithDelay()
    {
        if (openUp) doorsManager.animatorUp.Play("CloseDoors");
        if (openDown) doorsManager.animatorDown.Play("CloseDoors");
        if (openRight) doorsManager.animatorRight.Play("CloseDoors");
        if (openLeft) doorsManager.animatorLeft.Play("CloseDoors");
    }

    public IEnumerator NextRoomsMiniMapUpdate()
    {
        yield return new WaitUntil(() => RoomManager.instance.generationComplete);

        if (openUp) GetRoomFromGridPos(new Vector2Int(roomGridPos.x, roomGridPos.y + 1)).UpdateRoomMiniMapDisplay(true, false);
        if (openDown) GetRoomFromGridPos(new Vector2Int(roomGridPos.x, roomGridPos.y - 1)).UpdateRoomMiniMapDisplay(true, false);
        if (openRight) GetRoomFromGridPos(new Vector2Int(roomGridPos.x + 1, roomGridPos.y)).UpdateRoomMiniMapDisplay(true, false);
        if (openLeft) GetRoomFromGridPos(new Vector2Int(roomGridPos.x - 1, roomGridPos.y)).UpdateRoomMiniMapDisplay(true, false);
    }

    Room GetRoomFromGridPos(Vector2Int newRoomGridPos)
    {
        return RoomManager.instance.rooms.Find(x => x.GetComponent<Room>().roomGridPos ==
        newRoomGridPos).GetComponent<Room>();
    }

    IEnumerator ActivateRoom()
    {
        roomEntered = true;

        RoomManager.instance.currentRoom = this;

        UpdateRoomMiniMapDisplay(true, true);

        if (enemiesManager.enemiesAlive) Invoke(nameof(CloseCurrentDoorsWithDelay), .45f);

        StartCoroutine(NextRoomsMiniMapUpdate());

        CameraController.instance.ChangeCameraPos(transform);
        PlayerController.instance.Invoke(nameof(PlayerController.instance.MoveReset), .5f);
        PlayerController.instance._playerRigidbody.velocity = Vector2.zero;
        PlayerController.instance.currentEnemiesManager = enemiesManager;

        if (!roomCleared) StartCoroutine(PlaySoundDelayed(.45f));

        yield return new WaitForSeconds(.45f);

        enemiesManager.UpdateEnemies(enemiesManager.enemiesAlive);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            StartCoroutine(ActivateRoom());

            RoomManager.instance.SaveRoomData(roomIndex);
        }
    }
}
