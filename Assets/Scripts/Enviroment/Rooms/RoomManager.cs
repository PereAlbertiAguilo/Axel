using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Pathfinding;
using UnityEngine.SceneManagement;

public class RoomManager : MonoBehaviour
{
    public static RoomManager instance { get; private set;}

    [SerializeField] string floorFolderName;
    [SerializeField] string treasureRoomsFolderName;

    [Space]

    [SerializeField] int initialRoomsAmount = 1;
    [SerializeField] int normalRoomsAmount = 1;
    [SerializeField] int lastRoomsAmount = 1;
    [SerializeField] int treasureRoomsAmount = 1;

    [Space]

    public int minRooms = 10;
    public int maxRooms = 15;

    [Space]

    public Vector2Int roomSize = new Vector2Int();
    public Vector2Int gridSize = new Vector2Int();

    [Space]

    public List<GameObject> rooms = new List<GameObject>();

    Queue<Vector2Int> roomQueue = new Queue<Vector2Int>();

    int[,] roomGrid;

    int roomCount;

    public Room currentRoom;

    public bool generationComplete = false;

    List<int> initialUsedRoomsIndex = new List<int>();
    List<int> normalUsedRoomsIndex = new List<int>();
    List<int> lastUsedRoomsIndex = new List<int>();
    List<int> specialRoomsIndex = new List<int>();

    int treasuereRoomIndex;

    AstarPath astar;
    Pathfinding.AstarData data;
    Pathfinding.GridGraph gridGraph;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        astar = AstarPath.active;
        Pathfinding.AstarData data = astar.data;
        gridGraph = data.gridGraph;

        roomGrid = new int[gridSize.x, gridSize.y];
        roomQueue = new Queue<Vector2Int>();

