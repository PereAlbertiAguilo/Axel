using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowingAttack : MonoBehaviour
{
    [SerializeField] float speed = 10f;

    private void Update()
    {
        transform.Translate(Vector3.down * Time.deltaTime * speed);
    }
}