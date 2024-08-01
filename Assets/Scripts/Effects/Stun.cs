using System.Collections;
using System.Security.Cryptography;
using UnityEngine;

public class Stun : Effect
{
    public void Start()
    {
        SetEffect();

        parameters.durationCurrent = SetEffectPower(parameters.duration, false);
    }

    private void Update()
    {
        if (currentTime < parameters.durationCurrent)
        {
            currentTime += Time.deltaTime;

            entity.canMove = false;
            entity.canDealDamage = false;
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
        entity.canMove = true;
        entity.canDealDamage = true;
    }

    private void OnDestroy()
    {
        EndEffect();
    }
}