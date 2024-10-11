using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class RoomStructure
{
    public Vector2 pos;

    public string prefabPath;

    public string name;

    public Vector2Int gridPos;
    public int roomIndex;

    public bool enemiesAlive;
    public bool roomEntered;
    public bool roomCleared;
    public bool roomLocked;
    public bool rewardGiven;

    public bool openUp;
    public bool openDown;
    public bool openRight;
    public bool openLeft;

    public bool doorUpLocked;
    public bool doorDownLocked;
    public bool doorRightLocked;
    public bool doorLeftLocked;

    public RoomStructure(Vector2 pos, string prefabPath, string name, Vector2Int gridPos, int roomIndex,
        bool enemiesAlive, bool roomEntered, bool roomCleared, bool roomLocked, bool rewardGiven,
        bool openUp, bool openDown, bool openRight, bool openLeft,
        bool doorUpLocked, bool doorDownLocked, bool doorRightLocked, bool doorLeftLocked)
    {
        this.pos = pos;
        this.prefabPath = prefabPath;
        this.name = name;
        this.gridPos = gridPos;
        this.roomIndex = roomIndex;
        this.enemiesAlive = enemiesAlive;
        this.roomEntered = roomEntered;
        this.roomCleared = roomCleared;
        this.roomLocked = roomLocked;
        this.rewardGiven = rewardGiven;
        this.openUp = openUp;
        this.openDown = openDown;
        this.openRight = openRight;
        this.openLeft = openLeft;
        this.doorUpLocked = doorUpLocked;
        this.doorDownLocked = doorDownLocked;
        this.doorRightLocked = doorRightLocked;
        this.doorLeftLocked = doorLeftLocked;
    }
}
