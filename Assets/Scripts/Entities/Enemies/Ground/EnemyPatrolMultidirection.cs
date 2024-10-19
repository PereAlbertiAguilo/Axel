using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class EnemyPatrolMultidirection : EnemyPatrol
{
    Animator _animator;

    float offset = 2f;
    public LayerMask enemiesLayer;

    public override void Awake()
    {
        base.Awake();

        _animator = GetComponent<Animator>();
    }

    public override void Update()
    {
        base.Update();

        RaycastHit2D[] hits = Physics2D.RaycastAll(transform.position, _aIPath.desiredVelocity, offset, enemiesLayer);

        foreach (RaycastHit2D hit in hits)
        {
            if (hit.fraction != 0 && hit.distance <= offset) 
                StopMovement();
            else 
                StartMovement();
        }

        _animator.SetFloat("Horizontal", _aIPath.desiredVelocity.x);
        _animator.SetFloat("Vertical", _aIPath.desiredVelocity.y);
    }
}
