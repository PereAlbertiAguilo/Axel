using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class RoomCollectable : MonoBehaviour
{
    public Room room;

    [HideInInspector, Range(1, 3)] public int tier = 1;
    [HideInInspector] public CollectiblesManager.Prefabs1 collectableTierOne;
    [HideInInspector] public CollectiblesManager.Prefabs2 collectableTierTwo;
    [HideInInspector] public CollectiblesManager.Prefabs3 collectableTierThree;

    private void Start()
    {
        StartCoroutine(SpawnCollectable());
    }

    IEnumerator SpawnCollectable()
    {
        string prefabPath = "Collectables/";

        switch (tier)
        {
            case 1:
                prefabPath += "1/" + collectableTierOne.ToString();
                break;
            case 2:
                prefabPath += "2/" + collectableTierTwo.ToString();
                break;
            case 3:
                prefabPath += "3/" + collectableTierThree.ToString();
                break;
        }

        yield return new WaitUntil(() => CollectiblesManager.instance.collectiblesLoaded);

        CollectableStructure collectableStructure = new CollectableStructure(room.roomIndex, room.roomCollectibles.index, false, transform.position, prefabPath);

        if (!CollectiblesManager.instance.IsCollectableSaved(collectableStructure)) 
            CollectiblesManager.instance.SpawnCollectable(prefabPath, transform.position, room);
    }
}
