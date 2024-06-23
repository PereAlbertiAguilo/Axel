using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InstanceOnEvent : MonoBehaviour
{
    [SerializeField] GameObject instance;

    enum EventType
    {
        onColliderEnter,
        onDestory
    };

    [SerializeField] EventType eventType;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (eventType == EventType.onColliderEnter && collision.CompareTag("Target")) Instantiate(instance, collision.transform.position, Quaternion.identity);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (eventType == EventType.onColliderEnter && collision.gameObject.CompareTag("Target")) Instantiate(instance, collision.transform.position, Quaternion.identity);
    }

    private void OnDestroy()
    {
        if (eventType == EventType.onDestory) Instantiate(instance, transform.position, Quaternion.identity);
    }

    private void OnDisable()
    {
        if (eventType == EventType.onDestory) Instantiate(instance, transform.position, Quaternion.identity);
    }
}
