using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnHitEnter : MonoBehaviour
{
    [SerializeField] GameObject onDestroyInstance;

    enum InteractionType
    {
        onColliderEnter,
        onDestory
    };

    [SerializeField] InteractionType interactionType;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (interactionType == InteractionType.onColliderEnter && collision.CompareTag("Target")) Instantiate(onDestroyInstance, collision.transform.position, Quaternion.identity);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (interactionType == InteractionType.onColliderEnter && collision.gameObject.CompareTag("Target")) Instantiate(onDestroyInstance, collision.transform.position, Quaternion.identity);
    }

    private void OnDestroy()
    {
        if (interactionType == InteractionType.onDestory) Instantiate(onDestroyInstance, transform.position, Quaternion.identity);
    }

    private void OnDisable()
    {
        if (interactionType == InteractionType.onDestory) Instantiate(onDestroyInstance, transform.position, Quaternion.identity);
    }
}
