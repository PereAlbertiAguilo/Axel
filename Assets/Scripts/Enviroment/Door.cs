using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    float offset = 4f;

    public enum DoorDir 
    { 
        up,
        down, 
        right, 
        left 
    };

    public DoorDir dorDirection;

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Transform playerPos = collision.transform;

            switch (dorDirection)
            {
                case DoorDir.up:
                    playerPos.position = new Vector2(playerPos.position.x, playerPos.position.y + offset);
                    break;
                case DoorDir.down:
                    playerPos.position = new Vector2(playerPos.position.x, playerPos.position.y - offset);
                    break;
                case DoorDir.right:
                    playerPos.position = new Vector2(playerPos.position.x + offset, playerPos.position.y);
                    break;
                case DoorDir.left:
                    playerPos.position = new Vector2(playerPos.position.x - offset, playerPos.position.y);
                    break;
            }

            FadeBlack.instance.FadeFromBlack();
        }
    }
}
