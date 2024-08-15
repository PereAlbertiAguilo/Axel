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

        Vector2 dir = Direction.Normalized(entity.transform.position, attakerEntity.transform.position);

        if (entity.TryGetComponent(out entityRigidbody) && entity.canGetKnockback)
        {
            entity.canMove = false;

            entityRigidbody.velocity = Vector2.zero;
            entityRigidbody.AddForce((dir != null && Vector3.Distance(entity.transform.position, attakerEntity.transform.position) > .1f ?
                dir : Vector2.zero) * (entityRigidbody.mass * SetEffectPower(1200, false) / 50), ForceMode2D.Impulse);

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
        entity.canMove = true;
    }

    private void OnDestroy()
    {
        EndEffect();
    }
}
