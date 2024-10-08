using Pathfinding;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEngine;

public class EnemyAirFollow : Enemy
{
    Transform target;

    public float entitySpeed;

    public override void Start()
    {
        base.Start();

        target = PlayerController.instance.transform;

        entitySpeed = speedCurrent;
    }

    public override void Update()
    {
        base.Update();

        if (canMove)
        {
            if (Vector2.Distance(transform.position, target.position) < 4f)
            {
                entitySpeed = Mathf.Lerp(entitySpeed, speedCurrent * 2, Time.deltaTime * 5);
            }
            else
            {
                entitySpeed = Mathf.Lerp(entitySpeed, speedCurrent, Time.deltaTime * 5);
            }
        }
    }

    void FixedUpdate()
    {
        if (canMove)
        {
            Vector3 direction = Direction.Normalized(target.position, transform.position);

            _enemyRigidbody.AddForce(direction * Time.deltaTime * entitySpeed * 1000, ForceMode2D.Force);
        }
    }
}