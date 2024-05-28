using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridChunkTest : MonoBehaviour
{
    public bool hasRoom = false;

    public int index = 0;

    public Vector2 position;

    public Vector2 size;

    public int roomDirection = 0;

    private void OnDrawGizmos()
    {
        Gizmos.color = new Color(1, 1, 1, 0.1f);

        Gizmos.DrawWireCube(position, new Vector2(size.x, size.y));
    }
}
