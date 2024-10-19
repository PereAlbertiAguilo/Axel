using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Knockback : Effect
{
    Rigidbody2D entityRigidbody;

    public void Start()
    {
        SetEffect();

        parameters.durationCurrent = .2f;

        Vector2 dir = Direction.Normalized(targetEntity.transform.position, attackerEntity.transform.position);

        if (targetEntity.TryGetComponent(out entityRigidbody) && targetEntity.canGetKnockback)
        {
            targetEntity.StopMovement();

            entityRigidbody.velocity = Vector2.zero;
            entityRigidbody.AddForce((dir != null && Vector3.Distance(targetEntity.transform.position, attackerEntity.transform.position) > .1f ?
                dir : Vector2.zero) * (entityRigidbody.mass * SetEffectPower(6000, false) / 50), ForceMode2D.Impulse);

        }
    }

    private void Update()
    {
        if (currentTime < parameters.durationCurrent)
        {
            currentTime += Time.deltaTime;
        }
        else
        {
            EndEffect();

            Destroy(gameObject);
        }
    }

    public override void EndEffect()
    {
        currentTime = parameters.duration;
        entityRigidbody.velocity = Vector2.zero;
        targetEntity.StartMovement();
    }

    private void OnDestroy()
    {
        EndEffect();
    }
}
