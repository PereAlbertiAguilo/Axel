using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] float speed = 10f;

    GameObject shadow;

    private void Start()
    {
        shadow = transform.GetChild(0).gameObject;
    }

    private void OnEnable()
    {
        if (shadow == null) shadow = transform.GetChild(0).gameObject;
        shadow.transform.rotation = Direction.Rotation(transform.position - Vector3.up, transform.position);
    }

    private void Update()
    {
        transform.Translate(Vector3.down * Time.deltaTime * speed);
    }
}