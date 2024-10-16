using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpawnRepeating : MonoBehaviour
{
    [HideInInspector] public float fireRate;
    [HideInInspector] public float startDelay;
    [HideInInspector] public bool aimToPlayer = false;
    [HideInInspector] public float directionX;
    [HideInInspector] public float directionY;
    [HideInInspector] public GameObject shootingObject;
    [HideInInspector] public int poolSize = 10;

    protected List<GameObject> instances = new List<GameObject>();

    float currentTime = 0;
    Quaternion rotation;

    public virtual void Start()
    {
        FillObjectPool();

        rotation = Direction.Rotation(new Vector2(directionX, directionY), Vector2.zero);
    }

    public virtual void Update()
    {
        if(currentTime < fireRate)
        {
            currentTime += Time.deltaTime;
        }
        else
        {
            Shoot();
            currentTime = 0;
        }
    }

    void FillObjectPool()
    {
        for (int i = 0; i < poolSize; i++)
        {
            CreateInstance();
        }
    }

    GameObject InstanceEffect(Vector3 pos)
    {
        foreach (GameObject instance in instances)
        {
            if (!instance.activeInHierarchy)
            {
                instance.SetActive(true);
                instance.transform.position = pos;
                instance.transform.SetParent(null);
                instance.transform.rotation = rotation;

                return instance;
            }
        }

        return CreateInstance();
    }

    GameObject CreateInstance()
    {
        GameObject instance = Instantiate(shootingObject, transform);
        instance.SetActive(false);

        instances.Add(instance);

        return instance;
    }


    public void Shoot()
    {
        if (aimToPlayer)
        {
            rotation = Direction.Rotation(PlayerController.instance.transform.position, transform.position);
        }

        InstanceEffect(transform.position);
    }

    private void OnDisable()
    {
        foreach (GameObject instance in instances)
        {
            Destroy(instance);
        }
    }
}