        GenerateRooms();
    }

    private void Update()
    {
        if (PlayerPrefs.HasKey(SceneManager.GetActiveScene().name) || generationComplete) return;

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
            RegenerateRooms();
        }
        else if (!generationComplete)
        {
            GenerateLastRoom();

            treasuereRoomIndex = Random.Range(1, roomCount - 2);
            GenerateSpecialRoom(rooms[treasuereRoomIndex].GetComponent<Room>(), treasureRoomsFolderName);

            PlayerPrefs.SetInt(SceneManager.GetActiveScene().name, 1);
            DataPersistenceManager.instance.gameData.roomStructures = GetFloorRoomStructures().ToArray();

            Invoke(nameof(GrdGraphScan), .1f);

            generationComplete = true;

            Time.timeScale = 1;
        }
    }

    void GrdGraphScan()
    {
        gridGraph.Scan();
    }

    void GenerateRooms()
    {
        if (PlayerPrefs.HasKey(SceneManager.GetActiveScene().name))
        {
            SetFloorRoomFromStructures(DataPersistenceManager.instance.gameData.roomStructures);
            Time.timeScale = 1;
            Invoke(nameof(GrdGraphScan), .1f);
        }
        else if (!generationComplete)
        {
            Vector2Int initialRoomIndex = new Vector2Int(gridSize.x / 2, gridSize.y / 2);
            Time.timeScale = 0;
            StartRoomGenerationFromRoom(initialRoomIndex);
        }
    }

    void StartRoomGenerationFromRoom(Vector2Int roomIndex)
    {
        roomQueue.Enqueue(roomIndex);
        roomGrid[roomIndex.x, roomIndex.y] = 1;
        roomCount++;

        initialUsedRoomsIndex.Add(RandomNumberNoRepeat.GetRandomNumberFromList(initialUsedRoomsIndex, 0, initialRoomsAmount));

        Room initialRoom = SetRoom($"{floorFolderName}/Initial/{initialUsedRoomsIndex.Last()}", GetPositionFromGridIndex(roomIndex));
        UpdateRoom(initialRoom, "Initial", roomIndex, roomCount - 1);
        rooms.Add(initialRoom.gameObject);
    }

    bool TryGenerateRoom(Vector2Int roomIndex)
    {
        if (roomIndex.x >= gridSize.x || roomIndex.y >= gridSize.y || roomIndex.x < 0 || roomIndex.y < 0) return false;
        if (roomCount >= maxRooms) return false;
        if (Random.value < 0.5f && roomIndex != Vector2Int.zero) return false;
        if (AdjacentRoomsCount(roomIndex) > 1) return false;
        if (roomGrid[roomIndex.x, roomIndex.y] != 0) return false;

        roomQueue.Enqueue(roomIndex);
        roomGrid[roomIndex.x, roomIndex.y] = 1;
        roomCount++;

        normalUsedRoomsIndex.Add(RandomNumberNoRepeat.GetRandomNumberFromList(normalUsedRoomsIndex, 0, normalRoomsAmount));

        Room newRoom = SetRoom($"{floorFolderName}/Normal/{normalUsedRoomsIndex.Last()}", GetPositionFromGridIndex(roomIndex));
        UpdateRoom(newRoom, "Normal", roomIndex, roomCount - 1);
        rooms.Add(newRoom.gameObject);
        OpenDoors(newRoom, roomIndex.x, roomIndex.y);

        return true;
    }

    void GenerateLastRoom()
    {
        Room lastRoom = rooms.Last().GetComponent<Room>();
        Vector2 lastRoomPos = rooms.Last().transform.position;

        lastUsedRoomsIndex.Add(RandomNumberNoRepeat.GetRandomNumberFromList(lastUsedRoomsIndex, 0, lastRoomsAmount));

        Room newLastRoom = SetRoom($"{floorFolderName}/Last/{lastUsedRoomsIndex.Last()}", lastRoomPos);
        UpdateRoom(newLastRoom, "Last", lastRoom.roomGridPos, lastRoom.roomIndex);
        OpenDoors(newLastRoom, lastRoom.roomGridPos.x, lastRoom.roomGridPos.y);

        int removedIndex = lastRoom.roomIndex;

        Destroy(rooms.Last());
        rooms.RemoveAt(removedIndex);

        rooms.Add(newLastRoom.gameObject);
    }

    void GenerateSpecialRoom(Room replacedRoom, string specialRoomTypeFolderName)
    {
        Vector2 replacedRoomPos = replacedRoom.transform.position;

        specialRoomsIndex.Add(RandomNumberNoRepeat.GetRandomNumberFromList(specialRoomsIndex, 0, treasureRoomsAmount));

        Room especialRoom = SetRoom($"{floorFolderName}/Special/{specialRoomTypeFolderName}/{specialRoomsIndex.Last()}", replacedRoomPos);
        UpdateRoom(especialRoom, "Special", replacedRoom.roomGridPos, replacedRoom.roomIndex);
        OpenDoors(especialRoom, replacedRoom.roomGridPos.x, replacedRoom.roomGridPos.y);

        int removedIndex = replacedRoom.roomIndex;

        Destroy(rooms[removedIndex]);
        rooms.RemoveAt(removedIndex);

        rooms.Add(especialRoom.gameObject);
    }

    Room SetRoom(string path, Vector2 pos)
    {
        GameObject randomLoad = Resources.Load(path, typeof(GameObject)) as GameObject;
        Room room = Instantiate(randomLoad, pos, Quaternion.identity).GetComponent<Room>();

        room.prefabPath = path;

        return room;
    }

    void UpdateRoom(Room room, string name, Vector2Int gridPos, int roomIndex)
    {
        room.transform.parent = transform.GetChild(0);
        room.gameObject.name = name;
        room.roomGridPos = gridPos;
        room.roomIndex = roomIndex;
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

    void OpenDoors(Room newRoom, int x, int y)
    {
        Room leftRoom = GetRoomAt(new Vector2Int(x - 1, y));
        Room rightRoom = GetRoomAt(new Vector2Int(x + 1, y));
        Room upRoom = GetRoomAt(new Vector2Int(x, y + 1));
        Room downRoom = GetRoomAt(new Vector2Int(x, y - 1));

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
        GameObject room = rooms.Find(r => r.GetComponent<Room>().roomGridPos == index);

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

    public List<RoomStructure> GetFloorRoomStructures()
    {
        List<RoomStructure> roomStructures = new List<RoomStructure>();

        foreach (GameObject roomInstance in rooms)
        {
            Room room = roomInstance.GetComponent<Room>();

            RoomStructure roomStructure = new RoomStructure()
            {
                pos = roomInstance.transform.position,
                prefabPath = room.prefabPath,
                gridPos = room.roomGridPos,
                roomIndex = room.roomIndex,
                enemiesAlive = room.enemiesManager.enemiesAlive,
                roomEntered = room.roomEntered,
                roomCleared = room.roomCleared,
                openUp = room.openUp,
                openDown = room.openDown,
                openRight = room.openRight,
                openLeft = room.openLeft,
            };

            roomStructures.Add(roomStructure);
        }

        return roomStructures;
    }

    public void SaveRoomStructureFromGridPos(int roomIndex)
    {
        Room room = rooms.Find(x => x.GetComponent<Room>().roomIndex == roomIndex).GetComponent<Room>();

        RoomStructure savedStructure = DataPersistenceManager.instance.gameData.roomStructures.ToList().Find(x => x.roomIndex == roomIndex);

        savedStructure.prefabPath = room.prefabPath;
        savedStructure.gridPos = room.roomGridPos;
        savedStructure.roomIndex = room.roomIndex;
        savedStructure.enemiesAlive = room.enemiesManager.enemiesAlive;
        savedStructure.roomEntered = room.roomEntered;
        savedStructure.roomCleared = room.roomCleared;
        savedStructure.openUp = room.openUp;
        savedStructure.openDown = room.openDown;
        savedStructure.openRight = room.openRight;
        savedStructure.openLeft = room.openLeft;

        //Debug.Log("Enemies Alive: " + savedStructure.enemiesAlive + $", Room {savedStructure.gridPos} data saved: " + PlayerPrefs.HasKey(SceneManager.GetActiveScene().name));
    }

    void SetFloorRoomFromStructures(RoomStructure[] roomStructures)
    {
        int roomIterations = 0;

        foreach (RoomStructure roomStructure in roomStructures)
        {
            roomIterations++;

            Room room = SetRoom(roomStructure.prefabPath, roomStructure.pos);

            room.prefabPath = roomStructure.prefabPath;
            room.roomGridPos = roomStructure.gridPos;
            room.roomIndex = roomStructure.roomIndex;
            room.enemiesManager.enemiesAlive = roomStructure.enemiesAlive;
            room.roomEntered = roomStructure.roomEntered;
            room.roomCleared = roomStructure.roomCleared;
            room.openUp = roomStructure.openUp;
            room.openDown = roomStructure.openDown;
            room.openRight = roomStructure.openRight;
            room.openLeft = roomStructure.openLeft;

            UpdateRoom(room, "SavedRoom", roomStructure.gridPos, roomStructure.roomIndex);

            if (!roomStructure.enemiesAlive) room.OpenDoors();

            if (roomStructure.roomEntered)
            {
                room.UpdateRoomMiniMapDisplay(true, true);
                room.StartCoroutine(room.NextRoomsMiniMapUpdate());
            }

            rooms.Add(room.gameObject);
        }

        if (roomIterations > 0) generationComplete = true;
    }

    IEnumerator SaveRoomDataWhenGenerationComplete(int roomIndex)
    {
        yield return new WaitUntil(() => generationComplete);

        if (PlayerPrefs.HasKey(SceneManager.GetActiveScene().name))
        {
            SaveRoomStructureFromGridPos(roomIndex);
        }
    }

    public void SaveRoomData(int roomIndex)
    {
        StartCoroutine(SaveRoomDataWhenGenerationComplete(roomIndex));
    }
}
