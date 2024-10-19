using Pathfinding;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AIPath), typeof(Patrol))]
public class EnemyPatrol : Enemy
{
    protected AIPath _aIPath;
    protected Patrol _patrol;

    public override void Awake()
    {
        base.Awake();

        _aIPath = GetComponent<AIPath>();
        _patrol = GetComponent<Patrol>();
    }

    public override void Update()
    {
        base.Update();

        _aIPath.maxSpeed = speedCurrent;

        _spriteRenderer.flipX = _aIPath.desiredVelocity.x < 0;
    }

    public override void StartMovement()
    {
        base.StartMovement();

        _patrol.enabled = canMove;
        _aIPath.canMove = canMove;
    }

    public override void StopMovement()
    {
        base.StopMovement();

        _patrol.enabled = canMove;
        _aIPath.canMove = canMove;
    }
}

