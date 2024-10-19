using Pathfinding;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AIPath), typeof(AIDestinationSetter))]
public class EnemyGroundFollow : Enemy
{
    AIPath _aIPath;
    AIDestinationSetter _destinationSetter;

    public override void Awake()
    {
        base.Awake();

        _aIPath = GetComponent<AIPath>();
        _destinationSetter = GetComponent<AIDestinationSetter>();
    }

    public override void Start()
    {
        base.Start();

        _destinationSetter.target = PlayerController.instance.transform;
    }

    public override void Update()
    {
        base.Update();

        _aIPath.maxSpeed = speedCurrent;
    }

    public override void StartMovement()
    {
        base.StartMovement();

        _aIPath.canMove = canMove;
    }

    public override void StopMovement()
    {
        base.StopMovement();

        _aIPath.canMove = canMove;
    }
}
