using Pathfinding;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AIPath), typeof(Patrol))]
public class EnemyPatrol : Enemy
{
    AIPath _aIPath;
    Patrol _patrol;

    public override void Awake()
    {
        base.Awake();

        _aIPath = GetComponent<AIPath>();
        _patrol = GetComponent<Patrol>();
    }

    private void Update()
    {
        _aIPath.maxSpeed = speedCurrent;
    }

    public override void MoveReset()
    {
        base.MoveReset();

        _aIPath.canMove = canMove;
    }

    public override void DeactivateFollowState()
    {
        base.DeactivateFollowState();

        _aIPath.canMove = canMove;
    }
}

