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
        ObjectPooling.instance.FillPool(instances, shootingObject, transform, poolSize);

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

    public void Shoot()
    {
        if (aimToPlayer)
        {
            rotation = Direction.Rotation(PlayerController.instance.transform.position, transform.position);
        }

        ObjectPooling.instance.InstatiateObject(instances, shootingObject, transform.position, rotation, transform);
    }

    private void OnDisable()
    {
        ObjectPooling.instance.EmptyPool(instances);
    }
}
