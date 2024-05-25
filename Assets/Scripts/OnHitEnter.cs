using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnHitEnter : MonoBehaviour
{
    [SerializeField] GameObject onDestroyInstance;

    private void OnDestroy()
    {
        Instantiate(onDestroyInstance, transform.position, Quaternion.identity);
    }

    private void OnDisable()
    {
        Instantiate(onDestroyInstance, transform.position, Quaternion.identity);
    }
}
