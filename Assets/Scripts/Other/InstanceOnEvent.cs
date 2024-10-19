using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InstanceOnHit : MonoBehaviour
{
    public GameObject effect;
    [SerializeField] int poolSize = 10;

    List<GameObject> instances = new List<GameObject>();

    private void Start()
    {
        ObjectPooling.instance.FillPool(instances, effect, transform, poolSize);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Target"))
            ObjectPooling.instance.InstatiateObject(instances, effect, 
                collision.transform.position, Quaternion.identity, transform);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Target"))
            ObjectPooling.instance.InstatiateObject(instances, effect, 
                collision.transform.position, Quaternion.identity, transform);
    }

    private void OnDisable()
    {
        ObjectPooling.instance.EmptyPool(instances);
    }
}
