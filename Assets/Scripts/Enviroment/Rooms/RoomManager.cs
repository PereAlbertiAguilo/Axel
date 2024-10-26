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
    [SerializeField] string shopRoomsFolderName;

    [Space]

    [SerializeField] int initialRoomsAmount = 1;
    [SerializeField] int normalRoomsAmount = 1;
    [SerializeField] int lastRoomsAmount = 1;
    [SerializeField] int treasureRoomsAmount = 1;
    [SerializeField] int shopRoomsAmount = 1;

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
    List<int> treasureRoomsIndex = new List<int>();
    List<int> shopRoomsIndex = new List<int>();

    int treasuereRoomIndex;

    AstarPath astar;
    AstarData data;
    GridGraph gridGraph;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        astar = AstarPath.active;
        AstarData data = astar.data;
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

            TryGenerateRoom(new Vector2Int(roomIndex.x - 1, roomIndex.y));
            TryGenerateRoom(new Vector2Int(roomIndex.x + 1, roomIndex.y));
            TryGenerateRoom(new Vector2Int(roomIndex.x, roomIndex.y - 1));
            TryGenerateRoom(new Vector2Int(roomIndex.x, roomIndex.y + 1));
        }
        else if (roomCount < minRooms || FloorHasOneExitRooms())
        {
            RegenerateRooms();
        }
        else if (!generationComplete)
        {
            // Generate Special Rooms
            GenerateLastRoom();
            GenerateSpecialRoom(treasureRoomsFolderName, treasureRoomsIndex, treasureRoomsAmount);
            GenerateSpecialRoom(shopRoomsFolderName, shopRoomsIndex, shopRoomsAmount);

            Invoke(nameof(GridGraphScan), .1f);
            FinalFoorUpdate();
            Time.timeScale = 1;

            PlayerPrefs.SetInt(SceneManager.GetActiveScene().name, 1);

            generationComplete = true;

            SaveRoomStructures();
        }
    }

    public void FinalFoorUpdate()
    {
        foreach (GameObject roomObject in rooms)
        {
            Room room = roomObject.GetComponent<Room>();

            if (!room.enemiesManager.enemiesAlive) room.OpenDoors();

            if (room.roomEntered)
            {
                room.StartCoroutine(room.NextRoomsMiniMapUpdate());
                if (room.roomCleared) room.UpdateRoomMiniMapDisplay(true, true, room.openUp, room.openDown, room.openRight, room.openLeft);
            }
        }
    }

    bool FloorHasOneExitRooms()
    {
        int amountOfRoomsWithOneExit = 0;

        foreach (GameObject roomObject in rooms)
        {
            Room room = roomObject.GetComponent<Room>();

            if (room.openUp) room.exitsAmount++;
            if (room.openDown) room.exitsAmount++;
            if (room.openRight) room.exitsAmount++;
            if (room.openLeft) room.exitsAmount++;

            if (room.exitsAmount == 1) amountOfRoomsWithOneExit++;
        }

        return amountOfRoomsWithOneExit == 0;
    }

    void GridGraphScan()
    {
        gridGraph.Scan();
    }

    void GenerateRooms()
    {
        if (PlayerPrefs.HasKey(SceneManager.GetActiveScene().name))
        {
            LoadRoomStructures(DataPersistenceManager.instance.gameData.roomStructures);
            Time.timeScale = 1;
            Invoke(nameof(GridGraphScan), .1f);
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
        OpenDoors(newRoom, roomIndex.x, roomIndex.y);
        rooms.Add(newRoom.gameObject);

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

    void GenerateSpecialRoom(string specialRoomTypeFolderName, List<int> roomIndex, int roomAmount)
    {
        Vector2Int validGridPos = Vector2Int.zero;

        for (int i = 0; i < rooms.Count - 2; i++)
        {
            Room room = rooms[i].GetComponent<Room>();

            int x = room.roomGridPos.x;
            int y = room.roomGridPos.y;

            Vector2Int tempGridPos;

            if (y < gridSize.y - 1 && roomGrid[x, y + 1] == 0)
            {
                tempGridPos = new Vector2Int(x, y + 1);

                if (roomGrid[tempGridPos.x + 1, tempGridPos.y] == 0 &&
                    roomGrid[tempGridPos.x - 1, tempGridPos.y] == 0 &&
                    roomGrid[tempGridPos.x, tempGridPos.y + 1] == 0)
                {
                    validGridPos = tempGridPos;
                    break;
                }
            }
            if (y > 0 && roomGrid[x, y - 1] == 0)
            {
                tempGridPos = new Vector2Int(x, y - 1);

                if (roomGrid[tempGridPos.x + 1, tempGridPos.y] == 0 &&
                    roomGrid[tempGridPos.x - 1, tempGridPos.y] == 0 &&
                    roomGrid[tempGridPos.x, tempGridPos.y - 1] == 0)
                {
                    validGridPos = tempGridPos;
                    break;
                }
            }
            if (x < gridSize.x - 1 && roomGrid[x + 1, y] == 0)
            {
                tempGridPos = new Vector2Int(x + 1, y);

                if (roomGrid[tempGridPos.x, tempGridPos.y + 1] == 0 &&
                    roomGrid[tempGridPos.x, tempGridPos.y - 1] == 0 &&
                    roomGrid[tempGridPos.x + 1, tempGridPos.y] == 0)
                {
                    validGridPos = tempGridPos;
                    break;
                }
            }
            if (x > 0 && roomGrid[x - 1, y] == 0)
            {
                tempGridPos = new Vector2Int(x - 1, y);

                if (roomGrid[tempGridPos.x, tempGridPos.y + 1] == 0 &&
                    roomGrid[tempGridPos.x, tempGridPos.y - 1] == 0 &&
                    roomGrid[tempGridPos.x + 1, tempGridPos.y] == 0)
                {
                    validGridPos = tempGridPos;
                    break;
                }
            }
        }

        if (validGridPos == Vector2Int.zero)
        {
            RegenerateRooms();
            return; 
        }

        roomQueue.Enqueue(validGridPos);
        roomGrid[validGridPos.x, validGridPos.y] = 1;
        roomCount++;

        roomIndex.Add(RandomNumberNoRepeat.GetRandomNumberFromList(roomIndex, 0, roomAmount));

        Room especialRoom = SetRoom($"{floorFolderName}/Special/{specialRoomTypeFolderName}/{roomIndex.Last()}", GetPositionFromGridIndex(validGridPos));
        UpdateRoom(especialRoom, "Special", validGridPos, roomCount - 1);
        OpenDoors(especialRoom, validGridPos.x, validGridPos.y);
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

        int x = gridPos.x;
        int y = gridPos.y;

        Room leftRoom = GetRoomAt(new Vector2Int(x - 1, y));
        Room rightRoom = GetRoomAt(new Vector2Int(x + 1, y));
        Room upRoom = GetRoomAt(new Vector2Int(x, y + 1));
        Room downRoom = GetRoomAt(new Vector2Int(x, y - 1));

        if (y < gridSize.y - 1 && roomGrid[x, y + 1] != 0) upRoom.doorsManager.doorDown.isLocked = room.roomLocked;
        if (y > 0 && roomGrid[x, y - 1] != 0) downRoom.doorsManager.doorUp.isLocked = room.roomLocked;
        if (x < gridSize.x - 1 && roomGrid[x + 1, y] != 0) rightRoom.doorsManager.doorLeft.isLocked = room.roomLocked;
        if (x > 0 && roomGrid[x - 1, y] != 0) leftRoom.doorsManager.doorRight.isLocked = room.roomLocked;
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

    bool TryGetRoomAdjacentToGridPos(Vector2Int gridPos, Vector2Int dir)
    {
        Vector2Int newGridPos = gridPos + dir;

        if(newGridPos.x < 0 || newGridPos.x > gridSize.x || newGridPos.y < 0 || newGridPos.y > gridSize.y) return false;

        return roomGrid[newGridPos.x, newGridPos.y] != 0;
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

    public void SaveRoomStructures()
    {
        List<RoomStructure> roomStructures = new List<RoomStructure>();

        foreach (GameObject roomInstance in rooms)
        {
            Room room = roomInstance.GetComponent<Room>();

            RoomStructure roomStructure = new RoomStructure(roomInstance.transform.position, room.prefabPath, room.gameObject.name, room.roomGridPos,
                room.roomIndex, room.enemiesManager.enemiesAlive, room.roomEntered, room.roomCleared, room.roomLocked, room.rewardGiven,
                room.openUp, room.openDown, room.openRight, room.openLeft, room.doorsManager.doorUp.isLocked, room.doorsManager.doorDown.isLocked,
                room.doorsManager.doorRight.isLocked, room.doorsManager.doorLeft.isLocked);

            roomStructures.Add(roomStructure);
        }

        DataPersistenceManager.instance.gameData.roomStructures = roomStructures.ToArray();
    }

    void LoadRoomStructures(RoomStructure[] roomStructures)
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
            room.roomLocked = roomStructure.roomLocked;
            room.rewardGiven = roomStructure.rewardGiven;
            room.openUp = roomStructure.openUp;
            room.openDown = roomStructure.openDown;
            room.openRight = roomStructure.openRight;
            room.openLeft = roomStructure.openLeft;
            room.doorsManager.doorUp.isLocked = roomStructure.doorUpLocked;
            room.doorsManager.doorDown.isLocked = roomStructure.doorDownLocked;
            room.doorsManager.doorRight.isLocked = roomStructure.doorRightLocked;
            room.doorsManager.doorLeft.isLocked = roomStructure.doorLeftLocked;

            UpdateRoom(room, roomStructure.name, roomStructure.gridPos, roomStructure.roomIndex);

            if (!roomStructure.enemiesAlive) room.OpenDoors();

            if (roomStructure.roomEntered)
            {
                room.StartCoroutine(room.NextRoomsMiniMapUpdate());
                if (roomStructure.roomCleared) room.UpdateRoomMiniMapDisplay(true, true, room.openUp, room.openDown, room.openRight, room.openLeft);
            }

            rooms.Add(room.gameObject);
        }

        if (roomIterations > 0) generationComplete = true;
    }

    public void SaveRoomStructureFromGridPos(int roomIndex)
    {
        Room room = rooms.Find(x => x.GetComponent<Room>().roomIndex == roomIndex).GetComponent<Room>();

        RoomStructure savedStructure = DataPersistenceManager.instance.gameData.roomStructures.ToList().Find(x => x.roomIndex == roomIndex);

        if (savedStructure != null)
        {
            savedStructure.roomEntered = room.roomEntered;
            savedStructure.roomCleared = room.roomCleared;
            savedStructure.roomLocked = room.roomLocked;
            savedStructure.rewardGiven = room.rewardGiven;
            savedStructure.enemiesAlive = room.enemiesManager.enemiesAlive;

            savedStructure.doorUpLocked = room.doorsManager.doorUp.isLocked;
            savedStructure.doorDownLocked = room.doorsManager.doorDown.isLocked;
            savedStructure.doorRightLocked = room.doorsManager.doorRight.isLocked;
            savedStructure.doorLeftLocked = room.doorsManager.doorLeft.isLocked;
        }
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
