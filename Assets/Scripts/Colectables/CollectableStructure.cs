using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class CollectableStructure
{
    public int roomIndex;
    public int index;
    public bool pickedUp;
    public Vector2 pos;
    public string prefabPath;

    public CollectableStructure(int roomIndex, int index, bool pickedUp, Vector2 pos, string prefabPath)
    {
        this.roomIndex = roomIndex;
        this.index = index;
        this.pickedUp = pickedUp;
        this.pos = pos;
        this.prefabPath = prefabPath;
    }
}
