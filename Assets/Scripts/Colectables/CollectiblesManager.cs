using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CollectiblesManager : MonoBehaviour
{
    public enum Types
    {
        gems, planks
    };

    public enum Prefabs1
    {
        gem1
    };
    public enum Prefabs2
    {
        gem2
    };
    public enum Prefabs3
    {
        gem3, planks
    };

    public int gems = 0;
    public int planks = 0;

    public FieldInfo[] properties = typeof(CollectiblesManager).GetFields();

    public bool collectiblesLoaded = false;

    public static CollectiblesManager instance;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        UpdateCollectable(false);

        if(DataPersistenceManager.instance.gameData.collectableStructures != null && PlayerPrefs.HasKey(SceneManager.GetActiveScene().name))
        {
            StartCoroutine(LoadCollectibles());
        }
        else
        {
            collectiblesLoaded = true;
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            SpawnCollectable("Collectables/3/planks", transform.position + Vector3.down, RoomManager.instance.currentRoom);
        }
    }

    public void UpdateCollectable(bool clear)
    {
        foreach (Types colectable in Enum.GetValues(typeof(Types)))
        {
            if (clear)
            {
                PlayerPrefs.DeleteKey(colectable.ToString());
            }
            else
            {
                if (PlayerPrefs.HasKey(colectable.ToString())) SetCollectable(colectable, PlayerPrefs.GetInt(colectable.ToString()));
            }
        }

        HudManager.instance.UpdateCollectableUI();
    }

    public void SetCollectable(Types colectable, int amount)
    {
        FieldInfo colectableProperty = GetCollectable(colectable);
        int totalValue = ((int)colectableProperty.GetValue(this)) + amount;
        colectableProperty.SetValue(this, totalValue);

        PlayerPrefs.SetInt(colectable.ToString(), totalValue);

        HudManager.instance.UpdateCollectableUI();
    }

    public FieldInfo GetCollectable(Types colectable)
    {
        FieldInfo property = properties.ToList().Find(p => p.Name == colectable.ToString());

        return property;
    }

    public Collectable SpawnCollectable(string prefabPath, Vector2 pos, Room room)
    {
        GameObject prefab = Resources.Load(prefabPath) as GameObject;
        GameObject instance = Instantiate(prefab, pos, Quaternion.identity);
        Collectable collectable = instance.GetComponent<Collectable>();

        collectable.room = room;
        collectable.index = room.roomCollectibles.index;
        collectable.prefabPath = prefabPath;

        room.roomCollectibles.collectibles.Add(collectable);
        instance.transform.parent = room.roomCollectibles.transform;

        room.roomCollectibles.index++;

        SaveCollectibles();

        return collectable;
    }

    public string GetRandomCollectable()
    {
        float randomValue = UnityEngine.Random.value;
        string prefabName = "Collectables/";

        if (randomValue < .5f) 
            prefabName += "3/" + ((Prefabs3)UnityEngine.Random.Range(0, Enum.GetValues(typeof(Prefabs3)).Length)).ToString();
        else if (randomValue < .75f) 
            prefabName += "2/" + ((Prefabs2)UnityEngine.Random.Range(0, Enum.GetValues(typeof(Prefabs2)).Length)).ToString();
        else 
            prefabName += "1/" + ((Prefabs1)UnityEngine.Random.Range(0, Enum.GetValues(typeof(Prefabs1)).Length)).ToString();

        return prefabName;
    }

    public void SaveCollectibles()
    {
        List<CollectableStructure> collectableStructures = new List<CollectableStructure>();

        foreach (GameObject roomInstance in RoomManager.instance.rooms)
        {
            Room room = roomInstance.GetComponent<Room>();

            foreach (Collectable collectable in room.roomCollectibles.collectibles)
            {
                CollectableStructure collectableStructure = new(room.roomIndex, collectable.index, collectable.pickedUp,
                    collectable.transform.position, collectable.prefabPath);

                collectableStructures.Add(collectableStructure);
            }
        }

        if(collectableStructures.Count > 0) DataPersistenceManager.instance.gameData.collectableStructures = collectableStructures.ToArray();
    }

    public IEnumerator LoadCollectibles()
    {
        yield return new WaitUntil(() => RoomManager.instance.generationComplete);

        foreach (GameObject roomInstance in RoomManager.instance.rooms)
        {
            Room room = roomInstance.GetComponent<Room>();

            foreach (CollectableStructure collectableStructure in DataPersistenceManager.instance.gameData.collectableStructures)
            {
                if(collectableStructure.roomIndex == room.roomIndex)
                {
                    if (!collectableStructure.pickedUp) SpawnCollectable(collectableStructure.prefabPath, collectableStructure.pos, room);
                }
            }
        }

        collectiblesLoaded = true;
    }

    public bool IsCollectableSaved(CollectableStructure collectableStructure)
    {
        bool isCollectableSaved = false;

        foreach (CollectableStructure savedStructure in DataPersistenceManager.instance.gameData.collectableStructures)
        {
            if (savedStructure.roomIndex == collectableStructure.roomIndex &&
                savedStructure.prefabPath == collectableStructure.prefabPath &&
                savedStructure.pos == collectableStructure.pos)
            {
                isCollectableSaved = true;
            }
        }

        return isCollectableSaved;
    }
}
