using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectableSpawner : MonoBehaviour
{
    private void Start()
    {
        
    }

    IEnumerator TrySpawnCollectable()
    {
        yield return new WaitUntil(() => RoomManager.instance.generationComplete);

        
    }
}
