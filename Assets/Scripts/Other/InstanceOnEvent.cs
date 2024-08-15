using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectOnDestroy : MonoBehaviour
{
    public GameObject effect;
    [SerializeField] bool effectOnHit = false;
    [SerializeField] int poolSize = 10;

    List<GameObject> instances = new List<GameObject>();

    private void Start()
    {
        FillObjectPool();
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
                instance.transform.position = pos;
                instance.transform.SetParent(null);
                instance.SetActive(true);

                return instance;
            }
        }

        return CreateInstance();
    }

    GameObject CreateInstance()
    {
        GameObject instance = Instantiate(effect, transform.position, Quaternion.identity);
        instance.transform.SetParent(transform);
        instance.SetActive(false);

        instances.Add(instance);

        return instance;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Target") && effectOnHit) InstanceEffect(collision.transform.position);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Target") && effectOnHit) InstanceEffect(collision.transform.position);
    }

    private void OnDestroy()
    {
        //InstanceEffect(transform.position);
    }
}
