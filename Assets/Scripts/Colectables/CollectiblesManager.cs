using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;

public class CollectiblesManager : MonoBehaviour
{
    public enum Types
    {
        gems, planks
    };

    public enum Prefabs
    {
        gem1, gem2, gem3, planks
    };

    public int gems = 0;
    public int planks = 0;

    public FieldInfo[] properties = typeof(CollectiblesManager).GetFields();

    public List<Collectable> collectibles = new List<Collectable>();

    public int index = 0;

    public static CollectiblesManager instance;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        UpdateCollectable(false);

        if(DataPersistenceManager.instance.gameData.collectableStructures != null)
        {
            LoadCollectibles();
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            SpawnCollectable(Prefabs.planks, transform.position + Vector3.down, index);
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

    public Collectable SpawnCollectable(Prefabs prefabName, Vector2 pos, int index)
    {
        string prefabPath = "Collectables/" + prefabName.ToString();

        GameObject prefab = Resources.Load(prefabPath) as GameObject;
        GameObject instance = Instantiate(prefab, pos, Quaternion.identity);

        Collectable collectable = instance.GetComponent<Collectable>();

        this.index++;

        collectable.index = index;
        collectable.prefabPath = prefabPath;

        collectibles.Add(collectable);

        SaveCollectibles();

        return collectable;
    }

    public Prefabs GetRandomCollectable()
    {
        return (Prefabs)UnityEngine.Random.Range(0, Enum.GetValues(typeof(Prefabs)).Length);
    }

    public void SaveCollectibles()
    {
        List<CollectableStructure> collectableStructures = new List<CollectableStructure>();

        foreach (Collectable collectable in collectibles)
        {
            CollectableStructure collectableStructure = new CollectableStructure();

            collectableStructure.type = (int)collectable.collectable;
            collectableStructure.prefabPath = collectable.prefabPath;
            collectableStructure.pos = collectable.transform.position;
            collectableStructure.index = collectable.index;

            collectableStructures.Add(collectableStructure);
        }

        DataPersistenceManager.instance.gameData.collectableStructures = collectableStructures.ToArray();
    }

    public void LoadCollectibles()
    {
        foreach (CollectableStructure collectableStructure in DataPersistenceManager.instance.gameData.collectableStructures)
        {
            SpawnCollectable((Prefabs)collectableStructure.type, collectableStructure.pos, collectableStructure.index);
        }
    }
}
