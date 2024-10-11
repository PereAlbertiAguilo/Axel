using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Room : MonoBehaviour
{
    [HideInInspector] public string prefabPath;

    [HideInInspector] public GameObject miniMapUp;
    [HideInInspector] public GameObject miniMapDown;
    [HideInInspector] public GameObject miniMapRight;
    [HideInInspector] public GameObject miniMapLeft;

    public DoorsManager doorsManager;
    public EnemiesManager enemiesManager;

    public GameObject miniMapDisplay;

    public Vector2Int roomGridPos {  get; set; }

    [HideInInspector] public int roomIndex = 0;
    public int exitsAmount = 0;

    [Space]

    public bool openUp = false;
    public bool openDown = false;
    public bool openRight = false;
    public bool openLeft = false;

    [Space]

    public bool roomCleared = false;
    public bool roomEntered = false;
    public bool roomLocked = false;
    public bool rewardGiven = false;

    [HideInInspector] public bool[] doorStates;

    public virtual void Start()
    {
        miniMapUp = miniMapDisplay.transform.GetChild(0).gameObject;
        miniMapDown = miniMapDisplay.transform.GetChild(1).gameObject;
        miniMapRight = miniMapDisplay.transform.GetChild(2).gameObject;
        miniMapLeft = miniMapDisplay.transform.GetChild(3).gameObject;
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
        GameManager.instance.SlowMo(1f);

        if (!rewardGiven)
        {
            rewardGiven = true;

            CollectiblesManager cm = CollectiblesManager.instance;
            cm.SpawnCollectable(cm.GetRandomCollectable(), transform.position, cm.index);
        }

        OpenDoors();

        StartCoroutine(PlaySoundDelayed(.45f));
    }


    public void OpenDoors()
    {
        StartCoroutine(OpenDoorsWhenGenerationComplete());
    }

    IEnumerator OpenDoorsWhenGenerationComplete()
    {
        yield return new WaitUntil(() => RoomManager.instance.generationComplete);

        doorsManager.OpenDoors("Up", openUp);
        doorsManager.OpenDoors("Down", openDown);
        doorsManager.OpenDoors("Right", openRight);
        doorsManager.OpenDoors("Left", openLeft);

        UpdateRoomMiniMapDisplay(false, false, openUp, openDown, openRight, openLeft);
    }

    public void CloseDoors()
    {
        if (openUp) doorsManager.animatorUp.Play("CloseDoors");
        if (openDown) doorsManager.animatorDown.Play("CloseDoors");
        if (openRight) doorsManager.animatorRight.Play("CloseDoors");
        if (openLeft) doorsManager.animatorLeft.Play("CloseDoors");
    }

    public void UpdateRoomMiniMapDisplay(bool activate, bool current, bool up, bool down, bool right, bool left)
    {
        if (activate) miniMapDisplay.SetActive(true);

        miniMapUp.SetActive(up);
        miniMapDown.SetActive(down);
        miniMapRight.SetActive(right);
        miniMapLeft.SetActive(left);

        if (current)
        {
            SpriteRenderer miniMapDisplayRenderer = miniMapDisplay.GetComponent<SpriteRenderer>();
            Color startColor = miniMapDisplayRenderer.color;

            miniMapDisplayRenderer.color = new Color(startColor.r, startColor.g, startColor.b, 1);
        }
    }

    public IEnumerator NextRoomsMiniMapUpdate()
    {
        yield return new WaitUntil(() => RoomManager.instance.generationComplete);

        if (openUp)
        {
            Room upRoom = GetRoomFromGridPos(new Vector2Int(roomGridPos.x, roomGridPos.y + 1)); //Room up
            if (!upRoom.roomEntered) upRoom.UpdateRoomMiniMapDisplay(true, false, false, openUp, false, false); 
        }
        if (openDown)
        {
            Room downRoom = GetRoomFromGridPos(new Vector2Int(roomGridPos.x, roomGridPos.y - 1)); //Room down
            if (!downRoom.roomEntered) downRoom.UpdateRoomMiniMapDisplay(true, false, openDown, false, false, false); 
        }
        if (openRight)
        {
            Room rightRoom = GetRoomFromGridPos(new Vector2Int(roomGridPos.x + 1, roomGridPos.y)); //Room right
            if (!rightRoom.roomEntered) rightRoom.UpdateRoomMiniMapDisplay(true, false, false, false, false, openRight); 
        }
        if (openLeft)
        {
            Room leftRoom = GetRoomFromGridPos(new Vector2Int(roomGridPos.x - 1, roomGridPos.y)); //Room left
            if (!leftRoom.roomEntered) leftRoom.UpdateRoomMiniMapDisplay(true, false, false, false, openLeft, false); 
        }
    }

    IEnumerator PlaySoundDelayed(float delay)
    {
        yield return new WaitForSeconds(delay);

        Audio.instance.PlayOneShot(Audio.Sound.changeRoom, .01f, true);
    }

    Room GetRoomFromGridPos(Vector2Int newRoomGridPos)
    {
        return RoomManager.instance.rooms.Find(x => x.GetComponent<Room>().roomGridPos ==
        newRoomGridPos).GetComponent<Room>();
    }

    IEnumerator ActivateRoom()
    {
        RoomManager.instance.currentRoom = this;

        if (enemiesManager.enemiesAlive) Invoke(nameof(CloseDoors), .45f);
        if (!roomCleared) StartCoroutine(PlaySoundDelayed(.45f));
        if (!roomEntered) StartCoroutine(NextRoomsMiniMapUpdate());

        CameraController.instance.ChangeCameraPos(transform);
        PlayerController.instance.Invoke(nameof(PlayerController.instance.MoveReset), .5f);
        PlayerController.instance._playerRigidbody.velocity = Vector2.zero;
        PlayerController.instance.currentEnemiesManager = enemiesManager;

        UpdateRoomMiniMapDisplay(true, true, openUp, openDown, openRight, openLeft);
        
        roomEntered = true;

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
