using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class RoomStructure
{
    public Vector2 pos;

    public string prefabPath;

    public Vector2Int gridPos;
    public int roomIndex;

    public bool enemiesAlive;

    public bool roomEntered;
    public bool roomCleared;

    public bool openUp;
    public bool openDown;
    public bool openRight;
    public bool openLeft;
}
