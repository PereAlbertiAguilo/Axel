using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class RoomsManagerTest : MonoBehaviour
{
    public List<GridChunkTest> chunks = new List<GridChunkTest>();
    [SerializeField] GameObject chunk;

    [SerializeField] GameObject roomPrefab;

    public List<GameObject> rooms = new List<GameObject>();

    [Space]

    public int minRooms = 10;
    public int maxRooms = 15;

    [Space]

    public Vector2Int roomSize = new Vector2Int();
    public Vector2Int gridSize = new Vector2Int();

    int chunkIndex = 0;
    int roomIndex = 0;
    int roomCount = 0;

    Queue<GameObject> roomsQueue = new Queue<GameObject>();

    private void Start()
    {
        for (int x = 0; x < gridSize.x; x++)
        {
            for (int y = 0; y < gridSize.y; y++)
            {
                GenerateRoomChunk(new Vector2((x * roomSize.x) - (roomSize.x * (gridSize.x / 2)), (y * roomSize.y) - (roomSize.y * (gridSize.y / 2))), roomSize, chunkIndex);
                chunkIndex++;
            }
        }

        GenerateFirstRoom();

        roomCount = Random.Range(minRooms, maxRooms);

        for (int i = 0; i < roomCount; i++)
        {
            if(roomIndex < roomCount)
            {
                if (!TryGenerateRoom()) continue;
                if (!TryGenerateRoom()) continue;
                if (!TryGenerateRoom()) continue;
                if (!TryGenerateRoom()) continue;
            }
        }
    }

    public void GenerateFirstRoom()
    {
        GameObject roomInstance = Instantiate(roomPrefab, new Vector2(0, 0), Quaternion.identity);
        rooms.Add(roomInstance);
        RoomTest room = roomInstance.GetComponent<RoomTest>();
        room.roomIndex = roomIndex;
        roomInstance.name = "Room" + roomIndex;

        foreach (GridChunkTest chunk in chunks)
        {
            if(roomInstance.transform.position == chunk.transform.position)
            {
                chunk.hasRoom = true;
                room.chunkIndex = chunk.index;
            }
        }
    }

    public bool TryGenerateRoom()
    {
        RoomTest lastRoom = rooms[roomIndex].GetComponent<RoomTest>();

        roomIndex++;

        GridChunkTest newChunk = GetNewRoomPositon(chunks[lastRoom.chunkIndex], lastRoom);

        if(newChunk == null)
        {
            return false;
        }

        GameObject roomInstance = Instantiate(roomPrefab, newChunk.position, Quaternion.identity);
        rooms.Add(roomInstance);
        RoomTest room = roomInstance.GetComponent<RoomTest>();
        room.doorsState[newChunk.roomDirection] = true;
        room.roomIndex = roomIndex;
        roomInstance.name = "Room" + roomIndex;

        foreach (GridChunkTest chunk in chunks)
        {
            if (roomInstance.transform.position == chunk.transform.position)
            {
                chunk.hasRoom = true;
                room.chunkIndex = chunk.index;
            }
        }

        return true;
    }

    public GridChunkTest GetNewRoomPositon(GridChunkTest lastChunk, RoomTest lastRoom)
    {
        int axis = Random.Range(0, 2);
        int direction = Random.Range(0, 2);

        GridChunkTest upChunk = chunks[lastChunk.index + 1];
        GridChunkTest downChunk = chunks[lastChunk.index - 1];
        GridChunkTest rightChunk = chunks[lastChunk.index + gridSize.y <= chunks.Last().index ? lastChunk.index + gridSize.y: chunks.Last().index];
        GridChunkTest leftChunk = chunks[lastChunk.index - gridSize.y >= chunks[0].index ? lastChunk.index - gridSize.y : chunks[0].index];

        upChunk.roomDirection = 1;
        downChunk.roomDirection = 0;
        rightChunk.roomDirection = 3;
        leftChunk.roomDirection = 2;

        for (int i = 0; i < 4; i++)
        {
            if (axis == 0)
            {
                if (direction == 0 && upChunk.index % gridSize.y != 0 && !upChunk.hasRoom)
                {
                    lastRoom.doorsState[0] = true;
                    return upChunk;
                }
                if (direction > 0 && downChunk.index % gridSize.y != 0 && !downChunk.hasRoom)
                {
                    lastRoom.doorsState[1] = true;
                    return downChunk;
                }
            }
            if (axis > 0)
            {
                if (direction == 0 && rightChunk.index % gridSize.x != 0 && !rightChunk.hasRoom)
                {
                    lastRoom.doorsState[2] = true;
                    return rightChunk;
                }
                if (direction > 0 && leftChunk.index % gridSize.x != 0 && !leftChunk.hasRoom)
                {
                    lastRoom.doorsState[3] = true;
                    return leftChunk;
                }
            }

            //print("Try: " + i + " Chunk not founded");

            axis = Random.Range(0, 2);
            direction = Random.Range(0, 2);
        }

        if (upChunk.index % gridSize.y != 0 && !upChunk.hasRoom)
        {
            lastRoom.doorsState[0] = true;
            return upChunk;
        }
        if (downChunk.index % gridSize.y != 0 && !downChunk.hasRoom)
        {
            lastRoom.doorsState[1] = true;
            return downChunk;
        }
        if (rightChunk.index % gridSize.x != 0 && !rightChunk.hasRoom)
        {
            lastRoom.doorsState[2] = true;
            return rightChunk;
        }
        if (leftChunk.index % gridSize.x != 0 && !leftChunk.hasRoom)
        {
            lastRoom.doorsState[3] = true;
            return leftChunk;
        }

        //print("Chunk not founded. axis: " + axis + " direction: " + direction + " Chunks index: " + upChunk.index + " " + downChunk.index + " " + rightChunk.index + " " + leftChunk.index);

        return chunks[0];
    }

    public void GenerateRoomChunk(Vector2 pos, Vector2 size, int index)
    {
        GameObject gridChunkObj = Instantiate(chunk, pos, Quaternion.identity);
        gridChunkObj.transform.SetParent(transform);
        GridChunkTest gridChunk = gridChunkObj.GetComponent<GridChunkTest>();
        chunks.Add(gridChunk);

        gridChunk.position = pos;
        gridChunk.size = size;
        gridChunk.index = index;
    }
}
