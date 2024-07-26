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
        if (currentTime < effectParameters.duration)
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
        currentTime = effectParameters.duration;
        entity.damageCurrent = entity.damage;
    }

    private void OnDestroy()
    {
        EndEffect();
    }
}