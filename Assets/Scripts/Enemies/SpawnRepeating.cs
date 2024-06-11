using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnRepeating : MonoBehaviour
{
    Transform target;

    [SerializeField] float fireRate;

    [SerializeField] float startDelay;

    [SerializeField] bool aimToPlayer = false;

    [Range(-1f, 1f)]
    [SerializeField] float directionX;
    [Range(-1f, 1f)]
    [SerializeField] float directionY;

    [SerializeField] GameObject shootingObject;

    Quaternion rotation;

    private void Start()
    {
        target = FindObjectOfType<PlayerController>().transform;

        var angle = (Mathf.Atan2(directionY, directionX) * Mathf.Rad2Deg) + 90;
        rotation = Quaternion.AngleAxis(angle, Vector3.forward);

        InvokeRepeating(nameof(Shoot), startDelay, fireRate);
    }

    public void Shoot()
    {
        if (aimToPlayer && target != null)
        {
            Vector3 direction = target.position - transform.position;
            var angle = (Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg) + 90;
            rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        }

        Instantiate(shootingObject, transform.position, rotation);
    }
}
