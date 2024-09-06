using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPooling : MonoBehaviour
{
    public static ObjectPooling instance;

    private void Awake()
    {
        instance = this;
    }

    public void FillPool(List<GameObject> instances, GameObject objectToInstantiate, Transform parent)
    {
        for (int i = 0; i < 24; i++)
        {
            CreateInstance(instances, objectToInstantiate, parent);
        }
    }

    public GameObject InstatiateObject(List<GameObject> instances, GameObject objectToInstantiate, Vector2 pos, Quaternion rotation, Transform parent)
    {
        foreach (GameObject instance in instances)
        {
            if (!instance.activeInHierarchy)
            {
                return ActivateInstance(instance, pos, rotation);
            }
        }

        GameObject newInstance = CreateInstance(instances, objectToInstantiate, parent);

        return ActivateInstance(newInstance, pos, rotation);
    }

    public GameObject CreateInstance(List<GameObject> instances, GameObject objectToInstantiate, Transform parent)
    {
        GameObject instance = Instantiate(objectToInstantiate, parent);
        instance.SetActive(false);

        instances.Add(instance);

        return instance;
    }

    public GameObject ActivateInstance(GameObject instance, Vector2 pos, Quaternion rotation)
    {
        instance.transform.position = pos;
        instance.transform.rotation = rotation;
        instance.transform.SetParent(null);
        instance.SetActive(true);

        return instance;
    }
}
