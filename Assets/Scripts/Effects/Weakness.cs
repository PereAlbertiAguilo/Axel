using System.Collections;
using UnityEngine;

public class Weakness : Effect
{
    public void Start()
    {
        SetEffect();
    }

    private void Update()
    {
        if (currentTime < parameters.durationCurrent)
        {
            currentTime += Time.deltaTime;

            entity.damageCurrent = SetEffectPower(entity.damage, true);
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
        entity.damageCurrent = entity.damage;
    }

    private void OnDestroy()
    {
        EndEffect();
    }
}