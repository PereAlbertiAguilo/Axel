using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class RoomManager : MonoBehaviour
{
    [SerializeField] string initialRoomsFolderName;
    [SerializeField] string normalRoomsFolderName;
    [SerializeField] string lastRoomsFolderName;

    [Space]

    [SerializeField] int initialRoomsAmount = 1;
    [SerializeField] int normalRoomsAmount = 1;
    [SerializeField] int lastRoomsAmount = 1;

    [Space]

    public int minRooms = 10;
    public int maxRooms = 15;

    [Space]

    public Vector2Int roomSize = new Vector2Int();
    public Vector2Int gridSize = new Vector2Int();

    public List<GameObject> rooms = new List<GameObject>();

    Queue<Vector2Int> roomQueue = new Queue<Vector2Int>();

    int[,] roomGrid;

    int roomCount;

    bool generationComplete = false;

    List<int> initialUsedRoomsIndex = new List<int>();
    List<int> normalUsedRoomsIndex = new List<int>();
    List<int> lastUsedRoomsIndex = new List<int>();

    private void Start()
    {
        Time.timeScale = 0;

        roomGrid = new int[gridSize.x, gridSize.y];
        roomQueue = new Queue<Vector2Int>();

        Vector2Int initialRoomIndex = new Vector2Int(gridSize.x / 2, gridSize.y / 2);
        StartRoomGenerationFromRoom(initialRoomIndex);
    }

    private void Update()
    {
        if(roomQueue.Count >0 && roomCount < maxRooms && !generationComplete)
        {
            Vector2Int roomIndex = roomQueue.Dequeue();

            if (roomIndex.x > 0 && roomGrid[roomIndex.x - 1, roomIndex.y] == 0)
            {
                //No neighbor to the left
                TryGenerateRoom(new Vector2Int(roomIndex.x - 1, roomIndex.y));
            }
            if (roomIndex.x < gridSize.x - 1 && roomGrid[roomIndex.x + 1, roomIndex.y] == 0)
            {
                //No neighbor to the right
                TryGenerateRoom(new Vector2Int(roomIndex.x + 1, roomIndex.y));
            }
            if (roomIndex.y > 0 && roomGrid[roomIndex.x, roomIndex.y - 1] == 0)
            {
                //No neighbor below
                TryGenerateRoom(new Vector2Int(roomIndex.x, roomIndex.y - 1));
            }
            if (roomIndex.y < gridSize.y - 1 && roomGrid[roomIndex.x, roomIndex.y + 1] == 0)
            {
                //No neighbor above
                TryGenerateRoom(new Vector2Int(roomIndex.x, roomIndex.y + 1));
            }

        }
        else if (roomCount < minRooms)
        {
            Debug.Log("Number of rooms was les than the minimum amount (" + minRooms + ") Trying Again");
            RegenerateRooms();
        }
        else if (!generationComplete)
        {
            for (int i = 1; i < rooms.Count; i++)
            {
                rooms[i].GetComponent<Room>().enemiesManager.gameObject.SetActive(false);
            }

            GenerateLastRoom();

            generationComplete = true;
            Time.timeScale = 1;
            Debug.Log("Generation Complete: " + roomCount + " rooms created");
        }
    }

    void StartRoomGenerationFromRoom(Vector2Int roomIndex)
    {
        roomQueue.Enqueue(roomIndex);
        roomGrid[roomIndex.x, roomIndex.y] = 1;
        roomCount++;

        initialUsedRoomsIndex.Add(RandomNumberNoRepeat.GetRandomNumberFromList(initialUsedRoomsIndex, 0, initialRoomsAmount));

        GameObject randomLoad = Resources.Load($"{initialRoomsFolderName}/{initialUsedRoomsIndex.Last()}", typeof(GameObject)) as GameObject;
        GameObject initialRoom = Instantiate(randomLoad, GetPositionFromGridIndex(roomIndex), Quaternion.identity);

        initialRoom.transform.parent = transform;   
        initialRoom.name = $"Room_{roomCount}";
        initialRoom.GetComponent<Room>().roomIndex = roomIndex;
        rooms.Add(initialRoom);
    }

    bool TryGenerateRoom(Vector2Int roomIndex)
    {
        if (roomIndex.x >= gridSize.x || roomIndex.y >= gridSize.y || roomIndex.x < 0 || roomIndex.y < 0) return false;
        if (roomCount >= maxRooms) return false;
        if(Random.value < 0.5f && roomIndex != Vector2Int.zero) return false;
        if(AdjacentRoomsCount(roomIndex) > 1) return false;
        if (roomGrid[roomIndex.x, roomIndex.y] != 0) return false;

        roomQueue.Enqueue(roomIndex);
        roomGrid[roomIndex.x, roomIndex.y] = 1;
        roomCount++;

        normalUsedRoomsIndex.Add(RandomNumberNoRepeat.GetRandomNumberFromList(normalUsedRoomsIndex, 0, normalRoomsAmount));

        GameObject randomLoad = Resources.Load($"{normalRoomsFolderName}/{normalUsedRoomsIndex.Last()}", typeof(GameObject)) as GameObject;
        GameObject newRoom = Instantiate(randomLoad, GetPositionFromGridIndex(roomIndex), Quaternion.identity);

        newRoom.transform.parent = transform; 
        newRoom.name = $"Room_{roomCount}";
        newRoom.GetComponent<Room>().roomIndex = roomIndex;
        rooms.Add(newRoom);
        OpenDoors(newRoom, roomIndex.x, roomIndex.y);

        return true;
    }

    void GenerateLastRoom()
    {
        Room lastRoom = rooms.Last().GetComponent<Room>();
        Vector2 lastRoomPos = rooms.Last().transform.position;

        lastUsedRoomsIndex.Add(RandomNumberNoRepeat.GetRandomNumberFromList(lastUsedRoomsIndex, 0, lastRoomsAmount));

        GameObject randomLoad = Resources.Load($"{lastRoomsFolderName}/{lastUsedRoomsIndex.Last()}", typeof(GameObject)) as GameObject;
        Room newLastRoom = Instantiate(randomLoad, lastRoomPos, Quaternion.identity).GetComponent<Room>();

        newLastRoom.openUp = lastRoom.openUp;
        newLastRoom.openDown = lastRoom.openDown;
        newLastRoom.openLeft = lastRoom.openLeft;
        newLastRoom.openRight = lastRoom.openRight;

        Destroy(rooms.Last());
    }

    void RegenerateRooms()
    {
        rooms.ForEach(Destroy);
        rooms.Clear();

        roomGrid = new int[gridSize.x, gridSize.y];

        roomQueue.Clear();

        roomCount = 0;

        generationComplete = false;

        Vector2Int initialRoomIndex = new Vector2Int(gridSize.x / 2, gridSize.y / 2);
        StartRoomGenerationFromRoom(initialRoomIndex);
    }

    void OpenDoors(GameObject room, int x, int y)
    {
        Room newRoom = room.GetComponent<Room>();

        Room leftRoom = GetRoomAt(new Vector2Int(x - 1, y));
        Room rightRoom = GetRoomAt(new Vector2Int(x + 1, y));
        Room upRoom = GetRoomAt(new Vector2Int(x, y + 1));
        Room downRoom = GetRoomAt(new Vector2Int(x, y - 1));

        //newRoom.left = leftRoom;
        //leftRoom.right = newRoom;
        //newRoom.right = rightRoom;
        //rightRoom.left = newRoom;
        //newRoom.down = downRoom;
        //downRoom.up = newRoom;
        //newRoom.up = upRoom;
        //upRoom.down = newRoom;

        //print("" + leftRoom.gameObject.name + " " + rightRoom.gameObject.name + " " + upRoom.gameObject.name + " " + downRoom.gameObject.name);

        if (x > 0 && roomGrid[x - 1, y] != 0)
        {
            newRoom.openLeft = true;
            leftRoom.openRight = true;
        }
        if (x < gridSize.x - 1 && roomGrid[x + 1, y] != 0)
        {
            newRoom.openRight = true;
            rightRoom.openLeft = true;
        }
        if (y > 0 && roomGrid[x, y - 1] != 0)
        {
            newRoom.openDown = true;
            downRoom.openUp = true;
        }
        if (y < gridSize.y - 1 && roomGrid[x, y + 1] != 0)
        {
            newRoom.openUp = true;
            upRoom.openDown = true;
        }
    }

    Room GetRoomAt(Vector2Int index)
    {
        GameObject room = rooms.Find(r => r.GetComponent<Room>().roomIndex == index);

        if(room != null) return room.GetComponent<Room>();

        return null;
    }

    int AdjacentRoomsCount(Vector2Int roomIndex)
    {
        int x = roomIndex.x;
        int y = roomIndex.y;

        int count = 0;

        if(x > 0 && roomGrid[x - 1, y] != 0) count++;//Left
        if (x < gridSize.x - 1 && roomGrid[x + 1, y] != 0) count++;//Right
        if(y > 0 && roomGrid[x, y - 1] != 0) count++;//Down
        if (y < gridSize.y - 1 && roomGrid[x, y + 1] != 0) count++;//Up

        return count;
    }

    Vector2 GetPositionFromGridIndex(Vector2Int gridIndex)
    {
        return new Vector2(roomSize.x * (gridIndex.x - gridSize.x / 2), roomSize.y * (gridIndex.y - gridSize.y / 2));
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = new Color(0, 1, 1, 0.05f);

        for (int x = 0; x < gridSize.x; x++)
        {
            for (int y = 0; y < gridSize.y; y++)
            {
                Vector2 pos = GetPositionFromGridIndex(new Vector2Int(x, y));
                Gizmos.DrawWireCube(pos, new Vector2(roomSize.x, roomSize.y));
            }
        }
    }
}
