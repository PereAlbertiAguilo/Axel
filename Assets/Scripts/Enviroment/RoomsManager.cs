using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomsManager : MonoBehaviour
{
    [SerializeField] GameObject roomPrefab;

    public List<GameObject> rooms = new List<GameObject>();

    [Space]

    public int minRooms = 10;
    public int maxRooms = 15;

    [Space]

    public Vector2Int roomSize = new Vector2Int();
    public Vector2Int gridSize = new Vector2Int();

    int roomIndex = 0;
    int roomCount = 0;

    Queue<GameObject> roomsQueue = new Queue<GameObject>();

    private void Start()
    {
        GenerateFirstRoom();

        roomCount = Random.Range(minRooms, maxRooms);
    }

    private void Update()
    {
        if(roomIndex <= roomCount)
        {
            TryGenerateRoom();
        }
    }

    public void GenerateFirstRoom()
    {
        GameObject roomInstance = Instantiate(roomPrefab, new Vector2(0, 0), Quaternion.identity);
        rooms.Add(roomInstance);
        Room room = roomInstance.GetComponent<Room>();

        int openDoors = Random.Range(1, 3);

        for (int i = 0; i < openDoors; i++)
        {
            room.doorsState[Random.Range(0, room.doorsState.Length)] = true;
        }
    }

    public void TryGenerateRoom()
    {
        Vector2Int lastRoomPos = new Vector2Int(Mathf.RoundToInt(rooms[roomIndex].transform.position.x) ,Mathf.RoundToInt(rooms[roomIndex].transform.position.y));
        Room lastRoom = rooms[roomIndex].GetComponent<Room>();

        roomIndex++;

        GameObject roomInstance = Instantiate(roomPrefab, GetNewRoomPositon(lastRoomPos, lastRoom), Quaternion.identity);
        roomInstance.name = "Room" + roomIndex;
        rooms.Add(roomInstance);

        Room room = roomInstance.GetComponent<Room>();

        //if (lastRoomPos.y > roomInstance.transform.position.y) room.doorsState[1] = true;
        //else if (lastRoomPos.y < roomInstance.transform.position.y) room.doorsState[0] = true;
        //else if (lastRoomPos.x > roomInstance.transform.position.x) room.doorsState[3] = true;
        //else if (lastRoomPos.x < roomInstance.transform.position.x) room.doorsState[2] = true;

        int openDoors = Random.Range(1, 3);

        for (int i = 0; i < openDoors; i++)
        {
            int randomDoor = Random.Range(0, room.doorsState.Length);

            print(roomInstance.name + "Door " + randomDoor + " opened");

            if (room.doorsState[randomDoor]) continue;

            room.doorsState[randomDoor] = true;
        }
    }

    public Vector2 GetNewRoomPositon(Vector2Int lastRoomPos, Room room)
    {
        bool[] doorsState = room.doorsState;

        Vector2 newPos = Vector2.zero;

        for (int i = 0; i < doorsState.Length; i++)
        {
            if (doorsState[i])
            {
                switch (i)
                {
                    //Up
                    case 0:
                        newPos = new Vector2(lastRoomPos.x, lastRoomPos.y + roomSize.y);
                    break;
                    //Down
                    case 1:
                        newPos = new Vector2(lastRoomPos.x, lastRoomPos.y - roomSize.y);
                    break;
                    //Right
                    case 2:
                        newPos = new Vector2(lastRoomPos.x + roomSize.x, lastRoomPos.y);
                    break; 
                    //Left
                    case 3:
                        newPos = new Vector2(lastRoomPos.x - roomSize.x, lastRoomPos.y);
                    break;
                }
            }
        }

        return newPos;
    }

    private void OnDrawGizmos()
    {
        for (int x = 0; x < gridSize.x; x++)
        {
            for(int y = 0; y < gridSize.y; y++)
            {
                Gizmos.DrawWireCube(new Vector2((x * roomSize.x) - (roomSize.x * (gridSize.x / 2)), (y * roomSize.y) - (roomSize.y * (gridSize.y / 2))), new Vector2(roomSize.x, roomSize.y));
            }
        }
    }
}
